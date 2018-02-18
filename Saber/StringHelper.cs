using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Saber.TestTask
{
    public static class StringHelper
    {
        public static string ToLiteral(this string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    var result = writer.ToString();
                    return result.TrimStart('\"').TrimEnd('\"');
                }
            }
        }

        public static string FromLiteral(this string input)
        {
            return Regex.Unescape(input);
        }
    }
}