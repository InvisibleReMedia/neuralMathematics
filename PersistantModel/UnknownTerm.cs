using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Terme inconnu
    /// </summary>
    [Serializable]
    public class UnknownTerm : Arithmetic
    {

        #region Fields

        /// <summary>
        /// Index name to store letter value
        /// </summary>
        private static string letterName = "letter";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected UnknownTerm()
        {

        }

        /// <summary>
        /// Constructor
        /// given a letter name to
        /// identify unknown term
        /// </summary>
        /// <param name="letter"></param>
        public UnknownTerm(string letter)
        {
            this[letterName] = letter;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected override Arithmetic Create()
        {
            return new UnknownTerm();
        }

        #endregion


    }
}
