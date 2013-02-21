using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Noodles.Helpers
{
    public static class FormattableObject
    {
        public static string ToString(this object anObject, string aFormat)
        {
            return ToString(anObject, aFormat, null);
        }

        public static string ToString(this object anObject, string aFormat, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            Type targetType = anObject.GetType();
            Regex reg = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase);
            MatchCollection mc = reg.Matches(aFormat);
            int startIndex = 0;
            foreach (Match m in mc)
            {
                Group g = m.Groups[2]; //it's second in the match between { and }
                int length = g.Index - startIndex - 1;
                sb.Append(aFormat.Substring(startIndex, length));

                string toGet = String.Empty;
                string toFormat = String.Empty;
                int formatIndex = g.Value.IndexOf(":"); //formatting would be to the right of a :
                if (formatIndex == -1) //no formatting, no worries
                {
                    toGet = g.Value;
                }
                else //pickup the formatting
                {
                    toGet = g.Value.Substring(0, formatIndex);
                    toFormat = g.Value.Substring(formatIndex + 1);
                }

                Type retrievedType = null;

                var parts = toGet.Split('.');
                var type = null as Type;
                var targetObject = anObject;
                foreach (var part in parts)
                {
                    type = targetObject.GetType();
                    //first try properties
                    PropertyInfo retrievedProperty = type.GetProperty(part);
                    if (retrievedProperty != null)
                    {
                        retrievedType = retrievedProperty.PropertyType;
                        targetObject = retrievedProperty.GetValue(targetObject, null);
                    }
                    else //try fields
                    {
                        FieldInfo retrievedField = type.GetField(part);
                        if (retrievedField != null)
                        {
                            retrievedType = retrievedField.FieldType;
                            targetObject = retrievedField.GetValue(anObject);
                        }
                    }
                }
                
                if (retrievedType != null) //Cool, we found something
                {
                    string result = String.Empty;
                    if (toFormat == String.Empty) //no format info
                    {
                        result = retrievedType.InvokeMember("ToString",
                                                            BindingFlags.Public | BindingFlags.NonPublic |
                                                            BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                                                            , null, targetObject ?? "", null) as string;
                    }
                    else //format info
                    {
                        result = retrievedType.InvokeMember("ToString",
                                                            BindingFlags.Public | BindingFlags.NonPublic |
                                                            BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                                                            , null, targetObject ?? "", new object[] { toFormat, formatProvider }) as string;
                    }
                    sb.Append(result);
                }
                else //didn't find a property with that name, so be gracious and put it back
                {
                    sb.Append("{");
                    sb.Append(g.Value);
                    sb.Append("}");
                }
                startIndex = g.Index + g.Length + 1;
            }
            if (startIndex < aFormat.Length) //include the rest (end) of the string
            {
                sb.Append(aFormat.Substring(startIndex));
            }
            return sb.ToString();
        }
    }
}