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
    /// Implements a polynome as the second order
    /// Coefficients are in munuscule
    /// Terms are in majuscule
    /// </summary>
    public class Polynome2 : PersistentDataObject
    {

        #region Inner Class

        /// <summary>
        /// Solution
        /// </summary>
        public class Solution
        {
            private double x0;
            private double dx;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="x0">x0 input</param>
            /// <param name="dx">dx input</param>
            public Solution(double x0, double dx)
            {
                this.x0 = x0;
                this.dx = dx;
            }

            /// <summary>
            /// Gets or sets X0 value
            /// </summary>
            public double X0 { get { return this.x0; } set { this.x0 = value; } }

            /// <summary>
            /// Gets or sets DX value
            /// </summary>
            public double DX { get { return this.dx; } set { this.dx = value; } }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Index for coefficient b
        /// </summary>
        public static readonly string coefficientBName = "coeffB";

        /// <summary>
        /// Index for coefficient c
        /// </summary>
        public static readonly string coefficientCName = "coeffC";
        
        /// <summary>
        /// Index name for term x0
        /// </summary>
        public static readonly string termX0Name = "termX0";

        /// <summary>
        /// Index name for term y
        /// </summary>
        public static readonly string termYName = "termY";

        private static readonly Dictionary<string, IArithmetic> functions = new Dictionary<string, IArithmetic>() {
            { "Y_0", ("X_0^2 + b*X_0 + c").ToArithmetic() },
            { "Y'", ("2 * X_0 + b").ToArithmetic() },
            { "DY", ("Y - Y_0").ToArithmetic() },
            { "DX", ("X - X_0").ToArithmetic() },
            { "DX_1", ("(DY + (Y' / 2)^2) v 2 - Y'/2").ToArithmetic() },
            { "DX_2", ("-((DY + (Y' / 2)^2) v 2) - Y'/2").ToArithmetic() },
            { "X_1", ("X_0 + DX_1").ToArithmetic() },
            { "X_2", ("X_0 + DX_2").ToArithmetic() },
            { "Complementaire", "-(X_0+b)".ToArithmetic() }

        };

        private static readonly Dictionary<string, List<IArithmetic>> factorisations = new Dictionary<string, List<IArithmetic>>()
        {
            { "Factorisation 1", new List<IArithmetic>() {
                    ("Y = X^2 + b*X + c").ToArithmetic(),
                    ("Y = X*(X+b) + c").ToArithmetic(),
                    ("Y - c = X*(X+b)").ToArithmetic(),
                    ("Y - c = (X+b/2-b/2)*(X+b/2+b/2)").ToArithmetic(),
                    ("Y - c = (X+b/2)^2 - (b/2)^2").ToArithmetic()
                }
            },
            { "Factorisation 2", new List<IArithmetic>() {
                    ("Y - Y_0 = [X^2 + b*X + c] - [X_0^2 + b*X + c]").ToArithmetic(),
                    ("Y - Y_0 = (X^2 - X_0^2) - b*(X - X_0)").ToArithmetic(),
                    ("Y - Y_0 = (X - X_0)*( X + X_0 + b )").ToArithmetic(),
                    ("Y - Y_0 = (X - X_0)*( [X - X_0] + 2*X_0 + b)").ToArithmetic(),
                    ("Y - Y_0 = (X - X_0)*( [X - X_0] + Y'_0)").ToArithmetic()
                }
            }
        };

        #endregion

        #region Properties

        /// <summary>
        /// Gets functions to compute
        /// </summary>
        public Dictionary<string, IArithmetic> Functions
        {
            get { return Polynome2.functions; }
        }

        /// <summary>
        /// Gets formulas
        /// </summary>
        public Dictionary<string, List<IArithmetic>> Formulas
        {
            get { return Polynome2.factorisations; }
        }

        /// <summary>
        /// Gets or sets the B coefficient
        /// </summary>
        public double CoefficientB
        {
            get { return this.Get(coefficientBName, 2.0d); }
            set { this.Set(coefficientBName, value); }
        }

        /// <summary>
        /// Gets or sets the C coefficient
        /// </summary>
        public double CoefficientC
        {
            get { return this.Get(coefficientCName, 1.0d); }
            set { this.Set(coefficientCName, value); }
        }

        /// <summary>
        /// Gets or sets the X0 term
        /// </summary>
        public double TermX0
        {
            get { return this.Get(termX0Name, 0.0d); }
            set { this.Set(termX0Name, value); }
        }

        /// <summary>
        /// Gets or sets the Y term
        /// </summary>
        public double TermY
        {
            get { return this.Get(termYName, 0.0d); }
            set { this.Set(termYName, value); }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Compute Y0 value
        /// </summary>
        /// <param name="x0">X0 value input</param>
        /// <returns>Y0 value output</returns>
        public double ComputeY0(double x0)
        {
            IArithmetic function = Polynome2.functions["Y_0"];
            this.TermX0 = x0;
            function.Let("b", this.CoefficientB);
            function.Let("c", this.CoefficientC);
            function.Let("X_0", this.TermX0);
            return function.Converting().Compute().ToDouble();
        }


        /// <summary>
        /// Differential DY
        /// </summary>
        /// <param name="y">y</param>
        /// <param name="y0">y0</param>
        /// <returns>value</returns>
        public double Differential(double y, double y0)
        {
            IArithmetic function = Polynome2.functions["DY"];
            function.Let("Y", y);
            function.Let("Y_0", y0);
            return function.Converting().ToDouble();
        }

        /// <summary>
        /// Differential DY
        /// </summary>
        /// <param name="y">y</param>
        /// <param name="y0">y0</param>
        /// <param name="b">coefficient b</param>
        /// <param name="c">coefficient c</param>
        /// <returns>value</returns>
        public double Differential(double y, double y0, double b, double c)
        {
            IArithmetic function = Polynome2.functions["DY"];
            this.CoefficientB = b;
            this.CoefficientC = c;
            function.Let("b", this.CoefficientB);
            function.Let("c", this.CoefficientC);
            function.Let("Y", y);
            function.Let("Y_0", y0);
            return function.Converting().ToDouble();
        }

        /// <summary>
        /// Differential DX
        /// </summary>
        /// <param name="dx">dx</param>
        /// <param name="x0">x0</param>
        /// <returns>value</returns>
        public double DifferentialDX(double dx, double x0)
        {
            IArithmetic function = ("DX*[DX+Y']").ToArithmetic();
            IArithmetic yPrime = ("2*X_0 + b").ToArithmetic();
            function.Let("b", this.CoefficientB);
            function.Let("c", this.CoefficientC);
            function.Let("Y'", yPrime);
            function.Let("X_0", x0);
            return function.Converting().ToDouble();
        }

        /// <summary>
        /// Differential DX
        /// </summary>
        /// <param name="dx">dx</param>
        /// <param name="x0">x0</param>
        /// <param name="b">coefficient b</param>
        /// <param name="c">coefficient c</param>
        /// <returns>value</returns>
        public double DifferentialDX(double dx, double x0, double b, double c)
        {
            IArithmetic function = ("DX*[DX+Y']").ToArithmetic();
            IArithmetic yPrime = ("2*X_0 + b").ToArithmetic();
            this.CoefficientB = b;
            this.CoefficientC = c;
            function.Let("b", this.CoefficientB);
            function.Let("c", this.CoefficientC);
            function.Let("Y'", yPrime);
            function.Let("X_0", x0);
            function.Let("DX", dx);
            return function.Converting().ToDouble();
        }

        /// <summary>
        /// Compute Y0 value
        /// </summary>
        /// <param name="x0">X0 value input</param>
        /// <param name="b">b coefficient input</param>
        /// <param name="c">c coefficient input</param>
        /// <returns>Y0 value output</returns>
        public double ComputeY0(double x0, double b, double c)
        {
            IArithmetic function = Polynome2.functions["Y_0"];
            this.TermX0 = x0;
            this.CoefficientB = b;
            this.CoefficientC = c;
            function.Let("b", this.CoefficientB);
            function.Let("c", this.CoefficientC);
            function.Let("X_0", this.TermX0);
            return function.Converting().Compute().ToDouble();
        }

        /// <summary>
        /// Compute x1 output
        /// </summary>
        /// <param name="Y">y input</param>
        /// <param name="x0">x0 input</param>
        /// <returns>x1 output</returns>
        public double ComputeX1(double Y, double x0)
        {
            this.TermY = Y;
            this.TermX0 = x0;
            IArithmetic yPrime = Polynome2.functions["Y'"];
            IArithmetic y0 = Polynome2.functions["Y_0"];
            IArithmetic dy = Polynome2.functions["DY"];
            IArithmetic functionDX = Polynome2.functions["DX_1"];
            IArithmetic x1 = Polynome2.functions["X_1"];
            x1.Let("b", this.CoefficientB);
            x1.Let("c", this.CoefficientC);
            x1.Let("X_0", this.TermX0);
            x1.Let("Y_0", y0.Converting().ToDouble());
            x1.Let("Y", this.TermY);
            x1.Let("Y", Y);
            x1.Let("DY", dy);
            x1.Let("DX_1", functionDX);
            x1.Let("Y'", yPrime.Converting().ToDouble());
            return x1.Converting().Compute().ToDouble();
        }

        /// <summary>
        /// Compute x1 output
        /// </summary>
        /// <param name="Y">y input</param>
        /// <param name="x0">x0 input</param>
        /// <param name="b">b coefficient input</param>
        /// <param name="c">c coefficient input</param>
        /// <returns>x1 output</returns>
        public double ComputeX1(double Y, double x0, double b, double c)
        {
            this.TermY = Y;
            this.TermX0 = x0;
            this.CoefficientB = b;
            this.CoefficientC = c;
            IArithmetic yPrime = Polynome2.functions["Y'"];
            IArithmetic y0 = Polynome2.functions["Y_0"];
            IArithmetic dy = Polynome2.functions["DY"];
            IArithmetic functionDX = Polynome2.functions["DX_1"];
            IArithmetic x1 = Polynome2.functions["X_1"];
            x1.Let("b", this.CoefficientB);
            x1.Let("c", this.CoefficientC);
            x1.Let("X_0", this.TermX0);
            x1.Let("Y_0", y0.Converting().ToDouble());
            x1.Let("Y", this.TermY);
            x1.Let("Y", Y);
            x1.Let("DY", dy);
            x1.Let("DX_1", functionDX);
            x1.Let("Y'", yPrime.Converting().ToDouble());
            return x1.Converting().Compute().ToDouble();
        }

        /// <summary>
        /// Compute x2 output
        /// </summary>
        /// <param name="Y">y input</param>
        /// <param name="x0">x0 input</param>
        /// <returns>x2 output</returns>
        public double ComputeX2(double Y, double x0)
        {
            this.TermY = Y;
            this.TermX0 = x0;
            IArithmetic yPrime = Polynome2.functions["Y'"];
            IArithmetic y0 = Polynome2.functions["Y_0"];
            IArithmetic dy = Polynome2.functions["DY"];
            IArithmetic functionDX = Polynome2.functions["DX_2"];
            IArithmetic x2 = Polynome2.functions["X_2"];
            x2.Let("b", this.CoefficientB);
            x2.Let("c", this.CoefficientC);
            x2.Let("X_0", this.TermX0);
            x2.Let("Y_0", y0.Converting().ToDouble());
            x2.Let("Y", this.TermY);
            x2.Let("Y", Y);
            x2.Let("DY", dy);
            x2.Let("DX_2", functionDX);
            x2.Let("Y'", yPrime.Converting().ToDouble());
            return x2.Converting().Compute().ToDouble();
        }

        /// <summary>
        /// Compute x2 output
        /// </summary>
        /// <param name="Y">y input</param>
        /// <param name="x0">x0 input</param>
        /// <param name="b">b coefficient input</param>
        /// <param name="c">c coefficient input</param>
        /// <returns>x2 output</returns>
        public double ComputeX2(double Y, double x0, double b, double c)
        {
            this.TermY = Y;
            this.TermX0 = x0;
            this.CoefficientB = b;
            this.CoefficientC = c;
            IArithmetic yPrime = Polynome2.functions["Y'"];
            IArithmetic y0 = Polynome2.functions["Y_0"];
            IArithmetic dy = Polynome2.functions["DY"];
            IArithmetic functionDX = Polynome2.functions["DX_2"];
            IArithmetic x2 = Polynome2.functions["X_2"];
            x2.Let("b", this.CoefficientB);
            x2.Let("c", this.CoefficientC);
            x2.Let("X_0", this.TermX0);
            x2.Let("Y_0", y0.Converting().ToDouble());
            x2.Let("Y", this.TermY);
            x2.Let("Y", Y);
            x2.Let("DY", dy);
            x2.Let("DX_2", functionDX);
            x2.Let("Y'", yPrime.Converting().ToDouble());
            return x2.Converting().Compute().ToDouble();
        }

        /// <summary>
        /// Sum
        /// </summary>
        /// <param name="b">b coeff</param>
        /// <param name="xLeft">xLeft</param>
        /// <param name="xRight">xright</param>
        /// <returns>sum</returns>
        public double Sum(double b, double xLeft, double xRight)
        {
            IArithmetic function = ("(2*X0 + b + 1) * (1)").ToArithmetic();
            function.Let("b", b);
            double sum = 0;
            function.Let("Zero", xLeft);
            // si b est pair
            double start;
            if (b % 2 == 0)
                start = ("-(b+1)/2").ToArithmetic().Converting().ToDouble();
            else
                start = ("-b/2").ToArithmetic().Converting().ToDouble();
            for (double x = start; x < xRight; ++x)
            {
                function.Let("X0", x);
                sum = sum + function.Converting().ToDouble();
            }
            return sum;
        }

        /// <summary>
        /// Sum
        /// </summary>
        /// <param name="b">b coeff</param>
        /// <param name="c">coeff c</param>
        /// <param name="x0">y</param>
        /// <returns>sum</returns>
        public double Sum2(double b, double c, double x0)
        {
            IArithmetic diffY, n;
            if (b % 2 == 0) {
                diffY = ("1 + 2*(n+b/2)").ToArithmetic();
            }
            else
            {
                diffY = ("2*(n+1+(b-1)/2)").ToArithmetic();
            }
            diffY.Let("b", b);
            diffY.Let("c", c);
            diffY.Let("n", x0);
            IArithmetic Y = ("DY + c").ToArithmetic();
            Y.Let("DY", diffY.Converting().ToDouble());
            return Y.Converting().ToDouble();
        }

        /// <summary>
        /// Sum
        /// </summary>
        /// <param name="b">b coeff</param>
        /// <param name="c">coeff c</param>
        /// <param name="y">y</param>
        /// <returns>sum</returns>
        public double Sum3(double b, double c, double y)
        {
            IArithmetic diffY, n;
            diffY = ("(Y - c)").ToArithmetic();
            if (b % 2 == 0)
            {
                n = ("(DY - 1)/2-b/2").ToArithmetic();
            }
            else
            {
                n = ("DY/2-1-(b-1)/2").ToArithmetic();
            }
            diffY.Let("Y", y);
            diffY.Let("b", b);
            diffY.Let("c", c);
            n.Let("DY", diffY.Converting().ToDouble());
            return n.Converting().ToDouble();
        }


        /// <summary>
        /// Compare y and the iterative selection value
        /// </summary>
        /// <param name="x0">x0 start</param>
        /// <param name="dx">difference</param>
        /// <param name="y">y to find</param>
        /// <param name="n">digit n</param>
        /// <param name="b">coeff b</param>
        /// <param name="c">coeff c</param>
        /// <returns>1, 0, -1</returns>
        private int CompareY(double x0, double dx, double y, out int n, double b, double c)
        {
            List<Tuple<int, double>> res;
            Tuple<int, double>[] tab = new Tuple<int, double>[10];
            for (int cnt = 0; cnt < 10; ++cnt)
            {
                tab[cnt] = new Tuple<int, double>(cnt, y - this.ComputeY0(x0 + cnt * dx + dx, b, c));
            }
            res = tab.OrderBy(key => key.Item2).ToList();
            n = res.ElementAt(0).Item1;
            if (res.ElementAt(0).Item2 == 0)
            {
                return 0;
            }
            else if (res.ElementAt(0).Item2 > 0)
            {
                return 1;
            }
            else
            {
                foreach (Tuple<int, double> t in res)
                {
                    if (t.Item2 >= 0)
                    {
                        n = t.Item1;
                        return 1;
                    }
                }
                n = res.ElementAt(9).Item1;
                return -1;
            }
        }

        /// <summary>
        /// Search numerical evaluation
        /// </summary>
        /// <param name="y">y to find</param>
        /// <param name="exposant">exp</param>
        /// <param name="dec">decimal position</param>
        /// <param name="x0">x0 starting</param>
        /// <param name="x">x found</param>
        /// <param name="b">coeff b</param>
        /// <param name="c">coeff c</param>
        /// <returns>true if found</returns>
        private bool searchNumericalIntern(double y, int exposant, int dec, double x0, out double x, double b, double c)
        {
            double dx;
            for (int exp = dec; exp > -exposant; --exp)
            {
                dx = Math.Pow(10, exp);
                int n;
                int res = this.CompareY(x0, dx, y, out n, b, c);
                if (res == 0)
                {
                    x = x0 + n * dx + dx;
                    return true;
                }
                else if (res == 1)
                {
                    if (n == 9) // deux chiffres
                        --exp;
                    x0 = x0 + n * dx + dx;
                }
            }
            x = x0;
            return true;
        }

        /// <summary>
        /// Search numerical evaluation
        /// </summary>
        /// <param name="y">y to find</param>
        /// <param name="exposant">exp</param>
        /// <param name="dec">decimal position</param>
        /// <param name="x0">x0 starting</param>
        /// <param name="x">x found</param>
        /// <param name="b">coeff b</param>
        /// <param name="c">coeff c</param>
        /// <returns>true if found</returns>
        private bool searchNumericalInternNegate(double y, int exposant, int dec, double x0, out double x, double b, double c)
        {
            double dx;
            for (int exp = dec; exp > -exposant; --exp)
            {
                dx = -Math.Pow(10, exp);
                int n;
                int res = this.CompareY(x0, dx, y, out n, b, c);
                if (res == 0)
                {
                    x = x0 + n * dx + dx;
                    return true;
                }
                else if (res == 1)
                {
                    if (n == 9) // deux chiffres
                        --exp;
                    x0 = x0 + n * dx + dx;
                }
            }
            x = x0;
            return true;
        }

        /// <summary>
        /// Search numerical evaluation
        /// </summary>
        /// <param name="y">y to find</param>
        /// <param name="exposant">exp</param>
        /// <param name="x">x found</param>
        /// <param name="b">coeff b</param>
        /// <param name="c">coeff c</param>
        /// <returns>true if found</returns>
        public bool searchNumerical(double y, int exposant, out double x, double b, double c)
        {
            double x0, dx;
            this.CoefficientB = b;
            this.CoefficientC = c;
            for (int exp = exposant; exp > -exposant; --exp)
            {
                dx = Math.Pow(10, exp);
                x0 = -this.CoefficientB / 2;
                int n;
                int res = this.CompareY(x0, dx, y, out n, b, c);
                if (res == 0)
                {
                    x = x0 + n * dx + dx;
                    return true;
                }
                else if (res == 1)
                {
                    x0 = x0 + n * dx + dx;
                    return searchNumericalIntern(y, exposant, exp - 1, x0, out x, b, c);
                }
            }

            for (int exp = exposant; exp > -exposant; --exp)
            {
                dx = -Math.Pow(10, exp);
                x0 = -this.CoefficientB / 2;
                int n;
                int res = this.CompareY(x0, dx, y, out n, b, c);
                if (res == 0)
                {
                    x = x0 + n * dx + dx;
                    return true;
                }
                else if (res == 1)
                {
                    x0 = x0 + n * dx + dx;
                    return searchNumericalInternNegate(y, exposant, exp - 1, x0, out x, b, c);
                }
            }
            x = 0;
            return false;
        }


        /// <summary>
        /// Constructs a base from this polynome
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public void ConstructBaseX(double y, out double x, double b, double c) {

            x = 0;
        }


        #endregion


    }
}
