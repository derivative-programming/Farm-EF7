using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FS.Common.Strings
{
    public class Functions
    {

        public static string ToPhoneNumber(string value)
        {
            value = new Regex(@"\D").Replace(value, string.Empty);

            if (value.Length == 0)
                value = string.Empty;
            else if (value.Length <= 3)
                value = string.Format("{0}", value.Substring(0, value.Length));
            else if (value.Length <= 7)
                value = string.Format("{0}-{1}", value.Substring(0, 3), value.Substring(3, value.Length - 3));
            else if (value.Length <= 8)
                value = string.Format("({0}) {1}-{2}", value.Substring(0, 1), value.Substring(1, 3), value.Substring(4));
            else if (value.Length <= 9)
                value = string.Format("({0}) {1}-{2}", value.Substring(0, 2), value.Substring(2, 3), value.Substring(5));
            else if (value.Length <= 10)
                value = string.Format("({0}) {1}-{2}", value.Substring(0, 3), value.Substring(3, 3), value.Substring(6));
            else if (value.Length > 11)
                value = string.Format("{0} - ({1}) {2}-{3}", value.Substring(0, 1), value.Substring(1, 3), value.Substring(4, 3), value.Substring(7));
            return value;
        }
        public static string RemoveFromStart(string data,int count)
        {
            return data.Substring(count, data.Length - count);
        }

        public static string RemoveFromEnd(string data, int count)
        {
            return data.Substring(0, data.Length - count);
        }

        public static Guid GuidParse(object data)
        {
            return Guid.Parse(data.ToString());
        } 
        public static string UpperFirstLetter(string data)
        {
            if (data.Length == 0)
                return data;
            return System.Convert.ToChar(data[0].ToString().ToUpper()) + data.Remove(0, 1);
        }
        public static string LowerFirstLetter(string data)
        {
            if (data.Length == 0)
                return data;
            return System.Convert.ToChar(data[0].ToString().ToLower()) + data.Remove(0, 1);
        }
        public static string GetSpacedFromCamelCaseV1(string data)
        {
            if (data.Length == 0)
                return data; 
            int n = 0;
            string spacedName = string.Empty;
            for (int j = 0; j < data.ToCharArray().Length; j++)
            {
                spacedName += data[j].ToString();
                if (j == 0)
                    continue;
                if ((j + 1) < data.ToCharArray().Length)
                {
                    if (data[j].ToString().ToLower() == data[j].ToString() &&
                       data[j + 1].ToString().ToUpper() == data[j + 1].ToString())
                    {
                        spacedName += " ";
                    }
                    else if ((j + 2) < data.ToCharArray().Length &&
                        data[j].ToString().ToUpper() == data[j].ToString() &&
                       data[j + 1].ToString().ToUpper() == data[j + 1].ToString() &&
                       data[j + 2].ToString().ToLower() == data[j + 2].ToString() &&
                       !int.TryParse(data[j + 2].ToString(),out n))
                    {
                        spacedName += " ";
                    }
                }
            }
            return spacedName;

        }

        public static string GetSpacedFromCamelCase(string data)
        {
            if (data.Length == 0)
                return data;
            var result = "";
            for (int i = 0; (i < data.Length); i++)
            {
                if (i != 0)
                {
                    if (char.IsDigit(data.ToCharArray()[i]) && !char.IsDigit(data.ToCharArray()[i - 1]))
                    {
                        result += " ";
                    }
                    if (char.IsUpper(data.ToCharArray()[i]) && !char.IsUpper(data.ToCharArray()[i - 1]))
                    {
                        result += " ";
                    }
                    else if (char.IsUpper(data.ToCharArray()[i]) && char.IsUpper(data.ToCharArray()[i - 1]) && i != data.Length - 1 && !char.IsUpper(data.ToCharArray()[i + 1]))
                    {
                        result += " ";
                    }
                    result += data.ToCharArray()[i];
                }
                else
                {
                    result += data.ToCharArray()[i];
                }
            }
            return result;

        }
        public static string GetCamelCaseFromUnderscoredLowerCase(string data)
        {
            if (data.Length == 0)
                return data;
            string result = string.Empty;
            for (int j = 0; j < data.ToCharArray().Length; j++)
            {
                if (j == 0)
                {
                    result += data[j].ToString().ToUpper(); 
                    continue;
                }
                else
                {
                    if (data[j] == '_')
                        continue;
                    if (data[j - 1] == '_')
                        result += data[j].ToString().ToUpper();
                    else
                    {
                        result += data[j].ToString();
                    }
                }
            }
            return result;

        }
        public static string ReplaceSpecialCharacters(string rootString)
        {
            string tmp = rootString;
            //' TODO keep an eye on these chars then refactor when satisfied
            rootString = rootString.Replace(".", "");
            rootString = rootString.Replace(",", "");
            rootString = rootString.Replace("/", " ");
            rootString = rootString.Replace("\"", "");
            rootString = rootString.Replace("(", "");
            rootString = rootString.Replace(")", "");
            rootString = rootString.Replace("[", "");
            rootString = rootString.Replace("]", "");
            rootString = rootString.Replace("'", "");
            //'rootString = rootString.Replace("'", " ");
            rootString = rootString.Replace("-", " ");
            rootString = rootString.Replace("–", " ");
            rootString = rootString.Replace("`", " ");
            rootString = rootString.Replace("‘", "");
            rootString = rootString.Replace("@", " ");
            rootString = rootString.Replace("*", " ");
            rootString = rootString.Replace("%", " ");
            //'rootString = rootString.Replace("‘", " ");
            rootString = rootString.Replace(@"\", "");
            rootString = rootString.Replace("^", "");
            rootString = rootString.Replace("_", "");
            rootString = rootString.Replace("{", "");
            rootString = rootString.Replace("|", "");
            rootString = rootString.Replace("}", "");
            rootString = rootString.Replace("~", "");
            rootString = rootString.Replace(" & ", " and ");
            rootString = rootString.Replace(" &amp; ", " and ");
            rootString = rootString.Replace("&", "");
            rootString = rootString.Replace("&amp;", "");
            rootString = rootString.Replace(( char)160, ' ');

            //' remove duplicate spaces - up to 9 
            rootString = FS.Common.Strings.Functions.RemoveDoubleSpaces(rootString);
            //while (rootString.IndexOf("  ") > -1)
            //    rootString = rootString.Replace((char)65533, ' ').Replace((char)160, ' ').Replace("  ", " ");

            return rootString.Trim();

        }
        public static string RemoveDoubleSpaces(string data)
        {
            while (data.IndexOf("  ") > -1 &&
                    data != data.Replace((char)65533, ' ').Replace((char)160, ' ').Replace("  ", " "))
                data = data.Replace((char)65533, ' ').Replace((char)160, ' ').Replace("  ", " ");
            return data;
        }

        public static string NormalizeSymbols(string data)
        {
            return data.Replace("™", "&trade;").Replace("®", "&reg;").Replace("©", "&copy;").Replace("℠", "&#8480;");
        }

        public static string DeNormalizeSymbols(string data)
        {
            return data.Replace("&trade;", "™").Replace("&reg;", "®").Replace("&copy;", "©").Replace("&#8480;", "℠").Replace("&quot;","`");
        }

        public static string HtmlEncode(string data)
        {
            return System.Web.HttpUtility.HtmlEncode(data);
        }

        public static string HtmlDecode(string data)
        {
            return System.Web.HttpUtility.HtmlDecode(data);
        }
        public static string URLEncode(string data)
        {
            return System.Web.HttpUtility.UrlEncode(data);
        }

        public static string URLDecode(string data)
        {
            return System.Web.HttpUtility.UrlDecode(data);
        }

        public static bool FindSubString(string val, string surroundedByCharLeftSide, string surroundedByCharRightSide, out string subString, out string newval)
        {
            bool result = false;
            subString = string.Empty;
            newval = string.Empty;
            if (val.Contains(surroundedByCharLeftSide) &&
                val.Contains(surroundedByCharRightSide))
            {
                int leftIndex = val.IndexOf(surroundedByCharLeftSide);
                int rightIndex = val.IndexOf(surroundedByCharRightSide);
                for(int i = rightIndex-1;i >= 0;i--)
                {
                    if (val.IndexOf(surroundedByCharLeftSide, i) > -1 &&
                        val.IndexOf(surroundedByCharLeftSide, i) < rightIndex)
                    {
                        leftIndex = val.IndexOf(surroundedByCharLeftSide,i);
                        break;
                    }
                }
                subString = val.Substring(leftIndex + 1, rightIndex - leftIndex - 1);
                newval = val.Remove(leftIndex, rightIndex - leftIndex + 1);
                result = true;
            }
            return result;
        }

        public static bool FindSubString_v2(string val, string surroundedByCharLeftSide, string surroundedByCharRightSide, out string subString, out string newval)
        {
            bool result = false;
            subString = string.Empty;
            newval = val;
            try
            {
                if (val.Contains(surroundedByCharLeftSide) &&
                    val.Contains(surroundedByCharRightSide))
                {
                    int leftIndex = val.IndexOf(surroundedByCharLeftSide);
                    int rightIndex = val.IndexOf(surroundedByCharRightSide);
                    if (leftIndex > rightIndex)
                    {
                        return false;
                    }
                    int end = (rightIndex - 1);
                    int len = val.Length;
                    int start = -1;
                    while (val.IndexOf(surroundedByCharLeftSide, start + 1) < rightIndex && val.IndexOf(surroundedByCharLeftSide, start + 1) > -1)
                    {
                        start = val.IndexOf(surroundedByCharLeftSide, start + 1);
                    }

                    for (int i = start; i >= 0; i--)
                    { 
                        if (val.IndexOf(surroundedByCharLeftSide, i) > -1 &&
                            val.IndexOf(surroundedByCharLeftSide, i) < (rightIndex - surroundedByCharLeftSide.Length - 1))
                        {
                            leftIndex = val.IndexOf(surroundedByCharLeftSide, i);
                            break;
                        }

                        int start2 = -1;
                        while (val.IndexOf(surroundedByCharLeftSide, start2 + 1) < i && val.IndexOf(surroundedByCharLeftSide, start2 + 1) > -1)
                        {
                            start2 = val.IndexOf(surroundedByCharLeftSide, start2 + 1);
                        }
                        if (start2 == -1)
                            break;
                        else
                            i = start2 + 1;
                    } 
                    subString = val.Substring(leftIndex + surroundedByCharLeftSide.Length, rightIndex - leftIndex - surroundedByCharLeftSide.Length);
                    newval = val.Remove(leftIndex, (subString.Length + surroundedByCharLeftSide.Length + surroundedByCharRightSide.Length));
                    result = true;
                     
                }
            }
            catch (System.Exception)
            {

            }
            return result;
        }
        public static string RemoveDiacritics(string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string StripHTTP(string url)
        {
            string result = url;
            if(url.ToLower().StartsWith("http://"))
                result = url.Remove(0, 7).TrimEnd("/".ToCharArray());
            if (url.ToLower().StartsWith("https://"))
                result = url.Remove(0, 8).TrimEnd("/".ToCharArray());
            return result;
        }

        public static List<string> GetCSVIntersect(string csv1, string csv2)
        { 
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();

            list1.AddRange(csv1.Split(",".ToCharArray()));
            list2.AddRange(csv2.Split(",".ToCharArray()));
             
            return Lists.Functions.GetIntersection(list1, list2);
        }

        public static bool CSVIntersectExist(string csv1, string csv2)
        {
            bool result = false; 
            if (GetCSVIntersect(csv1, csv2).Count > 0)
                result = true;
            return result;
        }

        public static string RemoveEmptyLines(string data)
        {
            int count = 0;
            string testdata = data.TrimStart(" \t".ToCharArray());
            while(testdata.StartsWith("\r\n"))
            {
                testdata = testdata.TrimStart("\r\n".ToCharArray());
                testdata = testdata.TrimStart(" ".ToCharArray());
                if(testdata.StartsWith("\r\n"))
                    count++;
            }
            if (count > 0)
            {
                data = data.TrimStart(" \t".ToCharArray());
                for (int i = 0; i < count; i++)
                {
                    data = data.TrimStart("\r\n".ToCharArray());
                    data = data.TrimStart(" \t".ToCharArray());
                }
            }
            return data;
        }

    }
}
