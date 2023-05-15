using System.Runtime.CompilerServices;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OnlineMinion.Data.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message, UniqueConstraintException ex) : base(message) =>
        Errors = GetErrorData(ex.Entries, ex.Message, ex.InnerException!.Message);

    public Dictionary<string, string[]> Errors { get; init; }

    private Dictionary<string, string[]> GetErrorData(
        IReadOnlyList<EntityEntry> uniqueConstraintExceptionEntries,
        string                     dbUpdateExMessage,
        string                     sqlExMessage
    )
    {
        CheckEntries(uniqueConstraintExceptionEntries);

        var relationalModel = uniqueConstraintExceptionEntries.First().Context.Model.GetRelationalModel();

        var distinctTypeIndex = 0;
        var errors = new Dictionary<string, string[]>();

        var distinctEntriesByType = uniqueConstraintExceptionEntries.DistinctBy(
            e => e.Metadata.ClrType
        );

        foreach (var entityEntry in distinctEntriesByType)
        {
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
                )
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
                errors.Add(
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
}
