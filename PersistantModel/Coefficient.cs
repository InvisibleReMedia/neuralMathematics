using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Classe contenant le nom, la valeur numérique
    /// ou une équation d'un coefficient
    /// </summary>
    [Serializable]
    public class Coefficient : Arithmetic
    {

        #region Fields

        /// <summary>
        /// nom pour le champ lettre
        /// </summary>
        private static string letterName = "letter";
        /// <summary>
        /// nom pour le champ valeur
        /// </summary>
        private static string valueName = "value";

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Coefficient()
        {

        }

        /// <summary>
        /// Constructor for a coefficient
        /// given a name
        /// given a double precision numeric value
        /// </summary>
        /// <param name="letter">letter of coefficient</param>
        /// <param name="value">value behind this letter</param>
        public Coefficient(string letter, double value)
        {
            this[letterName] = letter;
            this[valueName] = value;
        }

        /// <summary>
        /// Constructor for a coefficient
        /// given a name
        /// with value as 0.0
        /// </summary>
        /// <param name="letter">letter of coefficient</param>
        public Coefficient(string letter)
        {
            this[letterName] = letter;
            this[valueName] = 0.0d;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected override Arithmetic Create()
        {
            return new Coefficient();
        }

        #endregion

    }
}
