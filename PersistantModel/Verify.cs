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
    public class Verify : IDocument
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

        /// <summary>
        /// Gets all variables
        /// </summary>
        public IEnumerable<IVariable> Variables
        {
            get
            {
                return this.equalOp.Coefficients.Concat(this.equalOp.UnknownTerms).Cast<IVariable>();
            }
        }

        /// <summary>
        /// Gets all records
        /// </summary>
        public IEnumerable<Weight> Elements
        {
            get
            {
                return this.equalOp.Records.Records;
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
            foreach (Weight w in this.equalOp.Records.Records)
            {
                w.OwnerObject.Calculate();
            }
            return this.equalOp.LeftOperand.Calculate() == this.equalOp.RightOperand.Calculate();
        }

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        public string ToTex()
        {
            foreach(IVariable v in this.Variables)
            {
            }
        }

        #endregion

    }
}
