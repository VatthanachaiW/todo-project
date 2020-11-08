using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Todo.Extensions.Swaggers
{
    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            //api/v{version}/ <= v1
            var paths = swaggerDoc.Paths
                .ToDictionary(
                    path => path.Key.Replace("v{version}", swaggerDoc.Info.Version.Replace("v", "").VersionFormatter()),
                    path => path.Value
                ).ToList();
            swaggerDoc.Paths.Clear();
            paths.ForEach(path => swaggerDoc.Paths.Add(path.Key, path.Value));
        }
    }
}