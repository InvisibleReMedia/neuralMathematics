using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Persistent data format of a negative value
    /// </summary>
    [Serializable]
    public class Negative : UnaryOperation
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Negative()
        {
        }

        /// <summary>
        /// Constructor
        /// given a positive value with one number
        /// </summary>
        /// <param name="n">number</param>
        public Negative(double n) : base('n', n)
        {
        }

        /// <summary>
        /// Constructor
        /// given a positive value with one term
        /// </summary>
        /// <param name="n">term</param>
        public Negative(IArithmetic n) : base('n', n)
        {
        }

        #endregion

        #region Properties

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

        #endregion

        #region Methods

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        public override string ToTex()
        {
            string output = string.Empty;
            if (this.InnerOperand != null)
                output = @"{\left[-\left(" + this.InnerOperand.ToTex() + @"\right)\right]}";
            return output;
        }

        /// <summary>
        /// Create a new addition class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Negative();
        }

        #endregion

    }
}
