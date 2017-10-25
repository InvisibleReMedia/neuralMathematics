using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    /// <summary>
    /// Interface for an equation
    /// </summary>
    public interface IEquation
    {

        #region Properties

        /// <summary>
        /// Gets true if an equation is calculable
        /// </summary>
        bool IsCalculable { get; }
        /// <summary>
        /// Gets all unknown terms
        /// </summary>
        IEnumerable<IArithmetic> UnknownTerms { get; }

        /// <summary>
        /// Gets all coefficients terms
        /// </summary>
        IEnumerable<IArithmetic> Coefficients { get; }

        /// <summary>
        /// Gets all constant values
        /// </summary>
        IEnumerable<IArithmetic> Constants { get; }

        /// <summary>
        /// Gets the underlying arithmetic operation
        /// </summary>
        IArithmetic Equation { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Generates a new arithmetic object
        /// that's handle by a unique record zone
        /// </summary>
        /// <returns>arithmetic object</returns>
        IArithmetic MakeUnique();
        /// <summary>
        /// Let a letter as a value
        /// given a letter and its value
        /// </summary>
        /// <param name="letter">letter value</param>
        /// <param name="value">numeric value</param>
        void Let(string letter, double value);

        /// <summary>
        /// Let a letter as an arithmetic operation
        /// </summary>
        /// <param name="letter">letter value</param>
        /// <param name="e">arithmetic expression</param>
        void Let(string letter, IEquation e);

        /// <summary>
        /// String representation of the algebraic equation
        /// </summary>
        /// <param name="type">type of representation ; string or tex</param>
        /// <returns>string text</returns>
        string AsRepresented(string type);

        /// <summary>
        /// Calculate the result of this equation
        /// terms that are valued are operated with its numeric value
        /// </summary>
        /// <returns>string representation number or algebraic</returns>
        string Calculate();

        /// <summary>
        /// Transform the current equation to an
        /// another equation
        /// </summary>
        /// <returns>transformed equation</returns>
        IEnumerable<IArithmetic> Transform();

        /// <summary>
        /// Factorization of an equation
        /// works only on a current equation as an addition
        /// </summary>
        /// <returns>factorized equation</returns>
        IEquation Factorize();

        /// <summary>
        /// Develope equation
        /// works only on a current equation as a product
        /// </summary>
        /// <returns>developed equation</returns>
        IEquation Develop();

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        string ToTex();

        #endregion

    }
}
