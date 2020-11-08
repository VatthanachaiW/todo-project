using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Todo.Extensions.Swaggers
{
    public static class DocInclusionPredicateExtension
    {
        public static bool ApplyPredicate(this ApiDescription desc, string version, HttpContext httpContext)
        {
            if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

            //var versions = methodInfo.DeclaringType
            var versions = methodInfo.ReflectedType?.GetCustomAttributes(true)
                .OfType<ApiVersionAttribute>()
                .SelectMany(s => s.Versions)
                .ToList();

            MapToApiVersionAttribute attr = null;
            if (desc.TryGetMethodInfo(out MethodInfo attrMethod) && attrMethod != null) attr = attrMethod.GetCustomAttribute<MapToApiVersionAttribute>(true);

            bool isMatchVersion;
            if (attr == null)
            {
                isMatchVersion = versions!.Any(v => v.ToString().VersionFormatter() == version);
            }
            else
            {
                var maps = attr.Versions;
                isMatchVersion = versions!.Any(v => v.ToString().VersionFormatter() == version) && (!maps.Any() || maps.Any(v => v.ToString().VersionFormatter() == version));
            }

            if (isMatchVersion)
            {
                var filter = httpContext.Request.Query["filter"].ToString();
                var filterOut = httpContext.Request.Query["filterout"].ToString();

                if (!string.IsNullOrEmpty(filterOut))
                {
                    var filterOutArr = filterOut.Split(",");
                    if (filterOutArr.Any())
                        if (filterOutArr.Any(itemFilter =>
                            desc.ActionDescriptor.DisplayName.Contains(itemFilter, StringComparison.OrdinalIgnoreCase)))
                            return false;
                }

                if (!string.IsNullOrEmpty(filter))
                {
                    var filterListArr = filter.Split(",");
                    if (filterListArr.Any())
                        return filterListArr.Any(itemFilter =>
                            desc.ActionDescriptor.DisplayName.Contains(itemFilter, StringComparison.OrdinalIgnoreCase));
                }
            }

            return isMatchVersion;
        }
    }
}