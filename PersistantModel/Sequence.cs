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
    public class Sequence : Arithmetic
    {

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected Sequence()
        {
            this[operatorName] = 'q';
            this[listName] = new List<IArithmetic>();
        }

        /// <summary>
        /// Constructor with a input list
        /// </summary>
        /// <param name="inputs">input list</param>
        public Sequence(params IArithmetic[] inputs)
        {
            this[operatorName] = 'q';
            this[listName] = inputs.ToList();
        }

        /// <summary>
        /// Constructor with a input list
        /// </summary>
        /// <param name="list">input list</param>
        public Sequence(List<IArithmetic> list)
        {
            this[operatorName] = 'q';
            this[listName] = list;
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
        /// Add an addition to the sum
        /// </summary>
        /// <param name="a">element to add</param>
        public void Add(IArithmetic a)
        {
            this.List.Add(a.Clone() as IArithmetic);
        }

        /// <summary>
        /// Replace an element to an another
        /// </summary>
        /// <param name="old">old element</param>
        /// <param name="n">new element</param>
        public void Replace(IArithmetic old, IArithmetic n)
        {
            int p = this.List.IndexOf(old);
            if (p != -1)
            {
                this.List[p] = n;
            }
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
                    output += " , " + a.ToTex();
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
                    output += " , " + a.ToString();
            }

            return output;
        }

        /// <summary>
        /// Flat the tree
        /// </summary>
        /// <param name="s">sequence</param>
        /// <returns>flat list</returns>
        public static IEnumerable<IArithmetic> ToFlat(Sequence s)
        {
            List<IArithmetic> flat = new List<IArithmetic>();
            foreach (IArithmetic a in s.List)
            {
                if (a is Sequence)
                {
                    flat.AddRange(Sequence.ToFlat(a as Sequence));
                }
                else
                {
                    flat.Add(a);
                }
            }
            return flat;
        }

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Sequence();
        }

        #endregion
    }
}
