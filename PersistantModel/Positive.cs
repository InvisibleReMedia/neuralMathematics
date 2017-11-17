using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Persistent data format of a positive value
    /// </summary>
    [Serializable]
    public class Positive : UnaryOperation
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Positive()
        {
        }

        /// <summary>
        /// Constructor
        /// given a positive value with one number
        /// </summary>
        /// <param name="n">number</param>
        public Positive(double n) : base('p', n)
        {
        }

        /// <summary>
        /// Constructor
        /// given a positive value with one term
        /// </summary>
        /// <param name="n">term</param>
        public Positive(IArithmetic n) : base('p', n)
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
                output = @"{\left[+\left(" + this.InnerOperand.ToTex() + @"\right)\right]}";
            return output;
        }

        /// <summary>
        /// Create a new addition class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Positive();
        }

        #endregion

    }
}
