using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Persistent data format of a power
    /// </summary>
    [Serializable]
    public class Power : BinaryOperation
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Power()
        {
            this[operatorName] = '^';
        }

        /// <summary>
        /// Default constructor
        /// given a n2 power of n1
        /// </summary>
        /// <param name="n1">left number</param>
        /// <param name="n2">right number</param>
        public Power(double n1, double n2) : base('^', n1, n2)
        {
        }

        /// <summary>
        /// Constructor
        /// given a n2 power of n1
        /// </summary>
        /// <param name="t1">left term</param>
        /// <param name="t2">right term</param>
        public Power(IArithmetic t1, IArithmetic t2) : base('^', t1, t2)
        {
        }

        /// <summary>
        /// Constructor
        /// given a constant t2 power of t1
        /// </summary>
        /// <param name="t1">left term</param>
        /// <param name="t2">right term</param>
        public Power(IArithmetic t1, double t2)
            : base('^', t1, new NumericValue(t2))
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
            if (this.LeftOperand is Addition || this.LeftOperand is Soustraction || this.LeftOperand is Sum || this.LeftOperand is Product)
                output = @"{\left(" + left + @"\right)}";
            else
                output = "{" + left + "}";
            if (this.RightOperand is Addition || this.RightOperand is Soustraction || this.RightOperand is Sum || this.RightOperand is Product)
                output += @" ^ {\left(" + right + @"\right)}";
            else
                output += @" ^ {" + right + "}";
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
                left = this.LeftOperand.ToTex();
            if (this.RightOperand != null)
                right = this.RightOperand.ToTex();
            if (this.LeftOperand is Addition || this.LeftOperand is Soustraction || this.LeftOperand is Sum || this.LeftOperand is Product)
                output = "(" + left + ")";
            else
                output = " " + left + " ";
            if (this.RightOperand is Addition || this.RightOperand is Soustraction || this.RightOperand is Sum || this.RightOperand is Product)
                output += " ^ (" + right + ")";
            else
                output += " ^ " + right;
            return output;
        }

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Power();
        }

        #endregion

    }
}
