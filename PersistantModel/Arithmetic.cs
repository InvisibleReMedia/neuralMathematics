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
    public class Arithmetic : PersistentDataObject, IArithmetic, IEquation, ICloneable
    {

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
        /// Gets or sets a value from
        /// persistent data
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>value</returns>
        public dynamic this[string name]
        {
            get
            {
                if (this.Data.ContainsKey(name))
                {
                    return this.Data[name];
                }
                else
                {
                    this.Data.Add(name, null);
                }
                return this.Data[name];
            }
            set
            {
                if (this.Data.ContainsKey(name))
                {
                    this.Data[name] = value;
                }
                else
                {
                    this.Data.Add(name, value);
                }
            }
        }

        /// <summary>
        /// Gets the operator ID
        /// </summary>
        public virtual string Operator
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
                    foreach (IArithmetic e in this.LeftOperand.UnknownTerms) yield return e;
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
                    foreach (IArithmetic e in this.LeftOperand.Coefficients) yield return e;
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
                    foreach (IArithmetic e in this.LeftOperand.Constants) yield return e;
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
        /// Create a new arithmetic class
        /// </summary>
        protected virtual IArithmetic Create()
        {
            return new Arithmetic();
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
                    p[key] = new string(this.Data[key].ToString());
                else
                    p[key] = this.Data[key];
            }
            return p;
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
                output = left + " " + this.Operator + " " + right;
            }
            else if (this.IsUnaryOperator)
            {
                if (this.LeftOperand != null)
                    output = this.Operator + "(" + this.LeftOperand.ToString() + ")";
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
        /// <returns>string text</returns>
        public string AsRepresented()
        {
            return this.ToString();
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

        public IEquation Transform()
        {
            throw new NotImplementedException();
        }

        public IEquation Factorize()
        {
            throw new NotImplementedException();
        }

        public IEquation Develop()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
