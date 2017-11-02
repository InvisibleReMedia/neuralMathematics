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

        private BitmapCache bitmap;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor as a coordinate system
        /// </summary>
        public DataTracer2D() : base(typeof(CoordinateSystem))
        {
            Coordinates a = new Coordinates(-10.0d, -10.0d);
            Coordinates b = new Coordinates(10.0d, 10.0d);
            Coordinates s = new Coordinates(1.0, 1.0);
            Vector v = new Vector(a, b);
            MovingCoordinates mc = new MovingCoordinates(v, s);
            this.sys = new CoordinateSystem(mc);
            Arithmetic f = new Sum(new Multiplication(new NumericValue(2.0d), new UnknownTerm("x")), new NumericValue(2.0d));
            for (double d = a.Value; d <= b.Value; d += s.Value)
            {
                try
                {
                    this.sys.Add(f, new string[] { "x", "y" }, new double[] { d });
                }
                catch (IndexOutOfRangeException)
                {

                }
            }
        }

        #endregion

        #region Methods

        public void DrawCurve()
        {
            DrawingVisual dv = new DrawingVisual();
            DrawingContext dc = dv.RenderOpen();

        }

        #endregion

    }
}
