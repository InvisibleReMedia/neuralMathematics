using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Classe stockant un terme
    /// </summary>
    [Serializable]
    public class Term : Arithmetic
    {

        #region Fields

        /// <summary>
        /// Index name to store a constant
        /// </summary>
        private static string constantName = "constant";
        /// <summary>
        /// Index name to store a coefficient
        /// </summary>
        private static string coefName = "coefficient";
        /// <summary>
        /// Index name to store an unknown term
        /// </summary>
        private static string unknownName = "unknown";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Term()
        {

        }

        /// <summary>
        /// Constructor for a term
        /// </summary>
        /// <param name="constant">store a constant value</param>
        /// <param name="letter">store a coefficient</param>
        /// <param name="unknown">store an unknown term</param>
        public Term(double constant, string letter, string unknown)
        {
            this[constantName] = new NumericValue(constant);
            this[coefName] = new Coefficient(letter);
            this[unknownName] = new UnknownTerm(unknown);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected override Arithmetic Create()
        {
            return new Term();
        }

        #endregion

    }
}
