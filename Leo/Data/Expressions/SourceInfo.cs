using System;
using System.Collections.Generic;

namespace Leo.Data.Expressions
{
    public class SourceInfo
    {
        public Type SourceType { get; set; }

        public string SourceText { get; set; }

        public string Alias { get; set; }

        public bool IsQuery { get; set; }

        public Dictionary<string, object> Parameters { get; set; }
    }

}
