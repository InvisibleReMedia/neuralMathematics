using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Persistent data format of a substract
    /// </summary>
    [Serializable]
    public class Soustraction : BinaryOperation
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Soustraction()
        {
        }

        /// <summary>
        /// Constructor
        /// given an addition with two numbers
        /// </summary>
        /// <param name="n1">left number</param>
        /// <param name="n2">right number</param>
        public Soustraction(double n1, double n2) : base(n1, n2)
        {
            this[operatorName] = '-';
        }

        /// <summary>
        /// Constructor
        /// given an addition with two terms
        /// </summary>
        /// <param name="t1">left term</param>
        /// <param name="t2">right term</param>
        public Soustraction(IArithmetic t1, IArithmetic t2) : base(t1, t2)
        {
            this[operatorName] = '-';
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert additions to a sum
        /// </summary>
        /// <returns></returns>
        public Sum ToSum()
        {
            return EnsureSum(this, -1);
        }

        /// <summary>
        /// Create a new addition class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Soustraction();
        }

        #endregion

    }
}
