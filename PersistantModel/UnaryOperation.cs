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
        #region Fields

        /// <summary>
        /// Index name to store operator name
        /// </summary>
        protected static string operatorName = "operator";
        /// <summary>
        /// Index name to store value
        /// </summary>
        protected static string valueName = "inner";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected UnaryOperation()
        {
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
            this[valueName] = new NumericValue(n);
            this[weightName] = this.ComputeOwnerWeight();
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
            this[valueName] = t.Clone();
            this[weightName] = this.ComputeOwnerWeight();
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
                return this[valueName];
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Update data with unique for serialization
        /// before
        /// </summary>
        protected override void UpdateSource()
        {
            if (this.InnerOperand != null && this.InnerOperand.OwnerWeight != null)
                this[valueName] = this.InnerOperand.OwnerWeight.OwnerObject;
        }

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
            throw new NotSupportedException();
        }

        #endregion
    }
}
