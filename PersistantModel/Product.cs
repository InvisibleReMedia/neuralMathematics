using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Product of multiple terms
    /// </summary>
    [Serializable]
    public class Product : Arithmetic
    {

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected Product()
        {
        }

        /// <summary>
        /// Constructor with a input list
        /// </summary>
        /// <param name="inputs">input list</param>
        public Product(params IArithmetic[] inputs)
        {
            this[operatorName] = 'r';
            this[listName] = inputs.ToList();
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
        private static Product IntermediateTransition(Product current)
        {
            if (current.List.Count == 2)
            {
                Multiplication m = new Multiplication(current.List[0], current.List[1]);
                return new Product(m);
            }
            else if (current.List.Count == 3)
            {
                Multiplication m = new Multiplication(current.List[0], current.List[1]);
                Multiplication m2 = new Multiplication(m, current.List[2]);
                return new Product(m2);
            }
            else
            {
                Product tempPro = null;
                int len = current.List.Count;
                for(int index = 0; index < (len >> 1); ++index)
                {
                    Multiplication m = new Multiplication(current.List[index], current.List[len - index - 1]);
                    if (tempPro != null)
                    {
                        tempPro.Add(m);
                    }
                    else
                    {
                        tempPro = new Product(m);
                    }
                }
                return IntermediateTransition(tempPro);
            }
        }

        /// <summary>
        /// Make a product to a
        /// tree of multiplication
        /// </summary>
        /// <returns>a structured addition</returns>
        public Multiplication Transition()
        {
            if (this.List.Count > 1)
            {
                Product t = IntermediateTransition(this);
                return (t.List[0] as Multiplication);
            }
            else if (this.List.Count == 1)
            {
                return new Multiplication(new NumericValue(1.0), this.List[0]);
            }
            else
            {
                throw new IndexOutOfRangeException();
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
                    if (a is Addition || a is Soustraction || a is Sum)
                        output = @"{\left[ " + a.ToTex() + @" \right]}";
                    else
                        output = @"{ " + a.ToTex() + @" }";
                    first = false;
                }
                else
                    if (a is Addition || a is Soustraction || a is Sum)
                    output += @" * {\left[ " + a.ToTex() + @" \right]}";
                else
                    output += @" * { " + a.ToTex() + @" }";
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
                    if (a is Addition || a is Soustraction || a is Sum)
                        output = "[" + a.ToString() + "]";
                    else
                        output = a.ToString();
                    first = false;
                }
                else
                    if (a is Addition || a is Soustraction || a is Sum)
                    output += " * [" + a.ToString() + "]";
                else
                    output += " * " + a.ToString();
            }

            return output;
        }

        /// <summary>
        /// This function tries to obtain a numerical value
        /// but if not returns only equations
        /// </summary>
        /// <returns>a numerical value or an equation</returns>
        public override IArithmetic Compute()
        {
            // leaves values
            double leaves = 0.0d;
            // output not calculable
            Product lefts = new Product();
            lefts[listName] = new List<IArithmetic>();

            bool first = true;

            foreach (IArithmetic a in this.Items)
            {
                if (first)
                {
                    IArithmetic element = a.Compute();
                    if (element is NumericValue)
                    {
                        leaves = (element as NumericValue).Value;
                    }
                    first = false;
                }
                else
                {
                    IArithmetic element = a.Compute();
                    if (element is NumericValue)
                    {
                        leaves *= (element as NumericValue).Value;
                    }
                    else
                    {
                        lefts[listName].Add(element);
                    }
                }
            }

            IArithmetic output;
            if (leaves != 0.0d)
            {
                if (lefts.Items.Count() > 0)
                {
                    lefts.Add(new NumericValue(leaves));
                    output = lefts;
                }
                else
                {
                    output = new NumericValue(leaves);
                }

            }
            else
            {
                if (lefts.Items.Count() > 0)
                {
                    output = lefts;
                }
                else
                {
                    output = new NumericValue(0.0d);
                }
            }
            return output;
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
        /// Add a multiplication to the product
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
        /// Create a new arithmetic class
        /// </summary>
        protected override IArithmetic Create()
        {
            return new Product();
        }

        #endregion
    }
}
