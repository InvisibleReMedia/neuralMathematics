using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace PersistantModel
{
    /// <summary>
    /// Verify the validity of an equal
    /// formula
    /// </summary>
    public class Verify
    {

        #region Fields

        /// <summary>
        /// Equal operation
        /// </summary>
        private Equal equalOp;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct with an equal term
        /// </summary>
        /// <param name="e">equal term</param>
        public Verify(Equal e)
        {
            this.equalOp = e;
        }

        #endregion

        #region Properties

        public IEnumerable<IArithmetic> Variables
        {
            get
            {
                return this.equalOp.Coefficients.Concat(this.equalOp.UnknownTerms);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if valid</returns>
        public bool IsValid()
        {
            return false;
        }

        #endregion

    }
}
