using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistantModel;
using Interfaces;

namespace Maths
{
    /// <summary>
    /// Equation class
    /// </summary>
    public class MathematicEquation : IEquation
    {

        #region Fields

        /// <summary>
        /// Equal operator ID
        /// </summary>
        public static string OperatorEqual = "equal";
        /// <summary>
        /// Add operator ID
        /// </summary>
        public static string OperatorAdd = "add";
        /// <summary>
        /// Sub operator ID
        /// </summary>
        public static string OperatorSubstract = "substract";
        /// <summary>
        /// Product operator ID
        /// </summary>
        public static string OperatorProduct = "product";
        /// <summary>
        /// Divide operator ID
        /// </summary>
        public static string OperatorDivide = "divide";
        /// <summary>
        /// Product operator ID
        /// </summary>
        public static string OperatorPower = "power";
        /// <summary>
        /// Divide operator ID
        /// </summary>
        public static string OperatorRoot = "root";
        /// <summary>
        /// Multiple addition operator ID
        /// </summary>
        public static string OperatorMultipleAdd = "multipleAdd";
        /// <summary>
        /// Multiple product operator ID
        /// </summary>
        public static string OperatorMultipleProduct = "multipleProduct";
        /// <summary>
        /// Positive operator ID
        /// </summary>
        public static string OperatorPositive = "positive";
        /// <summary>
        /// Negative operator ID
        /// </summary>
        public static string OperatorNegative = "negative";
        /// <summary>
        /// Inverse operator ID
        /// </summary>
        public static string OperatorInverse = "inverse";

        /// <summary>
        /// Equation data
        /// </summary>
        private IArithmetic eq;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// create an equality of 0 = 0
        /// </summary>
        public MathematicEquation()
        {
            this.eq = new Equal(0.0d, 0.0d);
        }

        /// <summary>
        /// Create an equation
        /// given a numeric value
        /// </summary>
        /// <param name="c">numeric value</param>
        public MathematicEquation(double c)
        {
            this.eq = new NumericValue(c);
        }

        /// <summary>
        /// Create an equation
        /// with a given operator
        /// and two operands
        /// </summary>
        /// <param name="op">operator name</param>
        /// <param name="e1">operand one</param>
        /// <param name="e2">operand two</param>
        public MathematicEquation(string op, IEquation e1, IEquation e2)
        {
            if (op == OperatorEqual)
            {
                this.eq = new Equal(e1.Equation, e2.Equation);
            }
            else if (op == OperatorAdd)
            {
                this.eq = new Addition(e1.Equation, e2.Equation);
            }
            else if (op == OperatorSubstract)
            {
                this.eq = new Soustraction(e1.Equation, e2.Equation);
            }
            else if (op == OperatorProduct)
            {
                this.eq = new Multiplication(e1.Equation, e2.Equation);
            }
            else if (op == OperatorDivide)
            {
                this.eq = new Division(e1.Equation, e2.Equation);
            }
            else if (op == OperatorPower)
            {
                this.eq = new Power(e1.Equation, e2.Equation);
            }
            else if (op == OperatorRoot)
            {
                this.eq = new Root(e1.Equation, e2.Equation);
            }
            else if (op == OperatorMultipleAdd)
            {
                Sum s1 = Arithmetic.EnsureSum(e1.Equation, 1);
                Sum s2 = Arithmetic.EnsureSum(e2.Equation, 1);
                this.eq = new Sum(s1.Items.Concat(s2.Items).ToArray());
            }
            else if (op == OperatorMultipleProduct)
            {
                Product p1 = Arithmetic.EnsureProduct(e1.Equation, 1);
                Product p2 = Arithmetic.EnsureProduct(e2.Equation, 1);
                this.eq = new Product(p1.Items.Concat(p2.Items).ToArray());
            }
            else
                throw new NotSupportedException();
        }

        /// <summary>
        /// Create an equation
        /// with a given operator
        /// and one operand
        /// </summary>
        /// <param name="op">operator name</param>
        /// <param name="e">operand</param>
        public MathematicEquation(string op, IEquation e)
        {
            if (op == OperatorPositive)
            {
                IArithmetic eqTemp = Arithmetic.EnsureSign(e.Equation);
                this.eq = new Positive(eqTemp);
            }
            else if (op == OperatorNegative)
            {
                IArithmetic eqTemp = Arithmetic.EnsureSign(e.Equation);
                this.eq = new Negative(eqTemp);
            }
            else if (op == OperatorInverse)
            {
                IArithmetic eqTemp = Arithmetic.EnsureInverse(e.Equation);
                this.eq = new Inverse(eqTemp);
            }
            else if (op == OperatorMultipleAdd)
            {
                this.eq = Arithmetic.EnsureSum(e.Equation, 1);
            }
            else if (op == OperatorMultipleProduct)
            {
                this.eq = Arithmetic.EnsureProduct(e.Equation, 1);
            }
            else
                throw new NotSupportedException();
        }

        /// <summary>
        /// Constructor to create a coefficient
        /// </summary>
        /// <param name="letter">coefficient letter</param>
        /// <param name="value">value</param>
        public MathematicEquation(string letter, double value)
        {
            this.eq = new Coefficient(letter, value);
        }

        /// <summary>
        /// Constructor to create an unknown term
        /// </summary>
        /// <param name="letter">unknown letter</param>
        /// <param name="value">value</param>
        public MathematicEquation(string letter, IArithmetic value)
        {
            this.eq = new UnknownTerm(letter, value);
        }

        /// <summary>
        /// Constructor to create a term
        /// </summary>
        /// <param name="constant">constant equation</param>
        /// <param name="coef">coefficient equation</param>
        public MathematicEquation(IArithmetic constant, IArithmetic coef, IArithmetic x)
        {
            this.eq = new Term(constant, coef, x);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets all coefficients terms
        /// </summary>
        public IEnumerable<IArithmetic> Coefficients
        {
            get
            {
                return this.eq.Coefficients;
            }
        }

        /// <summary>
        /// Gets all constant values
        /// </summary>
        public IEnumerable<IArithmetic> Constants
        {
            get
            {
                return this.eq.Constants;
            }
        }

        /// <summary>
        /// Gets all unknown terms
        /// </summary>
        public IEnumerable<IArithmetic> UnknownTerms
        {
            get
            {
                return this.eq.UnknownTerms;
            }
        }

        /// <summary>
        /// Gets the underlying arithmetic operation
        /// </summary>
        public IArithmetic Equation
        {
            get
            {
                return this.eq;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// String representation of the algebraic equation
        /// </summary>
        /// <param name="type">type representation</param>
        /// <returns>string text</returns>
        public string AsRepresented(string type)
        {
            return this.eq.AsRepresented(type);
        }

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        public virtual string ToTex()
        {
            return this.eq.ToTex();
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

        public IEquation Develop()
        {
            throw new NotImplementedException();
        }

        public IEquation Factorize()
        {
            throw new NotImplementedException();
        }

        public void Let(string letter, IEquation e)
        {
            throw new NotImplementedException();
        }

        public void Let(string letter, double value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IArithmetic> Transform()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
