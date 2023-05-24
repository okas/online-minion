using System.Runtime.CompilerServices;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OnlineMinion.Data.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message, UniqueConstraintException ex) : base(message) =>
        Errors = GetErrorData(ex.Entries, ex.Message, ex.InnerException!.Message);

    public IList<ErrorDescriptor> Errors { get; init; }

    private static IList<ErrorDescriptor> GetErrorData(
        IReadOnlyList<EntityEntry> uniqueConstraintExceptionEntries,
        string                     dbUpdateExMessage,
        string                     sqlExMessage
    )
    {
        CheckEntries(uniqueConstraintExceptionEntries);

        var relationalModel = uniqueConstraintExceptionEntries.First().Context.Model.GetRelationalModel();

        var distinctTypeIndex = 0;
        var errors = new List<ErrorDescriptor>();

        var distinctEntriesByType = uniqueConstraintExceptionEntries.DistinctBy(
            e => e.Metadata.ClrType
        );

        foreach (var entityEntry in distinctEntriesByType)
        {
            var descriptor = new ErrorDescriptor(
                entityEntry.Metadata.GetKeys().ToString(), // TODO: what does it output?
                new Dictionary<string, string[]>(StringComparer.Ordinal)
            );
            errors.Add(descriptor);

            var tablesRelatedToEntry = relationalModel
                .Tables.Where(
                    t => t.EntityTypeMappings.Any(
                        tm => tm.EntityType.ClrType == entityEntry.Metadata.ClrType
                    )
                );

            var allEntryUniqueIndices = tablesRelatedToEntry
                .SelectMany(t => t.Indexes)
                .Where(i => i.IsUnique);

            var groupsOfIndexAndEntryPropertyNames = allEntryUniqueIndices.GroupBy(
                i => i.Name,
                i => i.MappedIndexes.SelectMany(
                    mi => mi.Properties.Select(
                        p => p.Name
                    )
                ),
                StringComparer.Ordinal
            );

            var violatedIndices = groupsOfIndexAndEntryPropertyNames.Where(
                gIdxPropNames => sqlExMessage.Contains(
                    gIdxPropNames.Key,
                    StringComparison.InvariantCultureIgnoreCase
                )
            );

            var erroneousPropertyNames = violatedIndices.SelectMany(
                gIdxPropNames => gIdxPropNames.SelectMany(names => names)
            );

            foreach (var propertyName in erroneousPropertyNames)
            {
                descriptor.Details.Add(
                    $"[{distinctTypeIndex}].{entityEntry.Metadata.ClrType.Name}.{propertyName}",
                    new[] { dbUpdateExMessage, }
                );
            }

            distinctTypeIndex++;
        }

        return errors;
    }

    private static void CheckEntries(
        IReadOnlyList<EntityEntry> uniqueConstraintExceptionEntries,
        [CallerArgumentExpression("uniqueConstraintExceptionEntries")]
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

    public record ErrorDescriptor(object? InstanceId, IDictionary<string, string[]> Details);
}
