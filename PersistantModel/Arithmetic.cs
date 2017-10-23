using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace PersistantModel
{
    /// <summary>
    /// Classe permettant
    /// d'enregistrer des éléments
    /// arithmetiques
    /// </summary>
    [Serializable]
    public class Arithmetic : PersistentDataObject, IArithmetic, IEquation, ICloneable, IEqualityComparer<IArithmetic>
    {

        #region Fields

        protected static readonly string weightName = "weight";

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected Arithmetic()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique and readonly weight for this object
        /// </summary>
        public IWeight OwnerWeight
        {
            get
            {
                if (!this.Data.ContainsKey(weightName))
                    this[weightName] = this.ComputeOwnerWeight();
                return this[weightName];
            }
        }

        /// <summary>
        /// Gets or sets a value from
        /// persistent data
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>value</returns>
        public dynamic this[string name]
        {
            get
            {
                return this.Get(name);
            }
            set
            {
                this.Set(name, value);
            }
        }

        /// <summary>
        /// Gets the operator ID
        /// </summary>
        public virtual char Operator
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets binary switch test
        /// </summary>
        public virtual bool IsBinaryOperator
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets unary switch test
        /// </summary>
        public virtual bool IsUnaryOperator
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets multiple switch test
        /// </summary>
        public virtual bool IsMultipleOperator
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets true if it's not an operator
        /// </summary>
        public virtual bool IsNotOperator
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the inner operand
        /// </summary>
        public virtual IArithmetic InnerOperand
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the left operand
        /// </summary>
        public virtual IArithmetic LeftOperand
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the right operand
        /// </summary>
        public virtual IArithmetic RightOperand
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets all unknown terms
        /// </summary>
        public IEnumerable<IArithmetic> UnknownTerms
        {
            get
            {
                if (this.IsBinaryOperator)
                {
                    if (this.LeftOperand != null)
                        foreach (IArithmetic e in this.LeftOperand.UnknownTerms) yield return e;
                    if (this.RightOperand != null)
                        foreach (IArithmetic e in this.RightOperand.UnknownTerms) yield return e;
                }
                else if (this.IsUnaryOperator)
                {
                    if (this.InnerOperand != null)
                        foreach (IArithmetic e in this.InnerOperand.UnknownTerms) yield return e;
                }
                else
                {
                    if (this is Term)
                        foreach (IArithmetic e in (this as Term).Unknown.UnknownTerms) yield return e;
                    else if (this is UnknownTerm)
                        yield return this;
                }
            }
        }

        /// <summary>
        /// Gets all coefficients
        /// </summary>
        public IEnumerable<IArithmetic> Coefficients
        {
            get
            {
                if (this.IsBinaryOperator)
                {
                    if (this.LeftOperand != null)
                        foreach (IArithmetic e in this.LeftOperand.Coefficients) yield return e;
                    if (this.RightOperand != null)
                        foreach (IArithmetic e in this.RightOperand.Coefficients) yield return e;
                }
                else if (this.IsUnaryOperator)
                {
                    if (this.InnerOperand != null)
                        foreach (IArithmetic e in this.InnerOperand.Coefficients) yield return e;
                }
                else
                {
                    if (this is Term)
                        foreach (IArithmetic e in (this as Term).Coefficient.Coefficients) yield return e;
                    else if (this is Coefficient)
                        yield return this;
                }
            }
        }

        /// <summary>
        /// Gets all constants
        /// </summary>
        public IEnumerable<IArithmetic> Constants
        {
            get
            {
                if (this.IsBinaryOperator)
                {
                    if (this.LeftOperand != null)
                        foreach (IArithmetic e in this.LeftOperand.Constants) yield return e;
                    if (this.RightOperand != null)
                        foreach (IArithmetic e in this.RightOperand.Constants) yield return e;
                }
                else if (this.IsUnaryOperator)
                {
                    if (this.InnerOperand != null)
                        foreach (IArithmetic e in this.InnerOperand.Constants) yield return e;
                }
                else
                {
                    if (this is Term)
                        foreach (IArithmetic e in (this as Term).Constant.Constants) yield return e;
                    else if (this is NumericValue)
                        yield return this;
                }
            }
        }

        /// <summary>
        /// Gets equation
        /// </summary>
        public IArithmetic Equation
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the unique weight
        /// for this object
        /// </summary>
        /// <returns>weight</returns>
        protected virtual Weight ComputeOwnerWeight()
        {
            throw new NotSupportedException();
        } 

        /// <summary>
        /// Verify when a positive equation
        /// may contains multiple inner positive or negative
        /// equation
        /// </summary>
        /// <param name="e">equation to test</param>
        /// <returns>correct equation</returns>
        public static IArithmetic EnsureSign(IArithmetic e)
        {
            if (e is Negative || e is Positive)
            {
                return EnsureSign((e as UnaryOperation).InnerOperand);
            }
            else
            {
                return e;
            }
        }

        /// <summary>
        /// Verify when an inverse equation
        /// may contains multiple inner inverse
        /// equation
        /// </summary>
        /// <param name="e">equation to test</param>
        /// <returns>correct equation</returns>
        public static IArithmetic EnsureInverse(IArithmetic e)
        {
            if (e is Inverse)
            {
                return EnsureInverse((e as Inverse).InnerOperand);
            }
            else
            {
                return e;
            }
        }

        /// <summary>
        /// Verify when an equation
        /// may contains multiple inner add or sub equations
        /// </summary>
        /// <param name="e">equation to test</param>
        /// <param name="direction">sommation or difference</param>
        /// <returns>correct equation</returns>
        public static Sum EnsureSum(IArithmetic e, int direction)
        {
            if (e is Addition)
            {
                Sum s1 = EnsureSum((e as BinaryOperation).LeftOperand, direction);
                Sum s2 = EnsureSum((e as BinaryOperation).RightOperand, direction);
                return new Sum(s1.Items.Concat(s2.Items).ToArray());
            }
            else if (e is Soustraction)
            {
                Sum s1 = EnsureSum((e as BinaryOperation).LeftOperand, direction);
                Sum s2 = EnsureSum((e as BinaryOperation).RightOperand, -direction);
                return new Sum(s1.Items.Concat(s2.Items).ToArray());
            }
            else
            {
                if (direction > 0)
                {
                    IArithmetic eTemp = EnsureSign(e);
                    return new Sum(e);
                }
                else
                {
                    IArithmetic eTemp = EnsureSign(e);
                    return new Sum(new Negative(e));
                }
            }
        }

        /// <summary>
        /// Verify when an equation
        /// may contains multiple inner multiplication or division
        /// equation
        /// </summary>
        /// <param name="e">equation to test</param>
        /// <param name="direction">sommation or difference</param>
        /// <returns>correct equation</returns>
        public static Product EnsureProduct(IArithmetic e, int direction)
        {
            if (e is Multiplication)
            {
                Product s1 = EnsureProduct((e as BinaryOperation).LeftOperand, direction);
                Product s2 = EnsureProduct((e as BinaryOperation).RightOperand, direction);
                return new Product(s1.Items.Concat(s2.Items).ToArray());
            }
            else if (e is Division)
            {
                Product s1 = EnsureProduct((e as BinaryOperation).LeftOperand, direction);
                Product s2 = EnsureProduct((e as BinaryOperation).RightOperand, -direction);
                return new Product(s1.Items.Concat(s2.Items).ToArray());
            }
            else
            {
                if (direction > 0)
                {
                    return new Product(e);
                }
                else
                {
                    IArithmetic eTemp = EnsureInverse(e);
                    return new Product(new Inverse(e));
                }
            }
        }

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected virtual IArithmetic Create()
        {
            return new Arithmetic();
        }

        /// <summary>
        /// Transforms an addition into a multiplication
        /// </summary>
        /// <returns>a list of possibles equation</returns>
        protected virtual IEnumerable<IArithmetic> MakeTransform()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Clone this arithmetic object
        /// </summary>
        /// <returns>new arithmetic object</returns>
        public object Clone()
        {
            IArithmetic p = this.Create();
            foreach (string key in this.Data.Keys)
            {
                if (this.Data[key] is IArithmetic)
                    p[key] = this.Data[key].Clone();
                else if (this.Data[key] is string)
                    p[key] = this.Data[key].Clone();
                else
                    p[key] = this.Data[key];
            }
            return p;
        }

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        public virtual string ToTex()
        {
            string output = string.Empty;

            if (this.IsBinaryOperator)
            {
                string left = string.Empty, right = string.Empty;
                if (this.LeftOperand != null)
                    left = this.LeftOperand.ToTex();
                if (this.RightOperand != null)
                    right = this.RightOperand.ToTex();
                output = @"{\left[" + left + " " + this.Operator + " " + right + @"\right]}";
            }
            else if (this.IsUnaryOperator)
            {
                if (this.InnerOperand != null)
                    output = this.Operator + "{" + this.InnerOperand.ToTex() + "}";
            }
            else
            {
                if (this is Term)
                {
                    Term t = this as Term;
                    output = "{" + t.Constant.ToTex() + "} * {" + t.Coefficient.ToTex() + "} * {" + t.Unknown.ToTex() + "}";
                }
                else if (this is NumericValue)
                    output = (this as NumericValue).Value.ToString();
                else if (this is Coefficient)
                    output = (this as Coefficient).Name;
                else if (this is UnknownTerm)
                    output = (this as UnknownTerm).Name;
                else
                    throw new InvalidCastException();
            }
            return output;
        }

        /// <summary>
        /// Transforms equation object into a string representation
        /// </summary>
        /// <returns>string representation</returns>
        public override string ToString()
        {
            string output = string.Empty;

            if (this.IsBinaryOperator)
            {
                string left = string.Empty, right = string.Empty;
                if (this.LeftOperand != null)
                    left = this.LeftOperand.ToString();
                if (this.RightOperand != null)
                    right = this.RightOperand.ToString();
                output = "(" + left + " " + this.Operator + " " + right + ")";
            }
            else if (this.IsUnaryOperator)
            {
                if (this.InnerOperand != null)
                    output = this.Operator + "(" + this.InnerOperand.ToString() + ")";
            }
            else
            {
                if (this is Term)
                {
                    Term t = this as Term;
                    output = "(" + t.Constant.ToString() + ") * (" + t.Coefficient.ToString() + ") * (" + t.Unknown.ToString() + ")";
                }
                else if (this is NumericValue)
                    output = (this as NumericValue).Value.ToString();
                else if (this is Coefficient)
                    output = (this as Coefficient).Name;
                else if (this is UnknownTerm)
                    output = (this as UnknownTerm).Name;
                else
                    throw new InvalidCastException();
            }
            return output;
        }

        public void Let(string letter, double value)
        {
            throw new NotImplementedException();
        }

        public void Let(string letter, IEquation e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// String representation of the algebraic equation
        /// </summary>
        /// <param name="type">type of representation; string or tex</param>
        /// <returns>string text</returns>
        public string AsRepresented(string type)
        {
            if (type == "string")
            {
                return this.ToString();
            }
            else if (type == "tex")
            {
                return this.ToTex();
            }
            else
                return "";
        }

        /// <summary>
        /// Calculate the result of this equation
        /// terms that are valued are operated with its numeric value
        /// </summary>
        /// <returns>string representation number or algebraic</returns>
        public string Calculate()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IArithmetic> Transform()
        {
            return this.MakeTransform();
        }

        public IEquation Factorize()
        {
            throw new NotImplementedException();
        }

        public IEquation Develop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compare two object
        /// </summary>
        /// <param name="x">object left</param>
        /// <param name="y">object right</param>
        /// <returns></returns>
        public virtual bool Equals(IArithmetic x, IArithmetic y)
        {
            return x.OwnerWeight == y.OwnerWeight;
        }

        /// <summary>
        /// Hash code of this object
        /// replacement with the weight hash code
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>hash code</returns>
        public int GetHashCode(IArithmetic obj)
        {
            return obj.OwnerWeight.GetHashCode();
        }

        #endregion
    }
}