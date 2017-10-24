using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Sommation of multiple terms
    /// </summary>
    [Serializable]
    public class Sum : Arithmetic
    {

        #region Fields

        /// <summary>
        /// Index name to store operator name
        /// </summary>
        protected static readonly string operatorName = "operator";
        /// <summary>
        /// Index name to store list
        /// </summary>
        protected static readonly string listName = "list";

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected Sum()
        {
        }

        /// <summary>
        /// Constructor with a input list
        /// </summary>
        /// <param name="inputs">input list</param>
        public Sum(params IArithmetic[] inputs)
        {
            this[operatorName] = 's';
            this[listName] = inputs.ToList();
            this[weightName] = this.ComputeOwnerWeight();
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
                return this[operatorName];
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
                return false;
            }
        }

        /// <summary>
        /// Gets the enumeration of elements by sommation
        /// </summary>
        public IEnumerable<IArithmetic> Items
        {
            get
            {
                return this[listName];
            }
        }

        /// <summary>
        /// Gets the list source
        /// </summary>
        protected List<IArithmetic> List
        {
            get
            {
                return this[listName];
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// intermediate transition
        /// </summary>
        /// <param name="current">current sum</param>
        /// <returns>a lesser sum</returns>
        private static Sum IntermediateTransition(Sum current)
        {
            if (current.List.Count == 2)
            {
                Addition a = new Addition(current.List[0], current.List[1]);
                return new Sum(a);
            }
            else if (current.List.Count == 3)
            {
                Addition a = new Addition(current.List[0], current.List[1]);
                Addition a2 = new Addition(a, current.List[2]);
                return new Sum(a2);
            }
            else
            {
                Sum tempSum = null;
                int len = current.List.Count;
                for(int index = 0; index < (len >> 1); ++index)
                {
                    Addition a = new Addition(current.List[index], current.List[len - index - 1]);
                    if (tempSum != null)
                    {
                        tempSum.Add(a);
                    }
                    else
                    {
                        tempSum = new Sum(a);
                    }
                }
                return IntermediateTransition(tempSum);
            }
        }

        /// <summary>
        /// Make a sum to a
        /// tree of addition
        /// </summary>
        /// <returns>a structured addition</returns>
        public Addition Transition()
        {
            if (this.List.Count > 1)
            {
                Sum t = IntermediateTransition(this);
                return (t.List[0] as Addition);
            }
            else if (this.List.Count == 1)
            {
                return new Addition(this.List[0], new NumericValue(0.0));
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Add an addition to the sum
        /// </summary>
        /// <param name="a">element to add</param>
        public void Add(IArithmetic a)
        {
            this.List.Add(a.Clone() as IArithmetic);
        }

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        public override string ToTex()
        {
            string output = string.Empty;
            bool first = true;
            foreach (IArithmetic a in this.Items)
            {
                if (first)
                {
                    output = a.ToTex();
                    first = false;
                }
                else
                    output += " + " + a.ToTex();
            }

            return output;
        }

        /// <summary>
        /// Transforms equation object into a string representation
        /// </summary>
        /// <returns>string representation</returns>
        public override string ToString()
        {
            string output = string.Empty;
            bool first = true;
            foreach (IArithmetic a in this.Items)
            {
                if (first)
                {
                    output = a.ToString();
                    first = false;
                }
                else
                    output += " + " + a.ToString();
            }

            return output;
        }

        /// <summary>
        /// Update data with unique for serialization
        /// before
        /// </summary>
        protected override void UpdateSource()
        {
            for(int index = 0; index < this.List.Count; ++index)
            {
                if (this.List[index] != null && this.List[index].OwnerWeight != null)
                    this.List[index] = this.List[index].OwnerWeight.OwnerObject;
            }
        }

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
            return new Sum();
        }

        #endregion
    }
}
