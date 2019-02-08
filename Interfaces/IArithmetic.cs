using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    /// <summary>
    /// Interface d'objets arithmetiques
    /// </summary>
    public interface IArithmetic : IEquation, ICloneable
    {
        /// <summary>
        /// Event to fetch all data into
        /// a unique list of unique records
        /// </summary>
        event EventHandler Fetch;
        /// <summary>
        /// Event to unfetch all data into
        /// a unique list of unique records
        /// </summary>
        event EventHandler Unfetch;
        /// <summary>
        /// Gets the unique content
        /// </summary>
        IWeight OwnerWeight { get; }
        /// <summary>
        /// Gets an object from a serialized dictionary content
        /// </summary>
        /// <param name="name">key name</param>
        /// <returns>dynamix object (all data type is possible)</returns>
        dynamic this[string name] { get; set; }
        /// <summary>
        /// This function tries to obtain a numerical value
        /// but if not returns only equations
        /// </summary>
        /// <returns>a numerical value or an equation</returns>
        IArithmetic Compute();
        /// <summary>
        /// Convert an arithmetic object to a double value
        /// </summary>
        /// <returns>double value</returns>
        double ToDouble();
        /// <summary>
        /// Converts all sub-variables into an equation
        /// or into its value
        /// </summary>
        /// <returns>output new equation</returns>
        IArithmetic Converting();
        /// <summary>
        /// Converts all sub-variables into an equation
        /// or into its value
        /// </summary>
        /// <returns>output new equation</returns>
        IArithmetic ConvertingOne();
        /// <summary>
        /// Select all terms accordingly with model
        /// </summary>
        /// <param name="model">model to search</param>
        /// <returns>list of matches</returns>
        IEnumerable<IArithmetic> Select(IArithmetic model);


    }
}
