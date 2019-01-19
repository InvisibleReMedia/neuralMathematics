using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Parenthese content
    /// </summary>
    public class Parenthese : UnaryOperation
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Parenthese()
        {
        }

        /// <summary>
        /// Constructor
        /// given a positive value with one term
        /// </summary>
        /// <param name="n">term</param>
        public Parenthese(IArithmetic n) : base('d', n)
        {
        }

        #endregion

        /// <summary>
        /// Gets the operator char
        /// </summary>
        public override char Operator
        {
            get
            {
                return this[operatorName];
            }
        }

        #region Methods

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        public override string ToTex()
        {
            string output = string.Empty;
            if (this.InnerOperand != null)
                output = @"{\left(" + this.InnerOperand.ToTex() + @"\right)}";
            return output;
        }

        /// <summary>
        /// Transforms equation object into a string representation
        /// </summary>
        /// <returns>string representation</returns>
        public override string ToString()
        {
            string output = string.Empty;
            if (this.InnerOperand != null)
                output = "(" + this.InnerOperand.ToString() + ")";
            return output;
        }

        /// <summary>
        /// Create a new addition class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Parenthese();
        }

        #endregion

    }
}
