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
    public class Power : Operation
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Power()
        {
        }

        /// <summary>
        /// Default constructor
        /// given a n2 power of n1
        /// </summary>
        /// <param name="n1">left number</param>
        /// <param name="n2">right number</param>
        public Power(double n1, double n2) : base(n1, n2)
        {
            this[operatorName] = '^';
        }

        /// <summary>
        /// Constructor
        /// given a n2 power of n1
        /// </summary>
        /// <param name="t1">left term</param>
        /// <param name="t2">right term</param>
        public Power(IArithmetic t1, IArithmetic t2) : base(t1, t2)
        {
            this[operatorName] = '^';
        }

        #endregion

        #region Methods

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
