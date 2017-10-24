using Interfaces;
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
        private static readonly string constantName = "constant";
        /// <summary>
        /// Index name to store a coefficient
        /// </summary>
        private static readonly string coefName = "coefficient";
        /// <summary>
        /// Index name to store an unknown term
        /// </summary>
        private static readonly string unknownName = "unknown";

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
        /// Constructor for a simple term
        /// </summary>
        /// <param name="constant">store a constant value</param>
        /// <param name="letter">store a coefficient</param>
        /// <param name="unknown">store an unknown term</param>
        public Term(double constant, string letter, string unknown)
        {
            this[constantName] = new NumericValue(constant);
            this[coefName] = new Coefficient(letter, 0.0d);
            this[unknownName] = new UnknownTerm(unknown, new NumericValue(0.0d));
        }

        /// <summary>
        /// Constructor for a term
        /// </summary>
        /// <param name="c">constant equation</param>
        /// <param name="s">coefficient equation</param>
        /// <param name="u">unknown term equation</param>
        public Term(IArithmetic c, IArithmetic s, IArithmetic u)
        {
            this[constantName] = c.Clone();
            this[coefName] = s.Clone();
            this[unknownName] = u.Clone();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the operator ID
        /// </summary>
        public override char Operator
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets binary switch test
        /// </summary>
        public override bool IsBinaryOperator
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets unary switch test
        /// </summary>
        public override bool IsUnaryOperator
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets true if it's not an operator
        /// </summary>
        public override bool IsNotOperator
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the constant element
        /// </summary>
        public IArithmetic Constant
        {
            get { return this[constantName]; }
        }

        /// <summary>
        /// Gets the coefficient element
        /// </summary>
        public IArithmetic Coefficient
        {
            get { return this[coefName]; }
        }

        /// <summary>
        /// Gets the unknown element
        /// </summary>
        public IArithmetic Unknown
        {
            get { return this[unknownName]; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Term();
        }

        #endregion

    }
}
