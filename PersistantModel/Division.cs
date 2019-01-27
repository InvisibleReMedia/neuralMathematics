using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Persistent data format of a division
    /// </summary>
    [Serializable]
    public class Division : BinaryOperation
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Division()
        {
        }

        /// <summary>
        /// Default constructor
        /// given an addition with two numbers
        /// </summary>
        /// <param name="n1">left number</param>
        /// <param name="n2">right number</param>
        public Division(double n1, double n2) : base('/', n1, n2)
        {
        }

        /// <summary>
        /// Constructor
        /// given a multiplication with two terms
        /// </summary>
        /// <param name="t1">left term</param>
        /// <param name="t2">right term</param>
        public Division(IArithmetic t1, IArithmetic t2) : base('/', t1, t2)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert mult to a product
        /// </summary>
        /// <returns>product</returns>
        public Product ToProduct()
        {
            return EnsureProduct(this, -1);
        }

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
            output = @"\frac{" + left + @"}{" + right + @"}";
            return output;
        }

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Division();
        }

        #endregion

    }
}
