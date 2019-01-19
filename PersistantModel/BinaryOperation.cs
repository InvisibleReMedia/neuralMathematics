using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Définit une opération binaire
    /// </summary>
    [Serializable]
    public class BinaryOperation : Arithmetic
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected BinaryOperation()
        {
            this[operatorName] = '+';
            this[leftTermName] = new NumericValue(0.0d);
            this[rightTermName] = new NumericValue(0.0d);
        }

        /// <summary>
        /// Constructor
        /// given a binary operation with two numbers
        /// </summary>
        /// <param name="op">operator id</param>
        /// <param name="n1">left number</param>
        /// <param name="n2">right number</param>
        protected BinaryOperation(char op, double n1, double n2)
        {
            this[operatorName] = op;
            this[leftTermName] = new NumericValue(n1);
            this[rightTermName] = new NumericValue(n2);
        }

        /// <summary>
        /// Constructor
        /// given a binary operation with two terms
        /// </summary>
        /// <param name="op">operator id</param>
        /// <param name="t1">left term</param>
        /// <param name="t2">right term</param>
        protected BinaryOperation(char op, IArithmetic t1, IArithmetic t2)
        {
            this[operatorName] = op;
            this[leftTermName] = t1.Clone();
            this[rightTermName] = t2.Clone();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the operator ID
        /// </summary>
        public override char Operator
        {
            get
            {
                return this[operatorName];
            }
        }

        /// <summary>
        /// Gets binary switch test
        /// </summary>
        public override bool IsBinaryOperator
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets unary switch test
        /// </summary>
        public override bool IsUnaryOperator
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets multiple switch test
        /// </summary>
        public override bool IsMultipleOperator
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets true if it's not an operator
        /// </summary>
        public override bool IsNotOperator
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the left operand
        /// </summary>
        public override IArithmetic LeftOperand
        {
            get
            {
                return this[leftTermName];
            }
        }

        /// <summary>
        /// Gets the right operand
        /// </summary>
        public override IArithmetic RightOperand
        {
            get
            {
                return this[rightTermName];
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the unique weight
        /// for this object
        /// </summary>
        /// <returns>weight</returns>
        protected override Weight ComputeOwnerWeight()
        {
            return Weight.ComputeWeight(this);
        }

        /// <summary>
        /// Create a new addition class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new BinaryOperation();
        }


        /// <summary>
        /// This function tries to obtain a numerical value
        /// but if not returns only equations
        /// </summary>
        /// <returns>a numerical value or an equation</returns>
        public override IArithmetic Compute()
        {
            IArithmetic output = this;
            if (this.LeftOperand != null && this.RightOperand != null)
            {
                IArithmetic left, right;
                left = this.LeftOperand.Compute();
                right = this.RightOperand.Compute();
                if (left is NumericValue && right is NumericValue)
                {
                    switch (this.Operator)
                    {
                        case '+':
                            output = new NumericValue(left.ToDouble() + right.ToDouble());
                            break;
                        case '-':
                            output = new NumericValue(left.ToDouble() - right.ToDouble());
                            break;
                        case '*':
                            output = new NumericValue(left.ToDouble() * right.ToDouble());
                            break;
                        case '/':
                            double d = right.ToDouble();
                            if (d != 0.0d)
                            {
                                output = new NumericValue(left.ToDouble() / d);
                            }
                            else
                            {
                                Arithmetic.RaiseEventError(new OverflowException("valeur 1 / 0"));
                                output = new NumericValue(Double.NaN);
                            }
                            break;
                        case '^':
                            output = new NumericValue(Math.Pow(left.ToDouble(), right.ToDouble()));
                            break;
                        case 'v':
                            double r = right.ToDouble();
                            if (r == 0.0d)
                            {
                                Arithmetic.RaiseEventError(new OverflowException("valeur racine 1 / 0"));
                                output = new NumericValue(1.0d);
                            }
                            else if (r % 2 == 0)
                            {
                                if (left.ToDouble() > 0)
                                {
                                    output = new NumericValue(Math.Pow(left.ToDouble(), 1 / r));
                                }
                                else if (left.ToDouble() == 0)
                                {
                                    output = new NumericValue(0.0d);
                                }
                                else
                                {
                                    output = new NumericValue(0.0d);
                                    Arithmetic.RaiseEventError(new OverflowException("valeur sous la racine carrée négative"));
                                }
                            }
                            else
                                output = new NumericValue(Math.Pow(left.ToDouble(), 1 / r));
                            break;
                    }
                }
            }
            return output;
        }

        #endregion
    }
}
