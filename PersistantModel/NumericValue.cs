using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Classe stockant une valeur numérique
    /// </summary>
    [Serializable]
    public class NumericValue : Arithmetic
    {

        #region Fields

        /// <summary>
        /// Index name to store value
        /// </summary>
        private static string valueName = "value";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for a double precision number
        /// </summary>
        /// <param name="d">number</param>
        public NumericValue(double d)
        {
            this[valueName] = d;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the numeric value
        /// </summary>
        public double Value
        {
            get
            {
                return this[valueName];
            }
            set
            {
                this[valueName] = value;
            } 
        }

        #endregion

    }
}
