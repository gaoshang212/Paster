﻿using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace PasterVsix.Extensions
{
    public static class StringExtension
    {
        public static string ToLiteral(this string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }
    }
}