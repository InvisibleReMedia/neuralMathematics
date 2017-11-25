using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using System.Windows.Documents;

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
        /// <summary>
        /// Tex mode
        /// </summary>
        private bool texMode;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct with an equal term
        /// </summary>
        /// <param name="e">equal term</param>
        public Verify(Equal e) : this(e, false)
        {
        }

        /// <summary>
        /// Construct with an equal term
        /// given a specific mode
        /// </summary>
        /// <param name="e">equal term</param>
        /// <param name="mode">mode</param>
        public Verify(Equal e, bool mode)
        {
            this.equalOp = e;
            this.texMode = mode;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the tex mode
        /// </summary>
        public bool IsTexMode
        {
            get
            {
                return this.texMode;
            }
            set
            {
                this.texMode = value;
            }
        }

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
                w.OwnerObject.Calculate(false);
            }
            return this.equalOp.LeftOperand.Calculate(false) == this.equalOp.RightOperand.Calculate(false);
        }

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        public string ToTex()
        {
            return "";
        }

        /// <summary>
        /// Insert text elements into a list
        /// </summary>
        /// <param name="list">container</param>
        public void InsertIntoDocument(List list)
        {
            Run r;
            if (this.IsValid())
            {
                r = new Run("True");
            }
            else
            {
                r = new Run("False");
            }
            ListItem li = new ListItem(new Paragraph(r));
            list.ListItems.Add(li);
        }

        #endregion

    }
}
