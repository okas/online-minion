using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OnlineMinion.RestApi.Swagger;

/// <summary>
///     Produces enum values as strings in Swagger UI: <c>$"{name} = {value}"</c>
/// </summary>
[UsedImplicitly]
public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
        {
            return;
        }

        model.Enum.Clear();

        var names = Enum.GetNames(context.Type);

        ref var firstItemRef = ref MemoryMarshal.GetArrayDataReference(names);

        for (var i = 0; i < names.Length; i++)
        {
            var name = Unsafe.Add(ref firstItemRef, i);

            var value = ((int)Enum.Parse(context.Type, name)).ToString(CultureInfo.InvariantCulture);

            model.Enum.Add(new OpenApiString($"{name} = {value}"));
        }
    }
}
