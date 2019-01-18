using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Définit une opération unaire
    /// </summary>
    [Serializable]
    public class UnaryOperation : Arithmetic
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected UnaryOperation()
        {
            this[operatorName] = 'p';
            this[innerOperandName] = new NumericValue(0.0d);
        }

        /// <summary>
        /// Constructor
        /// given a unary operation with one number
        /// </summary>
        /// <param name="op">operator</param>
        /// <param name="n">number</param>
        protected UnaryOperation(char op, double n)
        {
            this[operatorName] = op;
            this[innerOperandName] = new NumericValue(n);
        }

        /// <summary>
        /// Constructor
        /// given a unary operation with one term
        /// </summary>
        /// <param name="op">operator</param>
        /// <param name="t">term</param>
        protected UnaryOperation(char op, IArithmetic t)
        {
            this[operatorName] = op;
            this[innerOperandName] = t.Clone();
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
                return false;
            }
        }

        /// <summary>
        /// Gets unary switch test
        /// </summary>
        public override bool IsUnaryOperator
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
        public override IArithmetic InnerOperand
        {
            get
            {
                return this[innerOperandName];
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
            return new UnaryOperation();
        }

        /// <summary>
        /// This function tries to obtain a numerical value
        /// but if not returns only equations
        /// </summary>
        /// <returns>a numerical value or an equation</returns>
        public override IArithmetic Compute()
        {
            IArithmetic output = this;
            if (this.InnerOperand != null)
            {
                IArithmetic inner = this.InnerOperand.Compute();
                switch (this.Operator)
                {
                    case 'p':
                        output = this.InnerOperand.Compute();
                        break;
                    case 'n':
                        output = new NumericValue(-this.InnerOperand.Compute().ToDouble());
                        break;
                    case '\\':
                        double d = Convert.ToDouble(this.InnerOperand);
                        if (d != 0.0d)
                            output = new NumericValue(1 / d);
                        else
                            output = new NumericValue(Double.NaN);
                        break;
                }
            }
            return output;
        }

        #endregion
    }
}
