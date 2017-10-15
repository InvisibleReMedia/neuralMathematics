using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using PersistantModel;

namespace Maths
{
    public class Equation : Interfaces.IEquation
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
        /// Equation data
        /// </summary>
        private PersistentDataObject eq;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// create an equality of 0 = 0
        /// </summary>
        public Equation()
        {
            this.eq = new Equal(0.0d, 0.0d);
        }

        /// <summary>
        /// Create an equation
        /// given a numeric value
        /// </summary>
        /// <param name="c">numeric value</param>
        public Equation(double c)
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
        public Equation(string op, IEquation e1, IEquation e2)
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
        }

        #endregion

        #region Properties

        public IEnumerable<Coefficient> Coefficients
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<NumericValue> Constants
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<UnknownTerm> UnknownTerms
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        Arithmetic IEquation.Equation
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Methods

        public string AsRepresented()
        {
            throw new NotImplementedException();
        }

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

        public IEquation Transform()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
