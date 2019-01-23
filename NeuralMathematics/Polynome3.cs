using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using PersistantModel;

namespace NeuralMathematics
{
    /// <summary>
    /// Represents a polynome at the third order
    /// </summary>
    public class Polynome3 : PersistentDataObject
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
        /// Index for coefficient d
        /// </summary>
        public static readonly string coefficientDName = "coeffD";

        /// <summary>
        /// Index name for term x0
        /// </summary>
        public static readonly string termX0Name = "termX0";

        /// <summary>
        /// Index name for term x1
        /// </summary>
        public static readonly string termX1Name = "termX1";

        /// <summary>
        /// Index name for term x2
        /// </summary>
        public static readonly string termX2Name = "termX2";

        /// <summary>
        /// Index name for term V
        /// </summary>
        public static readonly string termVName = "termV";

        /// <summary>
        /// Index name for term y
        /// </summary>
        public static readonly string termYName = "termY";

        private static readonly Dictionary<string, IArithmetic> functions = new Dictionary<string, IArithmetic>() {
            { "Y_0", ("X_0^3 + b*X_0^2 + c*X_0 + d").ToArithmetic() },
            { "Y'", ("3 * X_0^2 + 2 * b * X_0 + c").ToArithmetic() },
            { "Y''", ("6 * X_0 + 2 * b").ToArithmetic() },
            { "Y", ("[X^2 + U*X - X_1*A]*[X + V - X_2*A]").ToArithmetic() },
            { "S_1", ("U + V - X_2*A = b").ToArithmetic() },
            { "S_2", ("U*(V - X_2*A) - X_1*A = c").ToArithmetic() },
            { "S_3", ("X_1*A*[V-X_2*A] = Y - d").ToArithmetic() },
            { "S_4", ("(c+X_1*A)/(V-X_2*A)=((c+X_1*A)*X_1*A)/(Y-d)").ToArithmetic() }, 
            { "S_5", ("Y - d = X_1*A*(V - X_2*A)").ToArithmetic() },
            { "A", ("((V/[2*X_2])^2 - (Y-d)/(X_1*X_2))v2 + (V/[2*X_2])").ToArithmetic() },
            { "U", ("(c+X_1*A)/(V-X_2*A)").ToArithmetic() },
            { "R_1", ("([U/2]^2 + A*X_1)v2-U/2").ToArithmetic() },
            { "R_2", ("-([U/2]^2 + A*X_1)v2-U/2").ToArithmetic() },
            { "R_3", ("X_2*A-V").ToArithmetic() }
        };


        #endregion

        #region Properties

        /// <summary>
        /// Gets functions to compute
        /// </summary>
        public Dictionary<string, IArithmetic> Functions
        {
            get { return Polynome3.functions; }
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
        /// Gets or sets the D coefficient
        /// </summary>
        public double CoefficientD
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
        /// Gets or sets the X1 term
        /// </summary>
        public double TermX1
        {
            get { return this.Get(termX1Name, 1.0d); }
            set { this.Set(termX1Name, value); }
        }

        /// <summary>
        /// Gets or sets the X2 term
        /// </summary>
        public double TermX2
        {
            get { return this.Get(termX2Name, 1.0d); }
            set { this.Set(termX2Name, value); }
        }

        /// <summary>
        /// Gets or sets the V term
        /// </summary>
        public double TermV
        {
            get { return this.Get(termVName, 1.0d); }
            set { this.Set(termVName, value); }
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
            IArithmetic function = Polynome3.functions["Y_0"];
            this.TermX0 = x0;
            function.Let("b", this.CoefficientB);
            function.Let("c", this.CoefficientC);
            function.Let("d", this.CoefficientD);
            function.Let("X_0", this.TermX0);
            return function.Converting().Compute().ToDouble();
        }

        /// <summary>
        /// Compute Y0 value
        /// </summary>
        /// <param name="x0">X0 value input</param>
        /// <param name="b">b coefficient input</param>
        /// <param name="c">c coefficient input</param>
        /// <param name="d">d coefficient input</param>
        /// <returns>Y0 value output</returns>
        public double ComputeY0(double x0, double b, double c, double d)
        {
            IArithmetic function = Polynome3.functions["Y_0"];
            this.TermX0 = x0;
            this.CoefficientB = b;
            this.CoefficientC = c;
            this.CoefficientD = d;
            function.Let("b", this.CoefficientB);
            function.Let("c", this.CoefficientC);
            function.Let("d", this.CoefficientD);
            function.Let("X_0", this.TermX0);
            return function.Converting().Compute().ToDouble();
        }

        /// <summary>
        /// Compute A value
        /// </summary>
        /// <param name="y">y search input</param>
        /// <param name="x1">x1 input value</param>
        /// <param name="x2">x2 input value</param>
        /// <param name="v">v input value</param>
        /// <returns>A value</returns>
        public double ComputeA(double y, double x1, double x2, double v)
        {
            IArithmetic function = Polynome3.functions["A"];
            this.TermY = y;
            this.TermX1 = x1;
            this.TermX2 = x2;
            this.TermV = v;
            function.Let("b", this.CoefficientB);
            function.Let("c", this.CoefficientC);
            function.Let("d", this.CoefficientD);
            function.Let("X_1", this.TermX1);
            function.Let("X_2", this.TermX2);
            function.Let("V", this.TermV);
            return function.Converting().Compute().ToDouble();
        }

        /// <summary>
        /// Compute A value
        /// </summary>
        /// <param name="y">y search input</param>
        /// <param name="x1">x1 input value</param>
        /// <param name="x2">x2 input value</param>
        /// <param name="v">v input value</param>
        /// <param name="b">b coefficient input</param>
        /// <param name="c">c coefficient input</param>
        /// <param name="d">d coefficient input</param>
        /// <returns>A value</returns>
        public double ComputeA(double y, double x1, double x2, double v, double b, double c, double d)
        {
            IArithmetic function = Polynome3.functions["A"];
            this.TermY = y;
            this.TermX1 = x1;
            this.TermX2 = x2;
            this.TermV = v;
            this.CoefficientB = b;
            this.CoefficientC = c;
            this.CoefficientD = d;
            function.Let("b", this.CoefficientB);
            function.Let("c", this.CoefficientC);
            function.Let("d", this.CoefficientD);
            function.Let("X_1", this.TermX1);
            function.Let("X_2", this.TermX2);
            function.Let("V", this.TermV);
            function.Let("Y", this.TermY);
            return function.Converting().Compute().ToDouble();
        }

        /// <summary>
        /// Compute R1
        /// </summary>
        /// <returns>return R1</returns>
        public double ComputeR1()
        {
            IArithmetic function = Polynome3.functions["R_1"];
            function.Let("b", this.CoefficientB);
            function.Let("c", this.CoefficientC);
            function.Let("d", this.CoefficientD);
            function.Let("X_1", this.TermX1);
            function.Let("X_2", this.TermX2);
            function.Let("V", this.TermV);
            function.Let("Y", this.TermY);
            function.Let("A", Polynome3.functions["A"].Converting());
            function.Let("U", Polynome3.functions["U"].Converting());
            return function.Converting().Compute().ToDouble();
        }

        /// <summary>
        /// Sum
        /// </summary>
        /// <param name="xLeft">xleft</param>
        /// <param name="distance">d</param>
        /// <param name="xRight">xright</param>
        /// <returns>sum</returns>
        public IArithmetic Sum(double xLeft, double distance, double xRight)
        {
            IArithmetic function = ("1/" + distance.ToString() + " + [X0/2] ^2").ToArithmetic();
            IArithmetic sum = null;
            function.Let("X0", xLeft);
            for (double x = xLeft; x < xRight; x = x + 1 / distance)
            {
                if (sum != null)
                    sum = new Addition(sum, function.Converting());
                else
                    sum = function.Converting();
                function.Let("X0", function.Converting());
            }
            return sum;
        }

        #endregion

    }
}
