using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Equality assertion arithmetic operator
    /// </summary>
    [Serializable]
    public class Equal : BinaryOperation
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Equal()
        {
        }

        /// <summary>
        /// Constructor
        /// given an equality with two numbers
        /// </summary>
        /// <param name="n1">left number</param>
        /// <param name="n2">right number</param>
        public Equal(double n1, double n2) : base('=', n1, n2)
        {
        }

        /// <summary>
        /// Constructor
        /// given an equality with two equations
        /// </summary>
        /// <param name="t1">left equation</param>
        /// <param name="t2">right equation</param>
        public Equal(IArithmetic t1, IArithmetic t2) : base('=', t1, t2)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        public override string ToTex()
        {
            string output = string.Empty;
            string left = string.Empty, right = string.Empty;
            if (this.LeftOperand != null)
                left = this.LeftOperand.ToTex();
            if (this.RightOperand != null)
                right = this.RightOperand.ToTex();
            output = @"{" + left + " " + this.Operator + " " + right + @"}";
            return output;
        }

        /// <summary>
        /// Transforms equation object into a string representation
        /// </summary>
        /// <returns>string representation</returns>
        public override string ToString()
        {
            string output = string.Empty;

            string left = string.Empty, right = string.Empty;
            if (this.LeftOperand != null)
                left = this.LeftOperand.ToString();
            if (this.RightOperand != null)
                right = this.RightOperand.ToString();
            output = left + " " + this.Operator + " " + right;
            return output;
        }

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Equal();
        }

        #endregion
    }
}
