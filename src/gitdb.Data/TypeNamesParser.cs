using System;
using System.Collections.Generic;
using System.Text;

namespace gitdb.Data
{
    public class TypeNamesParser
    {
        public char DefinitionSeparator = '|';

        public char PairSeparator = '-';

        public TypeNamesParser ()
        {
        }

        public string[] ParseTypeNames(string typesString)
        {
            var typeNames = new List<string> ();

            // TODO: Should this be rewritten using linq?
            if (!String.IsNullOrEmpty (typesString)) {
                var typeDefinitionStrings = typesString.Split (DefinitionSeparator);
                foreach (var typeDefinitionString in typeDefinitionStrings) {
                    if (!String.IsNullOrEmpty (typeDefinitionString)
                        && typeDefinitionString.Contains(PairSeparator.ToString())) {
                        var parts = typeDefinitionString.Split (PairSeparator);
                        var typeName = parts [0];
                        typeNames.Add (typeName);
                    }
                }
            }

            return typeNames.ToArray ();
        }

        public Dictionary<string, string> ParseTypeDefinitions(string typesString)
        {
            var typeDefinitions = new Dictionary<string, string> ();

            if (!String.IsNullOrEmpty (typesString)) {
                var typeDefinitionStrings = typesString.Split (DefinitionSeparator);
                foreach (var typeDefinitionString in typeDefinitionStrings) {
                    if (!String.IsNullOrEmpty (typeDefinitionString)
                        && typeDefinitionString.Contains(PairSeparator.ToString())) {
                        var parts = typeDefinitionString.Split (PairSeparator);
                        var typeName = parts [0];
                        var typeFullName = parts [1];
                        typeDefinitions.Add (typeName, typeFullName);
                    }
                }
            }

            return typeDefinitions;
        }

        public string CompileTypeDefinitions(Dictionary<string, string> typeDefinitions)
        {
            var builder = new StringBuilder ();

            foreach (var typeName in typeDefinitions.Keys) {
                builder.Append (typeName + PairSeparator + typeDefinitions [typeName] + DefinitionSeparator);
            }

            var typesString = builder.ToString ().TrimEnd (DefinitionSeparator);

            return typesString;
        }
    }
}

