using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PersistantModel
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Extension to extract a tex content from a string
        /// </summary>
        /// <param name="s">owner</param>
        /// <param name="delimOn">delimiter on</param>
        /// <param name="delimOff">deliiter off</param>
        /// <param name="startingPoint">starting indice</param>
        /// <returns>tex code</returns>
        public static string ExtractTexCode(this string s, char delimOn, char delimOff, ref int startingPoint)
        {
            string output = string.Empty;
            bool recording = false;
            int state = 0;
            int index = startingPoint;
            int depth = 0;

            while (index < s.Length)
            {
                char c = s[index];
                if (state == 0)
                {
                    if (c == delimOn) {
                        state = 1;
                        ++depth;
                        recording = true;
                    }
                }
                else if (state == 1)
                {
                    if (c == delimOff)
                    {
                        --depth;
                        if (depth == 0)
                        {
                            state = 2;
                        }
                    }
                    else if (c == delimOn)
                    {
                        ++depth;
                    }
                }
                else
                {
                    recording = false;
                    break;
                }
                if (recording)
                    output += c;
                ++index;
            }
            startingPoint = index;
            return output;
        }

        /// <summary>
        /// Split each part between each tex part of a string
        /// </summary>
        /// <param name="s">owner</param>
        /// <param name="delimOn">delim on</param>
        /// <param name="delimOff">delim off</param>
        /// <returns>string list</returns>
        public static string[] SplitForTex(this string s, char delimOn, char delimOff)
        {
            int index = 0;
            List<string> list = new List<string>();
            while (index < s.Length)
            {
                if (s[index] == delimOn)
                    list.Add(s.ExtractTexCode(delimOn, delimOff, ref index));
                else
                {
                    int start = s.IndexOf(delimOn, index);
                    if (start == -1)
                    {
                        list.Add(s.Substring(index));
                        break;
                    }
                    else
                    {
                        list.Add(s.Substring(index, start - index));
                        index = start;
                    }
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Insert text elements into a list
        /// </summary>
        /// <param name="s">owner</param>
        /// <param name="delimOn">delimiter on</param>
        /// <param name="delimOff">deliiter off</param>
        /// <param name="p">paragraph</param>
        public static void InsertIntoDocument(this string s, char delimOn, char delimOff, Paragraph p)
        {
            foreach (string c in s.SplitForTex(delimOn, delimOff))
            {
                if (c.StartsWith(delimOn.ToString()) && c.EndsWith(delimOff.ToString()))
                {
                    string tex = c.Substring(1, c.Length - 2);
                    WpfMath.Controls.FormulaControl fc = new WpfMath.Controls.FormulaControl();
                    fc.Formula = tex;
                    p.Inlines.Add(new InlineUIContainer(fc));
                }
                else
                {
                    p.Inlines.Add(new Run(c));
                }
            }
        }

        /// <summary>
        /// Convert an IArithmetic object to a double
        /// </summary>
        /// <param name="a">from</param>
        /// <returns>double value</returns>
        public static double ToDouble(this Interfaces.IArithmetic a)
        {
            Interfaces.IArithmetic c = a.Compute();
            if (c is NumericValue)
                return (c as NumericValue).Value;
            else
                return 0.0d;
        }

        /// <summary>
        /// Convert a double precision number to an arithmetic object
        /// </summary>
        /// <param name="d">double value</param>
        /// <returns>arithmetic object</returns>
        public static Interfaces.IArithmetic ToArithmetic(this double d)
        {
            return new NumericValue(d);
        }

        /// <summary>
        /// Test if a is a double value
        /// </summary>
        /// <param name="a">arithmetic class</param>
        /// <returns>true if it is a double</returns>
        public static bool IsDouble(this Interfaces.IArithmetic a)
        {
            return a is NumericValue;
        }

    }
}
