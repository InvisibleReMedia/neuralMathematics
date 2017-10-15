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

        public UnknownTerm(string letter)
        {
            this[letterName] = letter;
        }

        #endregion

    }
}
