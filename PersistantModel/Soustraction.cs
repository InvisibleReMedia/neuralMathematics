using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Persistent data format of a substract
    /// </summary>
    [Serializable]
    public class Soustraction : Arithmetic
    {

        #region Fields

        /// <summary>
        /// Index name to store operator name
        /// </summary>
        private static string operatorName = "operator";
        /// <summary>
        /// Index name to store left value
        /// </summary>
        private static string leftTerm = "left";
        /// <summary>
        /// Index name to store right value
        /// </summary>
        private static string rightTerm = "right";
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Soustraction()
        {
        }

        /// <summary>
        /// Constructor
        /// given an addition with two numbers
        /// </summary>
        /// <param name="n1">left number</param>
        /// <param name="n2">right number</param>
        public Soustraction(double n1, double n2)
        {
            this[operatorName] = '-';
            this[leftTerm] = new NumericValue(n1);
            this[rightTerm] = new NumericValue(n2);
        }

        /// <summary>
        /// Constructor
        /// given an addition with two terms
        /// </summary>
        /// <param name="t1">left term</param>
        /// <param name="t2">right term</param>
        public Soustraction(Arithmetic t1, Arithmetic t2)
        {
            this[operatorName] = '-';
            this[leftTerm] = t1.Clone();
            this[rightTerm] = t2.Clone();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new addition class
        /// </summary>
        protected override Arithmetic Create()
        {
            return new Soustraction();
        }

        #endregion

    }
}
