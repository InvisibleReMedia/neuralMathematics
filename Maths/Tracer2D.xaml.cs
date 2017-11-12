using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Maths
{
    /// <summary>
    /// Logique d'interaction pour Tracer2D.xaml
    /// </summary>
    public partial class Tracer2D : UserControl
    {
        /// <summary>
        /// Translation on x axis to show a partial of picture
        /// </summary>
        public static DependencyProperty TranslateXProperty = DependencyProperty.Register("TranslateX", typeof(double), typeof(Tracer2D));

        /// <summary>
        /// Translation on y axis to show a partial of picture
        /// </summary>
        public static DependencyProperty TranslateYProperty = DependencyProperty.Register("TranslateY", typeof(double), typeof(Tracer2D));

        /// <summary>
        /// Property to set the graphical tracer algorithm
        /// </summary>
        public static DependencyProperty DistributedTracerProperty = DependencyProperty.Register("DistributedTracer", typeof(DistributedTracer2D), typeof(Tracer2D));

        /// <summary>
        /// Point where starting mouse moving arround
        /// </summary>
        private Point startedMousePosition;
        /// <summary>
        /// Enable mouse moving control
        /// </summary>
        private Boolean duringMove;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Tracer2D()
        {
            InitializeComponent();
            this.SetValue(TranslateXProperty, 0.0d);
            this.SetValue(TranslateYProperty, 0.0d);
            this.MouseDown += mouseOn;
            this.MouseUp += mouseOff;
            this.MouseMove += mouseMove;
            this.MouseLeave += mouseOff;
        }

        /// <summary>
        /// Gets or sets the tracer object
        /// </summary>
        public DistributedTracer2D Tracer
        {
            get
            {
                return this.GetValue(DistributedTracerProperty) as DistributedTracer2D;
            }
            set
            {
                this.SetValue(DistributedTracerProperty, value);
                Window p = Window.GetWindow(this);
                this.Update(new Point(0.0, 0.0),  new Size(p.Width, p.Height), 1.0);
            }
        }

        /// <summary>
        /// mouse leave event
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">mouse args</param>
        private void mouseOff(object sender, MouseEventArgs e)
        {
            if (this.duringMove && this.Tracer != null)
            {
                Point p = e.GetPosition(this);
                double dx = this.startedMousePosition.X - p.X;
                double dy = this.startedMousePosition.Y - p.Y;
                double x = (double)this.GetValue(TranslateXProperty);
                double y = (double)this.GetValue(TranslateYProperty);
                if (this.Translate(x + dx, y + dy))
                {
                    this.SetValue(TranslateXProperty, x + dx);
                    this.SetValue(TranslateYProperty, y + dy);
                }
            }
            this.duringMove = false;
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// mouse move arround control
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">mouse args</param>
        private void mouseMove(object sender, MouseEventArgs e)
        {
            if (this.duringMove && this.Tracer != null)
            {
                Point p = e.GetPosition(this);
                double dx = this.startedMousePosition.X - p.X;
                double dy = this.startedMousePosition.Y - p.Y;
                double x = (double)this.GetValue(TranslateXProperty);
                double y = (double)this.GetValue(TranslateYProperty);
                this.Translate(x + dx, y + dy);
            }
        }

        /// <summary>
        /// When the mouse button was pressed down
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">mouse args</param>
        private void mouseOn(object sender, MouseButtonEventArgs e)
        {
            if (!this.duringMove)
            {
                this.startedMousePosition = e.GetPosition(this);
                this.duringMove = true;
                this.Cursor = Cursors.ScrollAll;
            }
        }

        /// <summary>
        /// Translation
        /// </summary>
        /// <param name="dx">x move</param>
        /// <param name="dy">y move</param>
        /// <returns>true if done</returns>
        public bool Translate(double dx, double dy)
        {
            if (this.Tracer != null)
            {
                this.Update(new Point(dx, dy), this.Tracer.VisualSize, this.Tracer.VisualScale);
            }
            return true;
        }

        /// <summary>
        /// Scale image
        /// </summary>
        /// <param name="d">number &lt; 1.0</param>
        /// <returns>true if done</returns>
        public bool Scale(double d)
        {
            if (d < 0.0) return false;
            if (this.Tracer != null)
            {
                this.Update(this.Tracer.VisualLocation, this.Tracer.VisualSize, d);
            }
            return true;
        }


        /// <summary>
        /// Mise à jour de la taille du tracé
        /// </summary>
        public void Update(Point l, Size s, double scale)
        {
            if (this.Tracer != null)
            {
                this.Tracer.GetThumbnail(l.X, l.Y, s.Width, s.Height, scale, this);
            }
        }
    }
}
