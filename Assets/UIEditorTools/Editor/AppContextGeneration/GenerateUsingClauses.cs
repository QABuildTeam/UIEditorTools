using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;

namespace UIEditorTools.Editor
{
    public partial class AppContextGenerationUtility : EditorWindow
    {
        public class GenerateUsingClauses
        {
            private HashSet<string> usingClauses = new HashSet<string>();
            public GenerateUsingClauses() { }
            public GenerateUsingClauses(IEnumerable<string> clauses)
            {
                foreach (var clause in clauses)
                {
                    Add(clause);
                }
            }

            public void Add(string clause)
            {
                usingClauses.Add(clause);
            }
            private static string usingTemplate = @"using {0};
";

            public string GetUsingClauses()
            {
                string result = string.Empty;
                foreach (var usingClause in usingClauses)
                {
                    result += string.Format(usingTemplate, usingClause);
                }
                return result;
            }
        }
    }
}
