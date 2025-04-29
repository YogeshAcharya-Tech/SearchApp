using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace SearchApp.Api
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                var enumType = context.Type;
                var enumNames = Enum.GetNames(enumType);

                schema.Enum.Clear();
                foreach (var name in enumNames)
                {
                    var member = enumType.GetMember(name).First();
                    var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
                    var displayName = displayAttribute?.Name ?? name;
                    schema.Enum.Add(new OpenApiString(displayName));
                }
            }
        }
    }
}
