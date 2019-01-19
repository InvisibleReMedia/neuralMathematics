using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistantModel;

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

        private static readonly Dictionary<string, Arithmetic> functions = new Dictionary<string, Arithmetic>() {
            { "Y0", new Sum(new UnknownTerm("X0") ^ 2.0, new Coefficient("b") * new UnknownTerm("X0"), new Coefficient("c")) },
            { "Y'", new Sum(new UnknownTerm("X0") * 2.0d, new Coefficient("b")) },
            { "DY", new Soustraction(new UnknownTerm("Y"), new UnknownTerm("Y0")) },
            { "DX", new Soustraction(new UnknownTerm("X"), new UnknownTerm("X0")) },
            { "DX1", new Soustraction(
                                        new Root(new Sum(new UnknownTerm("DY"),
                                                         new Power(new Division(new UnknownTerm("Y'"), (2.0d).ToArithmetic()), (2.0d).ToArithmetic())),
                                                 (2.0d).ToArithmetic()),
                                        new UnknownTerm("Y'") / 2.0d) },
            { "DX2", new Soustraction(
                                        new Negative(new Root(new Sum(new UnknownTerm("DY"),
                                                         new Power(new Division(new UnknownTerm("Y'"), (2.0d).ToArithmetic()), (2.0d).ToArithmetic())),
                                                 (2.0d).ToArithmetic())),
                                        new UnknownTerm("Y'") / 2.0d) },
            { "X1", new Addition(new UnknownTerm("X0"), new UnknownTerm("DX1")) },
            { "X2", new Addition(new UnknownTerm("X0"), new UnknownTerm("DX2")) }

        };

        private static readonly Dictionary<string, List<Arithmetic>> factorisations = new Dictionary<string, List<Arithmetic>>()
        {
            { "Factorisation 1", new List<Arithmetic>() {
                    new Equal(new UnknownTerm("y"), new Sum(new UnknownTerm("x") ^ 2.0, new Coefficient("b") * new UnknownTerm("x"), new Coefficient("c"))),
                    new Equal(new UnknownTerm("y"), new Sum(new Product(new UnknownTerm("x"), new Sum(new UnknownTerm("x"), new Coefficient("b"))), new Coefficient("c"))),
                    new Equal(new Soustraction(new UnknownTerm("y"), new Coefficient("c")), new Product(new UnknownTerm("x"), new Sum(new UnknownTerm("x"), new Coefficient("b")))),
                    new Equal(new Soustraction(new UnknownTerm("y"), new Coefficient("c")), new Product(
                                                        new Sum(new UnknownTerm("x"), new Coefficient("b") / 2.0, new Negative(new Coefficient("b") / 2.0)),
                                                        new Sum(new UnknownTerm("x"), new Coefficient("b") / 2.0, new Coefficient("b") / 2.0))),
                    new Equal(new Soustraction(new UnknownTerm("y"), new Coefficient("c")),
                                            new Soustraction(new Power(new Addition(new UnknownTerm("x"), new Coefficient("b") / 2.0), 2.0d),
                                                             new Power(new Coefficient("b") / 2.0, 2.0d)))

                }
            }
        };

        #endregion

        #region Properties

        /// <summary>
        /// Gets functions to compute
        /// </summary>
        public Dictionary<string, Arithmetic> Functions
        {
            get { return Polynome2.functions; }
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
            Arithmetic function = Polynome2.functions["Y0"];
            this.TermX0 = x0;
            function.Let("b", this.CoefficientB);
            function.Let("c", this.CoefficientC);
            function.Let("x0", this.TermX0);
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
            Arithmetic function = Polynome2.functions["Y0"];
            this.TermX0 = x0;
            this.CoefficientB = b;
            this.CoefficientC = c;
            function.Let("b", this.CoefficientB);
            function.Let("c", this.CoefficientC);
            function.Let("x0", this.TermX0);
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
            Arithmetic yPrime = Polynome2.functions["Y'"];
            Arithmetic y0 = Polynome2.functions["Y0"];
            Arithmetic dy = Polynome2.functions["DY"];
            Arithmetic functionDX = Polynome2.functions["DX1"];
            Arithmetic x1 = Polynome2.functions["X1"];
            x1.Let("b", this.CoefficientB);
            x1.Let("c", this.CoefficientC);
            x1.Let("x0", this.TermX0);
            x1.Let("y0", y0);
            x1.Let("y", this.TermY);
            x1.Let("dy", dy);
            x1.Let("dx", functionDX);
            x1.Let("y'", yPrime);
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
            Arithmetic yPrime = Polynome2.functions["Y'"];
            Arithmetic y0 = Polynome2.functions["Y0"];
            Arithmetic dy = Polynome2.functions["DY"];
            Arithmetic functionDX = Polynome2.functions["DX1"];
            Arithmetic x1 = Polynome2.functions["X1"];
            x1.Let("b", this.CoefficientB);
            x1.Let("c", this.CoefficientC);
            x1.Let("x0", this.TermX0);
            x1.Let("y0", y0);
            x1.Let("y", this.TermY);
            x1.Let("dy", dy);
            x1.Let("dx", functionDX);
            x1.Let("y'", yPrime);
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
            Arithmetic yPrime = Polynome2.functions["Y'"];
            Arithmetic y0 = Polynome2.functions["Y0"];
            Arithmetic dy = Polynome2.functions["DY"];
            Arithmetic functionDX = Polynome2.functions["DX2"];
            Arithmetic x2 = Polynome2.functions["X2"];
            x2.Let("b", this.CoefficientB);
            x2.Let("c", this.CoefficientC);
            x2.Let("x0", this.TermX0);
            x2.Let("y0", y0);
            x2.Let("y", this.TermY);
            x2.Let("dy", dy);
            x2.Let("dx", functionDX);
            x2.Let("y'", yPrime);
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
            Arithmetic yPrime = Polynome2.functions["Y'"];
            Arithmetic y0 = Polynome2.functions["Y0"];
            Arithmetic dy = Polynome2.functions["DY"];
            Arithmetic functionDX = Polynome2.functions["DX2"];
            Arithmetic x2 = Polynome2.functions["X2"];
            x2.Let("b", this.CoefficientB);
            x2.Let("c", this.CoefficientC);
            x2.Let("x0", this.TermX0);
            x2.Let("y0", y0);
            x2.Let("y", this.TermY);
            x2.Let("dy", dy);
            x2.Let("dx", functionDX);
            x2.Let("y'", yPrime);
            return x2.Converting().Compute().ToDouble();
        }

        #endregion

    }
}
