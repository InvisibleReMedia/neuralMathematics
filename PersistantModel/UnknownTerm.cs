using Interfaces;
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
        private static string equationName = "equation";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected UnknownTerm()
        {

        }

        /// <summary>
        /// Constructor
        /// given a letter name to
        /// identify unknown term
        /// </summary>
        /// <param name="letter">letter</param>
        public UnknownTerm(string letter)
        {
            this[letterName] = letter;
            this[equationName] = new NumericValue(0.0d);
        }

        /// <summary>
        /// Constructor
        /// given a letter name to
        /// identify unknown term
        /// given its equation
        /// </summary>
        /// <param name="letter">ketter</param>
        /// <param name="eq">equation</param>
        public UnknownTerm(string letter, IArithmetic eq)
        {
            this[letterName] = letter;
            this[equationName] = eq.Clone() as IArithmetic;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of this unknown term
        /// </summary>
        public string Name
        {
            get
            {
                return this[letterName];
            }
        }

        /// <summary>
        /// Gets the underlying equation
        /// </summary>
        public IArithmetic Content
        {
            get
            {
                return this[equationName];
            }
        }

        /// <summary>
        /// Gets the operator ID
        /// </summary>
        public override string Operator
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
        /// Gets the left operand
        /// </summary>
        public override IArithmetic LeftOperand
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the right operand
        /// </summary>
        public override IArithmetic RightOperand
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new UnknownTerm();
        }

        #endregion


    }
}
