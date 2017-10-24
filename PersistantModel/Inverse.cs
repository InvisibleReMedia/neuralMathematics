using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Persistent data format of an inverse value
    /// </summary>
    [Serializable]
    public class Inverse : UnaryOperation
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Inverse()
        {
        }

        /// <summary>
        /// Constructor
        /// given a positive value with one number
        /// </summary>
        /// <param name="n">number</param>
        public Inverse(double n) : base('\\', n)
        {
        }

        /// <summary>
        /// Constructor
        /// given a positive value with one term
        /// </summary>
        /// <param name="n">term</param>
        public Inverse(IArithmetic n) : base('\\', n)
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
            output = @"\frac{1}{" + right + "}";
            return output;
        }

        /// <summary>
        /// Create a new addition class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Inverse();
        }

        #endregion

    }
}
