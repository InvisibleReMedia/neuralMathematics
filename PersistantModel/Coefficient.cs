using Interfaces;
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

        #region Properties

        /// <summary>
        /// Gets the name of this coefficient
        /// </summary>
        public string Name
        {
            get
            {
                return this[letterName];
            }
        }

        /// <summary>
        /// Gets or sets the value of this coefficient
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
        /// Gets multiple switch test
        /// </summary>
        public override bool IsMultipleOperator
        {
            get
            {
                return true;
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
            return new Coefficient();
        }

        #endregion

    }
}
