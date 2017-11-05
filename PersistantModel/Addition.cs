using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Modèle persistant des données
/// </summary>
namespace PersistantModel
{
    /// <summary>
    /// Persistent data format of an addition
    /// </summary>
    [Serializable]
    public class Addition : BinaryOperation
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Addition()
        {
        }

        /// <summary>
        /// Constructor
        /// given an addition with two numbers
        /// </summary>
        /// <param name="n1">left number</param>
        /// <param name="n2">right number</param>
        public Addition(double n1, double n2) : base('+', n1, n2)
        {
        }

        /// <summary>
        /// Constructor
        /// given an addition with two terms
        /// </summary>
        /// <param name="t1">left term</param>
        /// <param name="t2">right term</param>
        public Addition(IArithmetic t1, IArithmetic t2) : base('+', t1, t2)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Transforms an addition into a multiplication
        /// </summary>
        /// <returns>a list of possibles equation</returns>
        protected override IEnumerable<IArithmetic> MakeTransform()
        {
            List<IArithmetic> list = new List<IArithmetic>();
            Division d = new Division(this.LeftOperand, this.RightOperand);
            Addition a = new Addition(new NumericValue(1.0d), d);
            Multiplication m = new Multiplication(a, this.RightOperand);
            list.Add(m);
            d = new Division(this.RightOperand, this.LeftOperand);
            a = new Addition(new NumericValue(1.0d), d);
            m = new Multiplication(a, this.LeftOperand);
            list.Add(m);
            return list;
        }

        /// <summary>
        /// Convert additions to a sum
        /// </summary>
        /// <returns></returns>
        public Sum ToSum()
        {
            return EnsureSum(this, 1);
        }

        /// <summary>
        /// Create a new addition class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Addition();
        }

        #endregion

    }
}
