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
            { "X_2", ("X_0 + DX_2").ToArithmetic() }

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
            x1.Let("Y_0", y0);
            x1.Let("Y", this.TermY);
            x1.Let("DY", dy);
            x1.Let("DX", functionDX);
            x1.Let("Y'", yPrime);
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
            x1.Let("Y_0", y0);
            x1.Let("Y", this.TermY);
            x1.Let("DY", dy);
            x1.Let("DX", functionDX);
            x1.Let("Y'", yPrime);
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
            x2.Let("Y_0", y0);
            x2.Let("Y", this.TermY);
            x2.Let("DY", dy);
            x2.Let("DX", functionDX);
            x2.Let("Y'", yPrime);
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
            x2.Let("Y_0", y0);
            x2.Let("Y", this.TermY);
            x2.Let("DY", dy);
            x2.Let("DX", functionDX);
            x2.Let("Y'", yPrime);
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
        /// Calculate numerical difference
        /// </summary>
        /// <param name="y">y input</param>
        /// <param name="x0">x0 input</param>
        /// <param name="dx">size</param>
        /// <returns>numerical difference</returns>
        public int searchNumericalCalc(double y, double x0, double dx)
        {
            IArithmetic f = functions["Y_0"].Clone() as IArithmetic;
            f.Let("X_0", x0);
            f.Let("b", this.CoefficientB);
            f.Let("c", this.CoefficientC);
            double y0 = f.Converting().ToDouble();
            IArithmetic diffY = functions["DY"].Clone() as IArithmetic;
            diffY.Let("Y", y);
            diffY.Let("Y_0", y0);
            double dy = diffY.Converting().ToDouble();
            IArithmetic fdx = ("dx*[dx + Y']").ToArithmetic();
            fdx.Let("dx", dx);
            fdx.Let("Y'", functions["Y'"].Converting().ToDouble());
            double dyCalc = fdx.Converting().Converting().ToDouble();
            if (dyCalc == dy)
            {
                return 0;
            }
            else if (dyCalc < dy)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Search a numerical value for
        /// finding root or one of x values for a given y value
        /// </summary>
        /// <param name="y">y value</param>
        /// <param name="dec">number position</param>
        /// <param name="x0">initial value</param>
        /// <param name="x">x value</param>
        /// <returns>true if found</returns>
        public bool searchNumericalIntegerDigits(double y, double dec, double x0, out double x)
        {
            double turn = 9;
            double x0Step = 9 * Math.Pow(10, dec);
            while (turn > 0)
            {
                int c = searchNumericalCalc(y, x0Step, -Math.Pow(10, dec));
                if (c == 0)
                {
                    x = x0 + x0Step - Math.Pow(10, dec);
                    return true;
                }
                else if (c == -1)
                {
                    if (dec > 0)
                    {
                        if (searchNumericalIntegerDigits(y, dec - 1, x0 + x0Step, out x))
                        {
                            x = x - Math.Pow(10, dec);
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                    {
                        x = x0 + x0Step - Math.Pow(10, dec);
                        return true;
                    }
                }
                else
                {
                    x0Step -= Math.Pow(10, dec);
                }
                --turn;
            }
            x = 0;
            return false;
        }
        /// <summary>
        /// Search a numerical value for
        /// finding root or one of x values for a given y value
        /// </summary>
        /// <param name="y">y value</param>
        /// <param name="exposant">exposant</param>
        /// <param name="x0">initial value</param>
        /// <param name="x">x value</param>
        /// <returns>true if found</returns>
        public bool searchNumericalInteger(double y, double exposant, ref double x0, out double x)
        {
            double turn = exposant;
            double x0Step = x0;
            while (turn > 0)
            {
                int c = searchNumericalCalc(y, x0Step, -10);
                if (c == 0)
                {
                    x0 = x0Step;
                    x = x0Step - 10;
                    return true;
                }
                else if (c == -1)
                {
                    x0 = x0Step;
                    if (searchNumericalIntegerDigits(y, turn, x0, out x))
                    {
                        x = x - 10;
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    x0Step /= 10;
                }
                --turn;
            }
            x = 0;
            return false;
        }

        /// <summary>
        /// Search a numerical value for
        /// finding root or one of x values for a given y value
        /// </summary>
        /// <param name="y">y value</param>
        /// <param name="dec">decimal position</param>
        /// <param name="exposant">maximal position</param>
        /// <param name="x0">integer value</param>
        /// <param name="x">x value</param>
        /// <returns>true if found</returns>
        public bool searchNumericalDecimal(double y, double dec, double exposant, double x0, ref double x)
        {
            double turn = 10;
            double x0Step = 0;
            while (turn > 0)
            {
                int c = searchNumericalCalc(y, x0 + x0Step, Math.Pow(10, -dec));
                if (c == 0)
                {
                    x = x + x0Step;
                    return true;
                }
                else if (c == 1)
                {
                    if (dec < exposant)
                    {
                        if (searchNumericalDecimal(y, dec + 1, exposant, x0 + x0Step, ref x))
                        {
                            x = x + x0Step;
                            return true;
                        }
                        else
                            return true;
                    }
                    else
                        return true;
                }
                else
                {
                    x0Step += Math.Pow(10, -dec);
                }
                --turn;
            }
            if (dec < exposant)
            {
                if (searchNumericalDecimal(y, dec + 1, exposant, x0 + x0Step, ref x))
                {
                    return true;
                }
                else
                    return true;
            }
            else
                return true;
        }

        /// <summary>
        /// Search a numerical value for
        /// finding root or one of x values for a given y value
        /// </summary>
        /// <param name="y">y value</param>
        /// <param name="x">x value</param>
        /// <returns>true if found</returns>
        public bool searchNumericaDigit(double y, out double x)
        {
            double turn = 9;
            double x0Step = turn;
            while (turn > 0)
            {
                int c = searchNumericalCalc(y, x0Step, -1);
                if (c == 0)
                {
                    x = x0Step;
                    return true;
                }
                else if (c == 1)
                {
                    x = turn + 1;
                    return true;
                }
                else
                {
                    x0Step -= 1;
                }
                --turn;
            }
            x = 0;
            return false;
        }

        /// <summary>
        /// Search a numerical value for
        /// finding root or one of x values for a given y value
        /// </summary>
        /// <param name="b">b coeff</param>
        /// <param name="c">c coeff</param>
        /// <param name="y">y value</param>
        /// <param name="exposant">number size</param>
        /// <param name="x">x value found</param>
        /// <returns>true if found</returns>
        public bool searchNumerical(double b, double c, double y, double exposant, out double x)
        {
            this.CoefficientB = b;
            this.CoefficientC = c;
            double x0 = Math.Pow(10, exposant);
            double exp = exposant;
            while (x0 > 0)
            {
                if (searchNumericalInteger(y, exposant, ref x0, out x))
                {
                    goto deci;
                }
                else
                {
                    x0 /= 10;
                }
            }
            x0 = -1;
            while (exp > 0)
            {
                if (searchNumericalInteger(y, exposant, ref x0, out x))
                {
                    goto deci;
                }
                else
                {
                    x0 /= 10;
                    --exp;
                }
            }
            x = 0;
            return false;
        deci:
            {
                double x0Step = 0;
                x0 = x;
                exp = exposant;
                while (exp > 0)
                {
                    if (searchNumericalDecimal(y, 1, exposant, x0 + x0Step, ref x))
                    {
                        x = x + x0Step;
                        return true;
                    }
                    else
                    {
                        x0Step /= 10;
                        --exp;
                    }
                }
                x = 0;
                return false;
            }
        }

        /// <summary>
        /// Computes all positive exposant couple of numbers intervals
        /// </summary>
        /// <param name="exposant">size</param>
        /// <returns>list of couples</returns>
        public IEnumerable<Tuple<double, double>> Couples(double exposant)
        {
            for (double d = exposant; d > 0; --d)
            {
                yield return new Tuple<double, double>(Math.Pow(10, d), Math.Pow(10, d - 1) + 1);
            }
        }

        /// <summary>
        /// Gets digits
        /// </summary>
        public IEnumerable<Tuple<double, double>> Digits
        {
            get
            {
                for (double d = 9; d > 0; --d)
                {
                    yield return new Tuple<double, double>(d, d - 1);
                }
            }
        }

        /// <summary>
        /// Transforms digits with exposant from 0 (none) to n (x * 10^n)
        /// </summary>
        /// <param name="exposant">exposant</param>
        /// <returns>tuple list</returns>
        public IEnumerable<Tuple<double, double>> TransformDigits(double exposant)
        {
            foreach (Tuple<double, double> t in this.Digits)
            {
                yield return new Tuple<double, double>(t.Item1 * Math.Pow(10, exposant), t.Item2 * Math.Pow(10, exposant));
            }
        }

        /// <summary>
        /// Transforms digits with exposant from 0 (none) to n (x * 10^n)
        /// </summary>
        /// <param name="exposant">exposant</param>
        /// <param name="addition">addition to add</param>
        /// <returns>tuple list</returns>
        public IEnumerable<Tuple<double, double>> TransformDigits(double exposant, double addition)
        {
            foreach (Tuple<double, double> t in this.Digits)
            {
                yield return new Tuple<double, double>(t.Item1 * Math.Pow(10, exposant) + addition, t.Item2 * Math.Pow(10, exposant) + addition);
            }
        }


        #endregion


    }
}
