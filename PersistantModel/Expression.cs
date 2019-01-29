using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Interfaces;

namespace PersistantModel
{
    internal class Compute
    {
        #region Constants
        private const char StackIdentifierParentheses = '$';
        private const char StackIdentifierBrackets = '£';
        private readonly string[] errors = new string[] { null, "Erreur parenthèses", "Expression {0:G} incorrecte", "Terme inconnu" };
        private readonly char[] opers = new char[] { '=', ',', '+', '-', '/', '*', '^', 'v' };
        #endregion

        #region Private Fields
        int index;
        List<uint> stock = new List<uint>();
        List<string> words = new List<string>();
        Stack<string> stack = new Stack<string>();
        List<string> subexpressionsParentheses = new List<string>();
        List<string> subexpressionsBrackets = new List<string>();
        #endregion

        #region Private Methods

        private bool isNotLower(string s)
        {

            foreach (char c in s)
            {
                if (Char.IsLetter(c) && !Char.IsLower(c))
                    return true;
            }
            return false;
        }

        private void Enqueue(string s)
        {
            this.stack.Push(s);
        }

        private string Dequeue()
        {
            return this.stack.Pop();
        }

        private bool function(string expr, ref int i)
        {
            bool result = false;
            expr = expr.Trim();
            if (expr.EndsWith(Compute.StackIdentifierParentheses.ToString()))
            {
                this.words.Add(expr.Substring(0, expr.Length - 1));
                this.stock.Add('µ');
                this.stock.Add((uint)(this.words.Count - 1));
                result = true;
            }
            else if (expr.EndsWith(Compute.StackIdentifierBrackets.ToString()))
            {
                this.words.Add(expr.Substring(0, expr.Length - 1));
                this.stock.Add('µ');
                this.stock.Add((uint)(this.words.Count - 1));
                result = true;
            }
            i = expr.Length;
            return result;
        }

        private bool variable(string expr, ref int i)
        {
            string trimedExpr = expr.Trim();
            if (String.IsNullOrEmpty(trimedExpr))
                throw new Exception(String.Format(this.errors[2], expr));
            i = trimedExpr.Length;
            this.words.Add(trimedExpr);
            this.stock.Add('ù');
            this.stock.Add((uint)(this.words.Count - 1));
            return true;
        }

        private void noQuoteString(string expr, ref int i)
        {
            i = expr.Length;
            this.words.Add(expr);
            if (expr.StartsWith(Compute.StackIdentifierParentheses.ToString()))
            {
                this.stock.Add('a');
            }
            else
            {
                this.stock.Add('!');
            }
            this.stock.Add((uint)(this.words.Count - 1));
        }

        private void subExpressionParenthese(string expr, ref int i)
        {
            i = expr.Length;
            this.words.Add(expr);
            this.stock.Add('p');
            this.stock.Add((uint)(this.words.Count - 1));
        }

        private void subExpressionBrackets(string expr, ref int i)
        {
            i = expr.Length;
            this.words.Add(expr);
            this.stock.Add('g');
            this.stock.Add((uint)(this.words.Count - 1));
        }

        private void constante(string expr, ref int i)
        {
            int right;
            right = 0;
            string trimedExpr = expr.TrimStart();
            while (right < trimedExpr.Length && trimedExpr[right] >= '0' && trimedExpr[right] <= '9') ++right;
            i = right;
            string subexpr = trimedExpr.Substring(0, right);
            this.stock.Add('@');
            this.stock.Add(System.Convert.ToUInt32(subexpr));
        }

        private void monome(string expr)
        {
            int i, l;
            string trimedExpr = expr.TrimStart();
            l = trimedExpr.Length;
            if (l == 0) throw new Exception(String.Format(this.errors[2], expr));
            switch (trimedExpr[0])
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    i = 0;
                    this.constante(trimedExpr, ref i);
                    if (i == l) return;
                    break;
                case Compute.StackIdentifierParentheses:
                    {
                        i = 0;
                        trimedExpr = trimedExpr.TrimEnd();
                        l = trimedExpr.Length;
                        if (l == 1)
                        {
                            string subexpr = this.Dequeue();
                            this.subExpressionParenthese(subexpr, ref l);
                        }
                        else
                        {
                            // c'est un paramètre formel dans l'expression (commence par $)
                            this.noQuoteString(trimedExpr, ref i);
                        }
                        break;
                    }
                case Compute.StackIdentifierBrackets:
                    {
                        i = 0;
                        trimedExpr = trimedExpr.TrimEnd();
                        l = trimedExpr.Length;
                        if (l == 1)
                        {
                            string subexpr = this.Dequeue();
                            this.subExpressionBrackets(subexpr, ref l);
                        }
                        else
                        {
                            // c'est un paramètre formel dans l'expression (commence par $)
                            this.noQuoteString(trimedExpr, ref i);
                        }
                        break;
                    }
                default:
                    i = 0;
                    if (this.function(trimedExpr, ref i))
                    {
                        string subexpr = this.Dequeue();
                        this.parentheses(subexpr);
                    }
                    else
                    {
                        i = 0;
                        if (this.variable(trimedExpr, ref i))
                        {
                            if (i == l) return;
                        }
                    }
                    break;
            }
        }

        private void operateurs(string expr, int oper)
        {
            int i, j;
            string subexpr;

            j = i = expr.Length;
            if (i == 0)
                if (oper == 0)
                {
                    this.stock.Add('@');
                    this.stock.Add(0);
                    return;
                }
                else
                    throw new Exception(String.Format(this.errors[2], expr));
            if (expr[--i] != this.opers[oper])
            {
                while (--i >= 0)
                {
                    if (expr[i] == this.opers[oper]) break;
                }
            }
            if (i == -1)
            {
                if (oper + 1 < this.opers.Length)
                    this.operateurs(expr, oper + 1);
                else
                    this.monome(expr);
                return;
            }
            if (i == 0)
            {
                if (this.opers[oper] != '+' && this.opers[oper] != '-')
                    throw new Exception(String.Format(this.errors[2], expr));
                this.stock.Add(this.opers[oper]);
                monome("0");
                subexpr = expr.Substring(i + 1, j - i - 1);
                operateurs(subexpr, oper);
                return;
            }

            this.stock.Add(this.opers[oper]);
            subexpr = expr.Substring(0, i);
            operateurs(subexpr, oper);
            subexpr = expr.Substring(i + 1, j - i - 1);
            operateurs(subexpr, oper);

        }

        private void parentheses(string expr)
        {
            int i;
            int leftpar = -1, rightpar = -1;
            int profpar = 0;
            string subexpr;

            i = expr.Length;
            while (--i >= 0)
            {
                if (expr[i] == ')')
                    if (rightpar == -1) rightpar = i; else ++profpar;
                else if (expr[i] == '(')
                    if (profpar == 0) leftpar = i; else --profpar;

            }

            if (leftpar < 0 && rightpar < 0)
            {
                // il n'y a pas de parentheses
                // mais il peut y avoir des crochets
                {
                    i = expr.Length;
                    while (--i >= 0)
                    {
                        if (expr[i] == ']')
                            if (rightpar == -1) rightpar = i; else ++profpar;
                        else if (expr[i] == '[')
                            if (profpar == 0) leftpar = i; else --profpar;
                    }
                    if (leftpar < 0 && rightpar < 0)
                    {
                        // il n'y a pas de crochets
                        this.operateurs(expr, 0);
                        return;
                    }
                    if (leftpar < 0 || rightpar < leftpar)
                    {
                        throw new Exception(this.errors[1]);
                    }

                    subexpr = expr.Substring(leftpar + 1, rightpar - leftpar - 1);
                    this.Enqueue(subexpr);
                    string subexpr2 = expr.Substring(0, leftpar) + Compute.StackIdentifierBrackets.ToString() + expr.Substring(rightpar + 1);
                    this.Enqueue(subexpr2);
                    parentheses(this.Dequeue());
                    return;
                }

            }

            if (leftpar < 0 || rightpar < leftpar)
            {
                throw new Exception(this.errors[1]);
            }

            subexpr = expr.Substring(leftpar + 1, rightpar - leftpar - 1);
            this.Enqueue(subexpr);
            string subexpr3 = expr.Substring(0, leftpar) + Compute.StackIdentifierParentheses.ToString() + expr.Substring(rightpar + 1);
            this.Enqueue(subexpr3);
            parentheses(this.Dequeue());
        }

        private Arithmetic calculate()
        {
            Arithmetic a = null, b = null;
            Arithmetic res = new NumericValue(0.0d);
            switch (this.stock[this.index])
            {
                case '=':
                    ++this.index;
                    a = this.calculate();
                    b = this.calculate();
                    res = new Equal(a, b);
                    break;
                case '+':
                    ++this.index;
                    a = this.calculate();
                    b = this.calculate();
                    res = new Addition(a, b);
                    break;
                case '-':
                    ++this.index;
                    a = this.calculate();
                    b = this.calculate();
                    res = new Soustraction(a, b);
                    break;
                case '*':
                    ++this.index;
                    a = this.calculate();
                    b = this.calculate();
                    res = new Multiplication(a, b);
                    break;
                case '/':
                    ++this.index;
                    a = this.calculate();
                    b = this.calculate();
                    res = new Division(a, b);
                    break;
                case '^':
                    ++this.index;
                    a = this.calculate();
                    b = this.calculate();
                    res = new Power(a, b);
                    break;
                case 'v':
                    ++this.index;
                    a = this.calculate();
                    b = this.calculate();
                    res = new Root(a, b);
                    break;
                case 'p':
                    ++this.index;
                    string funp = this.words[(int)this.stock[this.index]];
                    res = new Parenthese(Expression.Convert(funp));
                    ++this.index;
                    break;
                case 'g':
                    ++this.index;
                    string fung = this.words[(int)this.stock[this.index]];
                    res = new Crochet(Expression.Convert(fung));
                    ++this.index;
                    break;
                case '@':
                    ++this.index;
                    res = new NumericValue((double)this.stock[this.index]);
                    ++this.index;
                    break;
                case 'ù':
                    ++this.index;
                    if (this.isNotLower(this.words[(int)this.stock[this.index]]))
                        res = new UnknownTerm(this.words[(int)this.stock[this.index]]);
                    else
                        res = new Coefficient(this.words[(int)this.stock[this.index]]);
                    ++this.index;
                    break;
                case 'µ':
                    ++this.index;
                    string fun = this.words[(int)this.stock[this.index]];
                    ++this.index;
                    res = new Function(fun, this.calculate());
                    break;
                case ',':
                    string paramLeft = string.Empty, paramRight = string.Empty;
                    if (this.stock[index] == ',')
                    {
                        ++this.index;
                        a = this.calculate();
                    }
                    else
                    {
                        b = this.calculate();
                    }
                    if (this.stock[index] == ',')
                    {
                        ++this.index;
                        b = this.calculate();
                    }
                    else
                    {
                        b = calculate();
                    }
                    List<IArithmetic> list = new List<IArithmetic>();
                    if (a is Sequence)
                    {
                        list.AddRange((a as Sequence).Items);
                    }
                    else
                    {
                        list.Add(a);
                    }
                    if (b is Sequence)
                    {
                        list.AddRange((b as Sequence).Items);
                    }
                    else
                    {
                        list.Add(b);
                    }
                    res = new Sequence(list);
                    break;
            }
            return res;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts a string to an arithmetic form
        /// </summary>
        /// <param name="expr">string representation</param>
        /// <returns>arithmetic object</returns>
        public Arithmetic run(string expr)
        {
            this.parentheses(expr);
            this.index = 0;
            try
            {
                return this.calculate();
            }
            catch (Exception ex)
            {
                throw new Exception("L'expression '" + expr + "' n'a pas fonctionnée pour la raison suivante :" + Environment.NewLine + ex.ToString());
            }
        }
        #endregion
    }

    /// <summary>
    /// Static class for convert string
    /// to Arithmetic object
    /// </summary>
    public static class Expression
    {
        /// <summary>
        /// Converts a string to an arithmetic object
        /// </summary>
        /// <param name="expr">expression string</param>
        /// <returns>arithmetic object</returns>
        public static Arithmetic Convert(string expr)
        {
            Compute c = new Compute();
            if (!String.IsNullOrEmpty(expr))
            {
                return c.run(expr);
            }
            else
            {
                return new NumericValue(0.0);
            }
        }
    }
}
