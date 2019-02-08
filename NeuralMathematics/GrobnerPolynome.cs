using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistantModel;
using Interfaces;

namespace NeuralMathematics
{
    /// <summary>
    /// A recursive polynome
    /// A polynome is a Base X
    /// </summary>
    public class GrobnerPolynome
    {

        #region Fields

        /// <summary>
        /// Base
        /// </summary>
        private Base b;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a grobner polynome
        /// </summary>
        /// <param name="order">order</param>
        public GrobnerPolynome(int order)
        {
            this.b = new Base(10, order + 1);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets arithmetic formula
        /// </summary>
        public IArithmetic PolynomeAsFunction
        {
            get
            {
                IEnumerable<double> v = this.b.Vector;
                IEnumerable<string> s = this.b.Coefficients;
                bool first = true;
                string sum = string.Empty;
                for (int index = v.Count() - 1; index >= 0; --index)
                {
                    IArithmetic product;
                    if (index > 0)
                    {
                        product = new Multiplication(new Coefficient(s.ElementAt(s.Count() - 1 - index)), new Power(new UnknownTerm("X"), index));
                    }
                    else
                    {
                        product = new Coefficient(s.ElementAt(s.Count() - 1 - index));
                    }
                    if (!first) sum += " + "; else first = false;
                    sum += product.ToString();
                }
                return sum.ToArithmetic();
            }
        }

        /// <summary>
        /// Gets the base of this polynome
        /// </summary>
        public Base Base
        {
            get { return this.b; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a polynome as a base
        /// </summary>
        /// <param name="name">indice names</param>
        /// <param name="uterm">unknown term</param>
        /// <returns>an arithmetic function</returns>
        public IArithmetic PolynomeAsBase(string name, string uterm)
        {
            IEnumerable<double> v = this.b.Vector;
            IEnumerable<Coefficient> s = this.b.Indices(name);
            bool first = true;
            string sum = string.Empty;
            for (int index = v.Count() - 1; index >= 0; --index)
            {
                IArithmetic product;
                if (index > 0)
                {
                    product = new Multiplication(s.ElementAt(s.Count() - 1 - index), new Power(new UnknownTerm(uterm), index));
                }
                else
                {
                    product = s.ElementAt(s.Count() - 1 - index).Clone() as IArithmetic;
                }
                if (!first) sum += " + "; else first = false;
                sum += product.ToString();
            }
            return sum.ToArithmetic();
        }

        /// <summary>
        /// Gets Base Y
        /// </summary>
        /// <param name="y">y value</param>
        /// <returns>arithmetic function</returns>
        public IArithmetic GetBaseY(double y)
        {
            Base Y = new Base(10, Base.ConvertToReadable(y, 10));
            // creates a polynome for X
            GrobnerPolynome p = new GrobnerPolynome(Y.Dimension - 1);
            IArithmetic a = p.PolynomeAsBase("p", "X");

            // gestion variables
            Dictionary<string, IArithmetic> variables = new Dictionary<string, IArithmetic>();

            Arithmetic.EventAddVariable += new EventHandler<KeyValuePair<string, IArithmetic>>((o, e) =>
            {
                if (e.Value != null)
                {
                    if (variables.ContainsKey(e.Key))
                    {
                        variables[e.Key] = e.Value;
                    }
                    else
                    {
                        variables.Add(e.Key, e.Value);
                    }
                }
                else
                {
                    if (variables.ContainsKey(e.Key))
                        variables.Remove(e.Key);
                }
            });
            Arithmetic.EventGetVariable = new Func<string, IArithmetic>((s) =>
            {
                if (variables.ContainsKey(s)) return variables[s];
                else return null;
            });


            for (int index = Y.Dimension - 1; index >= 0; --index)
            {
                a.Let("p_" + index.ToString(), Y.Vector.ElementAt(index));
            }
            a.Let("X", this.GetBaseX(3));

            return a.Converting();
        }

        /// <summary>
        /// Gets Base X
        /// </summary>
        /// <param name="size">size of X</param>
        /// <returns>arithmetic function</returns>
        public IArithmetic GetBaseX(int size)
        {
            Base X = new Base(10, size);
            // creates a polynome for X
            GrobnerPolynome p = new GrobnerPolynome(X.Dimension - 1);
            IArithmetic a = p.PolynomeAsBase("n", "B");

            return a;
        }

        /// <summary>
        /// Gets the pascal list of values
        /// </summary>
        /// <param name="order">order</param>
        /// <param name="source">list origin</param>
        /// <returns>new list</returns>
        public IEnumerable<uint> TrianglePascal(uint order, IEnumerable<uint> source)
        {
            if (order > 1)
            {
                List<uint> list = new List<uint>();
                bool first = true;
                uint previous = 0;
                list.Add(source.First());
                foreach (uint x in source)
                {
                    if (!first)
                    {
                        list.Add(previous + x);
                    }
                    else
                        first = false;
                    previous = x;
                }
                list.Add(source.Last());
                return TrianglePascal(order - 1, list);
            }
            else
            {
                return source;
            }
        }

        /// <summary>
        /// Computes the pascal triangle
        /// </summary>
        /// <param name="order">order</param>
        /// <returns>list result</returns>
        public IEnumerable<uint> TrianglePascal(uint order)
        {
            return TrianglePascal(order, new List<uint>() { 1, 1 });
        }

        /// <summary>
        /// Loi binomiale
        /// </summary>
        /// <param name="order">order</param>
        /// <param name="u">unknown term</param>
        /// <param name="v">coefficient</param>
        /// <returns>developed equation</returns>
        public IArithmetic BinomialLaw(uint order, string u, string v)
        {
            Sum sum = new Sum();
            List<IArithmetic> terms = new List<IArithmetic>();
            IEnumerable<uint> values = this.TrianglePascal(order);
            for(int indexU = 0, indexv = Convert.ToInt32(order); indexv >= 0; --indexv, ++indexU) {
                Product term = new Product();
                if (values.ElementAt(indexU) > 1)
                    term.Add(new NumericValue(values.ElementAt(indexU)));
                if (indexU == 1)
                {
                    term.Add(new UnknownTerm(u));
                }
                else if (indexU > 1)
                {
                    term.Add(new Power(new UnknownTerm(u), indexU));
                }
                if (indexv == 1)
                {
                    term.Add(new Coefficient(v));
                }
                else if (indexv > 1)
                {
                    term.Add(new Power(new UnknownTerm(v), indexv));
                }
                sum.Add(term);
            }
            return sum;
        }

        #endregion

    }
}
