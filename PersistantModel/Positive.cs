﻿using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Persistent data format of a positive value
    /// </summary>
    [Serializable]
    public class Positive : UnaryOperation
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Positive()
        {
        }

        /// <summary>
        /// Constructor
        /// given a positive value with one number
        /// </summary>
        /// <param name="n">number</param>
        public Positive(double n) : base(n)
        {
            this[operatorName] = 'p';
        }

        /// <summary>
        /// Constructor
        /// given a positive value with one term
        /// </summary>
        /// <param name="n">term</param>
        public Positive(IArithmetic n) : base(n)
        {
            this[operatorName] = 'p';
        }

        #endregion

        /// <summary>
        /// Gets the operator char
        /// </summary>
        public override char Operator
        {
            get
            {
                return '\0';
            }
        }

        #region Methods

        /// <summary>
        /// Create a new addition class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Positive();
        }

        #endregion

    }
}
