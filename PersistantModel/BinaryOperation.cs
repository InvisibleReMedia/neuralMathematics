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
        #region Fields

        /// <summary>
        /// Index name to store operator name
        /// </summary>
        protected static string operatorName = "operator";
        /// <summary>
        /// Index name to store left value
        /// </summary>
        protected static string leftTermName = "left";
        /// <summary>
        /// Index name to store right value
        /// </summary>
        protected static string rightTermName = "right";
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected BinaryOperation()
        {
        }

        /// <summary>
        /// Constructor
        /// given a binary operation with two numbers
        /// </summary>
        /// <param name="n1">left number</param>
        /// <param name="n2">right number</param>
        protected BinaryOperation(double n1, double n2)
        {
            this[leftTermName] = new NumericValue(n1);
            this[rightTermName] = new NumericValue(n2);
        }

        /// <summary>
        /// Constructor
        /// given a binary operation with two terms
        /// </summary>
        /// <param name="t1">left term</param>
        /// <param name="t2">right term</param>
        protected BinaryOperation(IArithmetic t1, IArithmetic t2)
        {
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
        /// Create a new addition class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new BinaryOperation();
        }

        #endregion
    }
}
