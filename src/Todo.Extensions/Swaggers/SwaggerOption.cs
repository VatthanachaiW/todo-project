using System;
using System.Collections.Generic;

namespace Todo.Extensions.Swaggers
{
    /// <summary>
    /// Configuration หลักของ Swagger
    /// </summary>
    [Serializable]
    public class SwaggerOption
    {
        public bool Enabled { get; set; }
        public bool ReDocEnabled { get; set; }
        public string RoutePrefix { get; set; }
        public bool IncludeSecurity { get; set; }
        public List<SwaggerVersion> Versions { get; set; }
    }

    /// <summary>
    /// Configuration ของ API แต่ละ Version
    /// </summary>
    [Serializable]
    public class SwaggerVersion
    {
        public string Title { get; set; }
        public string Version { get; set; }
    }
}