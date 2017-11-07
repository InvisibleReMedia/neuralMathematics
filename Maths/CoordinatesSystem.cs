using PersistantModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths
{

    /// <summary>
    /// Coordonnées d'un système de dimension n
    /// </summary>
    public class Coordinates
    {
        #region Fields

        /// <summary>
        /// dimension
        /// </summary>
        private uint dim;

        /// <summary>
        /// Sous-dimension
        /// </summary>
        private Coordinates position;

        /// <summary>
        /// Value in one dimension
        /// </summary>
        private double value;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for a coordinates
        /// given its dimension
        /// </summary>
        /// <param name="dimension">dimension</param>
        public Coordinates(uint dimension)
        {
            if (dimension > 1)
            {
                this.dim = dimension;
                this.value = 0.0d;
                this.position = new Coordinates(dimension - 1);
            }
            else
            {
                this.dim = dimension;
                this.value = 0.0d;
                this.position = null;
            }
        }

        /// <summary>
        /// Constructor with each value at each dimension
        /// </summary>
        /// <param name="positions">values</param>
        public Coordinates(IEnumerable<double> positions)
        {
            if (positions.Count() > 1)
            {
                this.dim = Convert.ToUInt32(positions.Count());
                this.value = positions.ElementAt(0);
                this.position = new Coordinates(positions.Skip(1));
            }
            else
            {
                this.dim = 1;
                this.value = positions.ElementAt(0);
                this.position = null;
            }
        }

        /// <summary>
        /// Constructor with each value at each dimension
        /// </summary>
        /// <param name="positions">values</param>
        public Coordinates(params double[] positions) : this(positions.AsEnumerable())
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the euclidian sub-coordinates
        /// </summary>
        public Coordinates Euclidian
        {
            get
            {
                return this.position;
            }
        }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Gets the dimension number
        /// </summary>
        public uint Dimension
        {
            get
            {
                return this.dim;
            }
        }

        #endregion
    }

    /// <summary>
    /// Vecteur de coordonnées
    /// </summary>
    public class Vector
    {

        #region Fields

        /// <summary>
        /// Vecteur de coordonnées
        /// </summary>
        private Coordinates start, end;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for a vector
        /// given a dimension
        /// </summary>
        /// <param name="dimension">dimension</param>
        public Vector(uint dimension)
        {
            this.start = new Coordinates(dimension);
            this.end = new Coordinates(dimension);
        }

        /// <summary>
        /// Constructor of a vector
        /// with two coordinates
        /// at same dimension
        /// </summary>
        /// <param name="a">coordinate one</param>
        /// <param name="b">coordinate two</param>
        public Vector(Coordinates a, Coordinates b)
        {
            this.start = a;
            this.end = b;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the euclidian sub-vector
        /// </summary>
        public Vector Euclidian
        {
            get
            {
                return new Vector(this.start.Euclidian, this.end.Euclidian);
            }
        }

        /// <summary>
        /// Gets the start of coordinate
        /// </summary>
        public Coordinates From
        {
            get
            {
                return this.start;
            }
        }

        /// <summary>
        /// Gets the end of coordinate
        /// </summary>
        public Coordinates To
        {
            get
            {
                return this.end;
            }
        }

        #endregion

    }

    /// <summary>
    /// Système de coordonnées
    /// </summary>
    public class CoordinateSystem
    {

        #region Fields

        /// <summary>
        /// Dimension du système
        /// </summary>
        private uint dim;

        /// <summary>
        /// Coordonnées du système
        /// </summary>
        private List<Coordinates> coords;

        /// <summary>
        /// moving values
        /// </summary>
        private MovingCoordinates move;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for a coordinate system
        /// </summary>
        /// <param name="dimension">dimension</param>
        public CoordinateSystem(uint dimension)
        {
            this.dim = dimension;
            this.coords = new List<Coordinates>();
        }

        /// <summary>
        /// Coordinate system with a limit and a step
        /// </summary>
        /// <param name="m">move</param>
        public CoordinateSystem(MovingCoordinates m)
        {
            this.move = m;
            this.dim = this.move.Steps.Dimension;
            this.coords = new List<Coordinates>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dimension of this system
        /// </summary>
        public uint Dimension
        {
            get
            {
                return this.dim;
            }
        }

        /// <summary>
        /// Gets all coordinates
        /// </summary>
        public IEnumerable<Coordinates> Coordinates
        {
            get
            {
                return this.coords;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a new coordinates into this system
        /// </summary>
        /// <param name="coords">coordinate</param>
        public void Add(params double[] coords)
        {
            Coordinates c = new Coordinates(coords);
            c = this.move.Floor(c);
            this.coords.Add(c);
        }

        /// <summary>
        /// Add a new coordinates into this system
        /// </summary>
        /// <param name="coords">coordinate</param>
        public void Add(IEnumerable<double> coords)
        {
            Coordinates c = new Coordinates(coords);
            c = this.move.Floor(c);
            this.coords.Add(c);
        }

        /// <summary>
        /// Add an existing coordinates into this system
        /// </summary>
        /// <param name="c">coordinate</param>
        public void Add(Coordinates c)
        {
            this.coords.Add(c);
        }


        /// <summary>
        /// Add a new coordinate into this system
        /// </summary>
        /// <param name="f">function</param>
        /// <param name="nameTerms">terms identified by name and ordered as same to coords</param>
        /// <param name="coords">coordinate</param>
        public void Add(Arithmetic f, IEnumerable<string> nameTerms, params double[] coords)
        {
            int index = 0;
            foreach (string u in nameTerms)
            {
                if (index < coords.Count())
                {
                    f.Let(u, coords.ElementAt(index));
                    ++index;
                }
                else
                {
                    break;
                }
            }
            string res = f.Calculate(true);
            if (f.IsCalculable)
            {
                List<double> c = new List<double>(coords);
                c.Add(Convert.ToDouble(res));
                Coordinates src = new Coordinates(c);
                src = this.move.Floor(src);
                this.Add(src);
            }
            else
            {
                // un ou plusieurs termes n'ont pas de valeur
                throw new KeyNotFoundException();
            }
        }

        #endregion

    }

    /// <summary>
    /// A movement
    /// </summary>
    public class MovingCoordinates
    {

        #region Fields

        /// <summary>
        /// Limits
        /// </summary>
        private Vector limit;

        /// <summary>
        /// Moving steps
        /// </summary>
        private Coordinates steps;

        #endregion

        #region Constructors

        /// <summary>
        /// Moving element
        /// </summary>
        /// <param name="v">vector limit</param>
        /// <param name="s">steps</param>
        public MovingCoordinates(Vector v, Coordinates s)
        {
            this.limit = v;
            this.steps = s;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the vector limit
        /// </summary>
        public Vector Vector
        {
            get
            {
                return this.limit;
            }
        }

        /// <summary>
        /// Gets the steps for each dimension
        /// </summary>
        public Coordinates Steps
        {
            get
            {
                return this.steps;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates move steps
        /// </summary>
        /// <returns>Moving Vector</returns>
        public MovingVector GenerateMove()
        {
            return new MovingVector(this);
        }

        /// <summary>
        /// Computes floor values
        /// </summary>
        /// <param name="c">coordinate</param>
        /// <returns>floor coordinate</returns>
        public Coordinates Floor(Coordinates c)
        {
            Coordinates src = c;
            Coordinates dest = new Coordinates(c.Dimension);
            Coordinates current = dest;
            Vector v = this.limit;
            Coordinates currentStep = this.steps;
            while (src != null)
            {
                if (src.Value >= v.From.Value && src.Value <= v.To.Value)
                {
                    current.Value = v.From.Value + Math.Ceiling((src.Value - v.From.Value) / currentStep.Value) * currentStep.Value;
                    current = current.Euclidian;
                    src = src.Euclidian;
                    currentStep = currentStep.Euclidian;
                    v = v.Euclidian;
                }
                else
                    throw new IndexOutOfRangeException();
            }
            return dest;
        }

        #endregion

    }

    /// <summary>
    /// Vecteur en mouvement
    /// </summary>
    public class MovingVector
    {

        #region Fields

        /// <summary>
        /// Dimension
        /// </summary>
        private uint dim;

        /// <summary>
        /// Sous-dimension
        /// </summary>
        private MovingVector position;

        /// <summary>
        /// List of values for one dimension
        /// </summary>
        private List<double> values;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for a coordinates
        /// given its dimension
        /// </summary>
        /// <param name="dimension">dimension</param>
        public MovingVector(uint dimension)
        {
            if (dimension > 1)
            {
                this.dim = dimension;
                this.values = new List<double>();
                this.position = new MovingVector(dimension - 1);
            }
            else
            {
                this.dim = dimension;
                this.values = new List<double>();
                this.position = null;
            }
        }

        /// <summary>
        /// Constructor with each value at each dimension
        /// </summary>
        /// <param name="positions">values</param>
        public MovingVector(params double[] positions)
        {
            if (positions.Count() > 1)
            {
                this.dim = Convert.ToUInt32(positions.Count());
                this.values = new List<double>();
                this.values.Add(positions[0]);
                this.position = new MovingVector(positions.Skip(1));
            }
            else
            {
                this.dim = 1;
                this.values = new List<double>();
                this.values.Add(positions[0]);
                this.position = null;
            }
        }

        /// <summary>
        /// Constructor with each value at each dimension
        /// </summary>
        /// <param name="positions">values</param>
        public MovingVector(IEnumerable<double> positions)
        {
            if (positions.Count() > 1)
            {
                this.dim = Convert.ToUInt32(positions.Count());
                this.values = new List<double>();
                this.values.Add(positions.ElementAt(0));
                this.position = new MovingVector(positions.Skip(1));
            }
            else
            {
                this.dim = 1;
                this.values = new List<double>();
                this.values.Add(positions.ElementAt(0));
                this.position = null;
            }
        }

        /// <summary>
        /// Constructor for a move with multi-dimensional array
        /// </summary>
        /// <param name="move">move</param>
        public MovingVector(MovingCoordinates move)
        {
            if (move.Steps.Dimension > 1)
            {
                this.dim = Convert.ToUInt32(move.Steps.Dimension);
                this.values = new List<double>();
                double start = move.Vector.From.Value;
                double end = move.Vector.To.Value;
                double step = move.Steps.Value;
                for (double v = start; v <= end; v += step)
                {
                    this.values.Add(v);
                }
                MovingCoordinates nextMove = new MovingCoordinates(move.Vector.Euclidian, move.Steps.Euclidian);
                this.position = new MovingVector(nextMove);
            }
            else
            {
                this.dim = 1;
                this.values = new List<double>();
                double start = move.Vector.From.Value;
                double end = move.Vector.To.Value;
                double step = move.Steps.Value;
                for (double v = start; v <= end; v += step)
                {
                    this.values.Add(v);
                }
                this.position = null;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dimension
        /// </summary>
        public uint Dimension
        {
            get
            {
                return this.dim;
            }
        }

        /// <summary>
        /// List of each values as the movement
        /// </summary>
        public IEnumerable<double> Values
        {
            get
            {
                return this.values;
            }
        }

        /// <summary>
        /// Gets the euclidian sub-vector
        /// </summary>
        public MovingVector Euclidian
        {
            get
            {
                return this.position;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add just one value on the first dimension
        /// </summary>
        /// <param name="value">value</param>
        public void Add(double value)
        {
            this.values.Add(value);
        }

        /// <summary>
        /// Add multi-values of moving at each dimension
        /// </summary>
        /// <param name="positions">position list</param>
        public void Add(params double[] positions)
        {
            if (positions.Count() > 1)
            {
                this.values.Add(positions[0]);
                this.position.Add(positions.Skip(1));
            }
            else
            {
                this.values.Add(positions[0]);
            }
        }

        /// <summary>
        /// Add multi-values of moving at each dimension
        /// </summary>
        /// <param name="positions">position list</param>
        public void Add(IEnumerable<double> positions)
        {
            if (positions.Count() > 1)
            {
                this.values.Add(positions.ElementAt(0));
                this.position.Add(positions.Skip(1));
            }
            else
            {
                this.dim = 1;
                this.values.Add(positions.ElementAt(0));
            }
        }

        #endregion
    }

    /// <summary>
    /// A line is a 1D graphical representaion of a moving vector
    /// at least one dimension
    /// this is a projection of a n-vector by one dimension
    /// A projection function must be used to compute each acurate speed values
    /// </summary>
    public class Line
    {

        #region Fields

        /// <summary>
        /// A moving n-vector
        /// </summary>
        private MovingVector move;

        /// <summary>
        /// Projection function
        /// </summary>
        private Arithmetic function;

        /// <summary>
        /// Coordonnées en mouvement à 1 dimension
        /// </summary>
        private MovingVector result;

        /// <summary>
        /// Each term of the arithmetic function
        /// is relative to a value at each coordinates
        /// the dimension is exactly equal to the number of terms
        /// 
        /// The arithmetic function can use all terms or less
        /// If the arithmetic function is not calculable after all given value for each term
        /// then it will throw an exception
        /// </summary>
        private List<UnknownTerm> terms;

        /// <summary>
        /// all terms affected with their names
        /// </summary>
        private List<string> nameTerms;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with a moving vector
        /// and an arithmetic function
        /// </summary>
        /// <param name="m">move</param>
        /// <param name="f">function</param>
        public Line(MovingVector m, Arithmetic f)
        {
            this.move = m;
            this.function = f;
            this.terms = f.UnknownTerms.Values.Cast<UnknownTerm>().ToList();
            this.nameTerms = new List<string>(Convert.ToInt32(this.move.Dimension));
            this.result = new MovingVector(1);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Iteration get each values for each term
        /// use the first term to reach it by the computation of the function
        /// </summary>
        /// <param name="indexes">list of indexes position</param>
        /// <param name="depth">depth value</param>
        /// <param name="m">vector</param>
        /// <returns>values</returns>
        protected IEnumerable<double> Iteration(List<uint> indexes, uint depth, MovingVector m)
        {
            int iDepth = Convert.ToInt32(depth);
            if (m.Dimension > 1)
            {
                List<double> res = this.Iteration(indexes, depth + 1, m.Euclidian).ToList();
                if (indexes[iDepth] < m.Values.Count())
                {
                    double val = m.Values.ElementAt(Convert.ToInt32(indexes[iDepth]));
                    res.Add(val);
                }
                else
                {
                    for (int d = iDepth; d < indexes.Count; ++d)
                    {
                        indexes[d] = 0;
                    }
                    // incremente le précédent index
                    ++indexes[iDepth - 1];
                }
                return res;
            }
            else
            {
                List<double> res = new List<double>();
                if (indexes[iDepth] < m.Values.Count())
                {
                    double val = m.Values.ElementAt(Convert.ToInt32(indexes[iDepth]));
                    res.Add(val);
                }
                else
                {
                    if (iDepth == 0)
                    {
                        // fin du processus d'iteration car fin du premier index atteint
                        throw new IndexOutOfRangeException();
                    }
                    else
                    {
                        // recommence le processus au coup suivant
                        indexes[iDepth] = 0;
                        // incrémente le précédent index
                        ++indexes[iDepth - 1];
                    }
                }
                return res;
            }
        }

        /// <summary>
        /// Iterate all elements and play each values
        /// </summary>
        protected void Iteration()
        {
            this.result = new MovingVector(1);
            List<uint> indexes = new List<uint>(Convert.ToInt32(this.move.Dimension));
            try
            {
                while (true)
                {
                    IEnumerable<double> values = this.Iteration(indexes, 0, this.move);
                    // la première valeur est l'index de recherche
                    double y = values.ElementAt(0);
                    // les autres valeurs sont calculées avec l'équation
                    double z = this.Calculate(values.Skip(1));
                    if (z < y)
                    {
                        // continue to play
                        this.result.Add(z);
                    }
                    else
                    {
                        this.result.Add(y);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        /// <summary>
        /// Calculate function value
        /// given each values with each name of terms
        /// to evaluate
        /// </summary>
        /// <param name="values">values to set each terms</param>
        /// <returns>a double value as the calculation result</returns>
        protected double Calculate(IEnumerable<double> values)
        {
            int index = 0;
            foreach (string u in this.nameTerms)
            {
                if (index < values.Count())
                {
                    this.function.Let(u, values.ElementAt(index));
                    ++index;
                }
                else
                {
                    break;
                }
            }
            string res = this.function.Calculate(true);
            if (this.function.IsCalculable)
            {
                return Convert.ToDouble(res);
            }
            else
            {
                // un ou plusieurs termes n'ont pas de valeur
                throw new KeyNotFoundException();
            }
        }

        #endregion

    }

    /// <summary>
    /// A plan is a 2D graphical representation of a moving vector
    /// at least 2 or more dimension
    /// </summary>
    public class Plan
    {

    }

}
