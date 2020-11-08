using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Todo.Extensions.Swaggers
{
    public class SwaggerJsonIgnore : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var ignoredProperties = context.MethodInfo.GetParameters()
                .SelectMany(p => p.ParameterType.GetProperties()
                    .Where(prop => prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)).ToList();
            if (ignoredProperties.Any())
            {
                foreach (var property in ignoredProperties)
                {
                    operation.Parameters = operation.Parameters
                        .Where(p => (!p.Name.Equals(property.Name, StringComparison.InvariantCulture)
                                     && !p.Name.StartsWith($"{property.Name}.", StringComparison.InvariantCulture)
                                     && !p.Name.Equals("route", StringComparison.InvariantCultureIgnoreCase)))
                        .ToList();
                }
            }
        }
    }
}