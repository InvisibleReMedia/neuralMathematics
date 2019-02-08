using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// A base contains a vector space
    /// with its ker and its dim
    /// </summary>
    public class Base : PersistantModel.PersistentDataObject, ICloneable
    {
        #region Fields

        /// <summary>
        /// base number
        /// </summary>
        public static readonly string baseName = "base";

        /// <summary>
        /// Dimension
        /// </summary>
        public static readonly string dimName = "dim";

        /// <summary>
        /// Vector space
        /// </summary>
        public static readonly string vectorSpaceName = "vector";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a base with ker and dimension
        /// left vector at zero values
        /// </summary>
        /// <param name="b">ker</param>
        /// <param name="d">dimension</param>
        public Base(int b, int d)
        {
            this.Set(Base.baseName, b);
            this.Set(Base.dimName, d);
            this.Set(vectorSpaceName, new double[d]);
        }

        /// <summary>
        /// Constructs a base with ker
        /// and a vector space
        /// </summary>
        /// <param name="b">ker</param>
        /// <param name="vector">vector space</param>
        public Base(int b, IEnumerable<double> vector)
        {
            this.Set(Base.baseName, b);
            this.Set(Base.dimName, vector.Count());
            this.Set(vectorSpaceName, vector.ToArray());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the vector space
        /// </summary>
        public IEnumerable<double> Vector
        {
            get
            {
                return this.Get(vectorSpaceName);
            }
        }

        /// <summary>
        /// Gets the coefficient list
        /// </summary>
        public IEnumerable<string> Coefficients
        {
            get
            {
                UniqueStrings u = new UniqueStrings();
                for (int index = 0; index < this.Dimension; ++index)
                {
                    yield return u.ComputeNewString();
                }
            }
        }

        /// <summary>
        /// Gets the ker
        /// </summary>
        public int Ker
        {
            get { return this.Get(baseName); }
        }

        /// <summary>
        /// Gets the dimension
        /// </summary>
        public int Dimension
        {
            get
            {
                return this.Get(dimName);
            }
        }

        /// <summary>
        /// Gets or sets the indiced vector space
        /// </summary>
        /// <param name="p">size of dim</param>
        /// <returns>double value</returns>
        public double this[int p]
        {
            get
            {
                double[] vector = this.Get(vectorSpaceName);
                return vector[p];
            }
            set
            {
                double[] vector = this.Get(vectorSpaceName);
                vector[p] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets indices for base
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>coefficient list</returns>
        public IEnumerable<Coefficient> Indices(string name)
        {
            for (int index = 0; index < this.Dimension; ++index)
            {
                yield return new Coefficient(name + "_" + (this.Dimension - 1 - index).ToString());
            }
        }

        /// <summary>
        /// Converts a value to a vector space
        /// regarding the ker value
        /// </summary>
        /// <param name="d">double value input</param>
        public void SetValue(double d)
        {
            this.Set(vectorSpaceName, Base.ConvertToReadable(d, this.Ker).ToArray());
        }

        /// <summary>
        /// Converts the vector space to a double
        /// </summary>
        /// <returns>double value</returns>
        public double ToDouble()
        {
            return Base.ConvertToNumber(this.Vector, this.Ker);
        }

        /// <summary>
        /// Converts this Base to an another
        /// </summary>
        /// <param name="b">another base</param>
        /// <param name="d">dim</param>
        /// <returns>new base</returns>
        public Base ToBase(int b, int d)
        {
            if (this.Ker == b)
            {
                return this.Clone() as Base;
            }
            else
            {
                double s = this.ToDouble();
                return new Base(b, Base.ConvertToReadable(s, b));
            }
        }

        /// <summary>
        /// Transforms a number into a vector space
        /// </summary>
        /// <param name="d">double value</param>
        /// <param name="radix">radix</param>
        /// <returns>vector space</returns>
        public static IEnumerable<double> ConvertToReadable(double d, double radix)
        {
            double iter, left, quotient;
            iter = d;
            do
            {
                quotient = Math.Floor(iter / radix);
                left = iter - quotient * radix;
                yield return left;
                iter = quotient;
            } while (iter >= 1);
        }

        /// <summary>
        /// Converts a vector space to a double value
        /// </summary>
        /// <param name="list">vector space</param>
        /// <param name="radix">radix</param>
        /// <returns>double value</returns>
        public static double ConvertToNumber(IEnumerable<double> list, double radix)
        {
            double res = 0;
            double rac = 1;
            for (int index = list.Count() - 1; index >= 0; --index)
            {
                res += rac * list.ElementAt(index);
                rac *= radix;
            }
            return res;
        }

        /// <summary>
        /// Clone this object
        /// </summary>
        /// <returns>new object</returns>
        public object Clone()
        {
            Base b = new Base(this.Ker, this.Dimension);
            this.Set(vectorSpaceName, this.Vector.ToArray());
            return b;
        }

        #endregion

    }
}
