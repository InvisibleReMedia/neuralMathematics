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
    public class Equal : Operation
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
        public Equal(double n1, double n2) : base(n1, n2)
        {
            this[operatorName] = '=';
        }

        /// <summary>
        /// Constructor
        /// given an equality with two equations
        /// </summary>
        /// <param name="t1">left equation</param>
        /// <param name="t2">right equation</param>
        public Equal(IArithmetic t1, IArithmetic t2) : base(t1, t2)
        {
            this[operatorName] = '=';
        }
        #endregion

        #region Methods

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
