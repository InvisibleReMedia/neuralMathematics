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

        #endregion

    }
}
