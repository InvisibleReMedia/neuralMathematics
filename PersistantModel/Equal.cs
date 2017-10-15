using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Equality assertion arithmetic operator
    /// </summary>
    [Serializable]
    public class Equal : Arithmetic
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
        protected Equal()
        {
        }

        /// <summary>
        /// Constructor
        /// given an equality with two numbers
        /// </summary>
        /// <param name="n1">left number</param>
        /// <param name="n2">right number</param>
        public Equal(double n1, double n2)
        {
            this[operatorName] = '=';
            this[leftTerm] = new NumericValue(n1);
            this[rightTerm] = new NumericValue(n2);
        }

        /// <summary>
        /// Constructor
        /// given an equality with two numbers
        /// </summary>
        /// <param name="n1">left number</param>
        /// <param name="n2">right number</param>
        public Equal(Arithmetic n1, Arithmetic n2)
        {
            this[operatorName] = '=';
            this[leftTerm] = n1.Clone() as Arithmetic;
            this[rightTerm] = n2.Clone() as Arithmetic;
        }
        #endregion

    }
}
