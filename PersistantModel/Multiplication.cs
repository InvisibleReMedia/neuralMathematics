﻿using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Persistent data format of a product
    /// </summary>
    [Serializable]
    public class Multiplication : Operation
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Multiplication()
        {
        }

        /// <summary>
        /// Default constructor
        /// given an addition with two numbers
        /// </summary>
        /// <param name="n1">left number</param>
        /// <param name="n2">right number</param>
        public Multiplication(double n1, double n2) : base(n1, n2)
        {
            this[operatorName] = '*';
        }

        /// <summary>
        /// Constructor
        /// given a multiplication with two terms
        /// </summary>
        /// <param name="t1">left term</param>
        /// <param name="t2">right term</param>
        public Multiplication(IArithmetic t1, IArithmetic t2) : base(t1, t2)
        {
            this[operatorName] = '*';
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Multiplication();
        }

        #endregion

    }
}
