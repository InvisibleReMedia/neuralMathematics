using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Persistent data format of an addition
    /// </summary>
    [Serializable]
    public class Addition : Arithmetic
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
        /// given an addition of two numbers 0 + 0
        /// </summary>
        public Addition()
        {
            this[operatorName] = '+';
            this[leftTerm] = new NumericValue(0.0d);
            this[rightTerm] = new NumericValue(0.0d);
        }

        /// <summary>
        /// Default constructor
        /// given an addition with two numbers
        /// </summary>
        /// <param name="n1">left number</param>
        /// <param name="n2">right number</param>
        public Addition(double n1, double n2)
        {
            this[operatorName] = '+';
            this[leftTerm] = new NumericValue(n1);
            this[rightTerm] = new NumericValue(n2);
        }

        #endregion

    }
}
