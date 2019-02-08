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
    public class Coefficient : Arithmetic, IVariable
    {

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Coefficient()
        {
            this[letterName] = "p";
            this[hasValueName] = false;
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
            this[hasValueName] = true;
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
            this[hasValueName] = false;
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
        /// Gets the value of this coefficient
        /// </summary>
        dynamic IVariable.Value
        {
            get
            {
                return this.Value;
            }
            set
            {
                this.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of this coefficient
        /// </summary>
        public double? Value
        {
            get
            {
                return Convert.ToDouble(this[valueName]);
            }
            set
            {
                this[valueName] = value;
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

        #endregion

        #region Methods

        /// <summary>
        /// Computes the unique weight
        /// for this object
        /// </summary>
        /// <returns>weight</returns>
        protected override Weight ComputeOwnerWeight()
        {
            return Weight.ComputeWeight(this);
        }

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Coefficient();
        }

        /// <summary>
        /// Select all terms accordingly with model
        /// </summary>
        /// <param name="model">model to search</param>
        /// <returns>list of elements</returns>
        public override IEnumerable<IArithmetic> Select(IArithmetic model)
        {
            if (this.Name == model.ToString())
                yield return model;
            else
                yield break;
        }

        #endregion

    }
}
