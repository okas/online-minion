using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace OnlineMinion.DataStore.Helpers;

public static class ModelConfigurationBuilderHelpers
{
    [RequiresUnreferencedCode("Gets all types from the given assembly - unsafe for trimming")]
    public static IEnumerable<TypeInfo> GetConstructibleTypes(this Assembly assembly)
        => assembly.GetLoadableDefinedTypes()
            .Where(
                t => t is { IsAbstract: false, IsGenericTypeDefinition: false, }
            );

    [RequiresUnreferencedCode("Gets all types from the given assembly - unsafe for trimming")]
    public static IEnumerable<TypeInfo> GetLoadableDefinedTypes(this Assembly assembly)
    {
        try
        {
            return assembly.DefinedTypes;
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t != null).Select(IntrospectionExtensions.GetTypeInfo!);
        }
    }

    /// <summary>
    ///     Applies configuration from all <see cref="ValueConverter{TModel,TProvider}" /> derived class
    ///     instances that are defined in provided assembly.
    /// </summary>
    /// <remarks>
    ///     See
    ///     <a href="https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations">
    ///         Value Conversions
    ///     </a>
    ///     for more information and examples.
    /// </remarks>
    /// <param name="modelBuilderInstance">Builder instance to configure.</param>
    /// <param name="assembly">The assembly to scan.</param>
    /// <returns>
    ///     The same <see cref="ModelConfigurationBuilder" /> instance so that additional configuration calls can be chained.
    /// </returns>
    [RequiresUnreferencedCode(
        "This API isn't safe for trimming, since it searches for types in an arbitrary assembly."
    )]
    public static ModelConfigurationBuilder ApplyValueConversionsFromAssembly(
        this ModelConfigurationBuilder modelBuilderInstance,
        Assembly                       assembly
    )
    {
        var methodProperties = GetMethodProperties();

        var methodHaveConversion = GetMethodHaveConversation();

        foreach (var type in GetTypesToAnalyze(assembly))
        {
            if (ValueConverterBaseTypeFilter(type, out var genericTypeBaseValueConverter))
            {
                ApplyConfigurationToModelBuilder(
                    modelBuilderInstance,
                    genericTypeBaseValueConverter,
                    type,
                    methodProperties,
                    methodHaveConversion
                );
            }
        }

        return modelBuilderInstance;
    }

    private static IOrderedEnumerable<TypeInfo> GetTypesToAnalyze(Assembly assembly) => assembly
        .GetConstructibleTypes()
        .OrderBy(t => t.FullName, StringComparer.Ordinal);

    /// <summary>
    ///     Gets <see cref="MethodInfo" /> for the <see cref="ModelConfigurationBuilder.Properties(Type)" /> method.
    /// </summary>
    /// <remarks>
    ///     No need to bother with the generic type argument, since non-generic method does the same thing.
    /// </remarks>
    private static MethodInfo GetMethodProperties() => typeof(ModelConfigurationBuilder).GetMethods()
        .Single(
            mi => mi is
                  {
                      Name: nameof(ModelConfigurationBuilder.Properties),
                      ContainsGenericParameters: false,
                      ReturnType.IsGenericType: false,
                  }
                  && mi.ReturnType == typeof(PropertiesConfigurationBuilder)
        );

    /// <summary>
    ///     Gets <see cref="MethodInfo" /> for the <see cref="PropertiesConfigurationBuilder.HaveConversion(Type)" /> method.
    /// </summary>
    /// <remarks>
    ///     No need to bother with the generic type argument, since non-generic method does the same thing.
    /// </remarks>
    private static MethodInfo GetMethodHaveConversation() => typeof(PropertiesConfigurationBuilder<>).GetMethods()
        .Single(
            mi => mi is
                  {
                      Name: nameof(PropertiesConfigurationBuilder.HaveConversion),
                      ContainsGenericParameters: false,
                      ReturnType.IsGenericType: false,
                  }
                  && mi.GetParameters().Length == 1
                  && mi.ReturnType == typeof(PropertiesConfigurationBuilder)
        );

    /// <summary>
    ///     Only accept types that contain a parameterless constructor and are assignable from
    ///     <see cref="ValueConverter{TModel,TProvider}" />
    /// </summary>
    /// <param name="type"></param>
    /// <param name="genericBaseValueConverterType"></param>
    /// <returns></returns>
    private static bool ValueConverterBaseTypeFilter(
        Type                          type,
        [NotNullWhen(true)] out Type? genericBaseValueConverterType
    )
    {
        genericBaseValueConverterType = null;

        if (type.GetConstructor(Type.EmptyTypes) is null
            || type.BaseType is not { IsGenericType: true, } testType
            || testType.GetGenericTypeDefinition() != typeof(ValueConverter<,>))
        {
            return false;
        }

        genericBaseValueConverterType = testType;
        return true;
    }

    /// <summary>
    ///     Configures provided <paramref name="modelBuilder" /> using provided <paramref name="conversionType" />.
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="genericTypeBaseValueConverter"></param>
    /// <param name="conversionType"></param>
    /// <param name="methodProperties"></param>
    /// <param name="methodHaveConversion"></param>
    private static void ApplyConfigurationToModelBuilder(
        ModelConfigurationBuilder modelBuilder,
        Type                      genericTypeBaseValueConverter,
        Type                      conversionType,
        MethodBase                methodProperties,
        MethodBase                methodHaveConversion
    )
    {
        var typedIdGenericTypeArgument = genericTypeBaseValueConverter.GenericTypeArguments[0];

        var propertiesConfigurationBuilder = methodProperties.Invoke(
            modelBuilder,
            new object?[] { typedIdGenericTypeArgument, }
        );

        methodHaveConversion.Invoke(
            propertiesConfigurationBuilder,
            new object?[] { conversionType, }
        );
    }
}
