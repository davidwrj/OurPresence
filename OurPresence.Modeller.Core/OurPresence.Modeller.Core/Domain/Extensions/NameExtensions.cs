using System.Collections.Generic;
using System.Linq;

namespace OurPresence.Modeller.Domain.Extensions
{
    public static class NameExtensions
    {
        public static string TrimEnd(this Name name, string value)
        {
            if (string.IsNullOrEmpty(value))
                return name.ToString();

            var fieldName = name.ToString();
            if (fieldName.EndsWith(value))
                fieldName = fieldName.Substring(0, fieldName.Length - value.Length);
            return fieldName;
        }

        public static readonly IEnumerable<string> ReservedWords = new[] { "abstract","as","base","bool","break","byte","case","catch","char","checked","class","const","continue","decimal","default","delegate",
            "do","double","else","enum","event","explicit","extern","false","finally","fixed","float","for","foreach","goto","if","implicit","in","int","interface","internal","is","lock","long","namespace",
            "new","null","object","operator","out","override","params","private","protected","public","readonly","ref","return","sbyte","sealed","short","sizeof","stackalloc","static","string",
            "struct","switch","this","throw","true","try","typeof","uint","ulong","unchecked","unsafe","ushort","using","virtual","void","volatile","while" };

        public static Name CheckKeyword(this Name name)
        {
            var result = name.ToString();
            var alternate = result.CheckKeyword();
            return new Name(alternate);
        }

        public static string CheckKeyword(this string name) => string.IsNullOrWhiteSpace(name) ? string.Empty : ReservedWords.Contains(name.ToLowerInvariant()) ? "@" + name : name;
    }
}
