using PersistantModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Maths
{
    /// <summary>
    /// Tracer en deux dimensions
    /// </summary>
    public class DataTracer2D : DataTemplate
    {

        #region Fields

        /// <summary>
        /// Système de coordonnées
        /// </summary>
        private CoordinateSystem sys;

        /// <summary>
        /// bornes limites à gauche et à droite
        /// </summary>
        private Coordinates from, to;

        /// <summary>
        /// Série d'intervalles
        /// </summary>
        private Coordinates[] intervals;

        /// <summary>
        /// arithmetic function
        /// </summary>
        private Arithmetic function;

        /// <summary>
        /// To draw
        /// </summary>
        private DrawingVisual drawer;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor as a coordinate system
        /// </summary>
        public DataTracer2D() : base(typeof(CoordinateSystem))
        {
            this.drawer = new DrawingVisual();
            this.from = new Coordinates(-10.0d, -10.0d);
            this.to = new Coordinates(10.0d, 10.0d);
            this.intervals = new Coordinates[2];
            this.intervals[0] = new Coordinates(1.0d, 1.0d);
            this.intervals[1] = new Coordinates(0.1d, 0.1d);

            Vector v = new Vector(this.from, this.to);
            MovingCoordinates mc = new MovingCoordinates(v, this.intervals[0]);
            this.sys = new CoordinateSystem(mc);
            this.function = new Sum(new Multiplication(new NumericValue(2.0d), new UnknownTerm("x")), new NumericValue(2.0d));
            for (double d = this.from.Value; d <= this.to.Value; d += this.intervals[0].Value)
            {
                try
                {
                    this.sys.Add(this.function, new string[] { "x", "y" }, new double[] { d });
                }
                catch (IndexOutOfRangeException)
                {

                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draw lines
        /// </summary>
        /// <param name="start">first point</param>
        /// <param name="end">last point</param>
        /// <param name="interval">interval between two points</param>
        /// <param name="dc">drawing context</param>
        /// <param name="p">pen for the draw</param>
        private void DrawLines(Coordinates start, Coordinates end, Coordinates interval, DrawingContext dc, Pen p)
        {
            for (double d = start.Value; d <= end.Value; d += this.intervals[0].Value)
            {
                dc.DrawLine(p, new Point(d, start.Euclidian.Value), new Point(d, end.Euclidian.Value));
            }
        }

        /// <summary>
        /// Dessine la graduation
        /// </summary>
        protected void DrawGrad()
        {
            Vector v = new Vector(this.from, this.to);
            MovingCoordinates mc = new MovingCoordinates(v, this.intervals[0]);
            CoordinateSystem[] axes = new CoordinateSystem[3];
            axes[0] = new CoordinateSystem(mc);
            mc = new MovingCoordinates(v, this.intervals[1]);
            axes[1] = new CoordinateSystem(mc);

            DrawingContext dc = this.drawer.RenderOpen();
            Brush b1 = new SolidColorBrush(Colors.BurlyWood);
            Pen p1 = new Pen(b1, 0.1d);

            this.DrawLines(this.from, this.to, this.intervals[0], dc, p1);
            Pen p2 = new Pen(b1, 0.01d);
            this.DrawLines(this.from.Euclidian, this.to.Euclidian, this.intervals[0].Euclidian, dc, p2);

        }

        /// <summary>
        /// Draw the curve
        /// </summary>
        public void DrawCurve()
        {
            this.DrawGrad();
        }

        #endregion

    }
}
