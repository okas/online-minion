using System.Runtime.CompilerServices;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OnlineMinion.Data.Exceptions;

public class ConflictException(string message, UniqueConstraintException ex) : Exception(message)
{
    public IList<ErrorDescriptor> Errors { get; } = GetErrorData(
        ex.Entries,
        ex.Message,
        ex.InnerException!.Message
    );

    private static List<ErrorDescriptor> GetErrorData(
        IReadOnlyList<EntityEntry> uniqueConstraintExceptionEntries,
        string                     dbUpdateExMessage,
        string                     sqlExMessage
    )
    {
        CheckEntries(uniqueConstraintExceptionEntries);

        var relationalModel = uniqueConstraintExceptionEntries[0].Context.Model.GetRelationalModel();

        var distinctTypeIndex = 0;

        var list = new List<ErrorDescriptor>();

        foreach (var entityEntry in uniqueConstraintExceptionEntries.DistinctBy(e => e.Metadata.ClrType))
        {
            var allEntryUniqueIndices = FindAllEntryUniqueIndices(relationalModel, entityEntry);

            var groupedProperties = GroupEntryPropertyNamesByIndices(allEntryUniqueIndices);

            var violatedIndices = FindViolatedIndices(sqlExMessage, groupedProperties);

            var erroneousPropertyNames = GetAllPropertyNames(violatedIndices);

            list.Add(
                TransformToErrorDescriptor(
                    distinctTypeIndex++,
                    dbUpdateExMessage,
                    entityEntry,
                    erroneousPropertyNames
                )
            );
        }

        return list;
    }

    private static void CheckEntries(
        IReadOnlyList<EntityEntry> uniqueConstraintExceptionEntries,
        [CallerArgumentExpression(nameof(uniqueConstraintExceptionEntries))]
        string? argumentExpression = null
    )
    {
        if (uniqueConstraintExceptionEntries is not { Count: > 0, })
        {
            throw new ArgumentException(
                $"Cannot collect error data for {nameof(ConflictException)}, no entity entries info.",
                nameof(uniqueConstraintExceptionEntries)
            );
        }
    }

    private static IEnumerable<ITableIndex> FindAllEntryUniqueIndices(
        IRelationalModel relationalModel,
        EntityEntry      entityEntry
    )
    {
        var uniqueIndices = relationalModel.Tables
            .Where(table => EntityTableMatcher(table, entityEntry))
            .SelectMany(t => t.Indexes)
            .Where(i => i.IsUnique);

        return uniqueIndices;
    }

    private static bool EntityTableMatcher(ITable table, EntityEntry entityEntry) =>
        table.EntityTypeMappings.Any(
            tableMapping => tableMapping.TypeBase.ClrType == entityEntry.Metadata.ClrType
        );

    private static IEnumerable<IGrouping<string, IEnumerable<string>>> GroupEntryPropertyNamesByIndices(
        IEnumerable<ITableIndex> allEntryUniqueIndices
    ) => allEntryUniqueIndices.GroupBy(
        i => i.Name,
        i => i.MappedIndexes.SelectMany(
            mi => mi.Properties.Select(
                p => p.Name
            )
        ),
        StringComparer.Ordinal
    );

    private static IEnumerable<IGrouping<string, IEnumerable<string>>> FindViolatedIndices(
        string                                              exceptionMessage,
        IEnumerable<IGrouping<string, IEnumerable<string>>> groupedProperties
    ) => groupedProperties.Where(
        grIdxPropNames => exceptionMessage.Contains(
            grIdxPropNames.Key,
            StringComparison.InvariantCultureIgnoreCase
        )
    );

    private static IEnumerable<string> GetAllPropertyNames(
        IEnumerable<IGrouping<string, IEnumerable<string>>> violatedIndices
    ) => violatedIndices
        .SelectMany(grIdxPropNames => grIdxPropNames)
        .SelectMany(names => names);

    private static ErrorDescriptor TransformToErrorDescriptor(
        int                 distinctTypeIndex,
        string              dbUpdateExMessage,
        EntityEntry         entityEntry,
        IEnumerable<string> erroneousPropertyNames
    )
    {
        var descriptor = new ErrorDescriptor(entityEntry.Metadata.GetKeys().ToString());

        foreach (var propertyName in erroneousPropertyNames)
        {
            descriptor.Details.Add(
                CreateMemberNameKey(distinctTypeIndex, entityEntry, propertyName),
                new[] { dbUpdateExMessage, }
            );
        }

        return descriptor;
    }

    private static string CreateMemberNameKey(int distinctTypeIndex, EntityEntry entityEntry, string propertyName) =>
        $"[{distinctTypeIndex}].{entityEntry.Metadata.ClrType.Name}.{propertyName}";

    public record ErrorDescriptor(object? InstanceId)
    {
        public IDictionary<string, string[]> Details { get; } =
            new Dictionary<string, string[]>(StringComparer.Ordinal);
    }
}
