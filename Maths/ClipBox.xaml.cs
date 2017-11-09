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
    /// Logique d'interaction pour ClipBox.xaml
    /// </summary>
    public partial class ClipBox : UserControl
    {

        /// <summary>
        /// Translation on x axis to show a partial of picture
        /// </summary>
        public static DependencyProperty TranslateXProperty = DependencyProperty.Register("TranslateX", typeof(double), typeof(ClipBox));

        /// <summary>
        /// Translation on y axis to show a partial of picture
        /// </summary>
        public static DependencyProperty TranslateYProperty = DependencyProperty.Register("TranslateY", typeof(double), typeof(ClipBox));

        /// <summary>
        /// Scale on x and y axis to zoom into picture
        /// </summary>
        public static DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(double), typeof(ClipBox));

        /// <summary>
        /// Scale on y axis to zoom into picture
        /// </summary>
        public static DependencyProperty TracerProperty = DependencyProperty.Register("Tracer", typeof(DistributedTracer2D), typeof(ClipBox));

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
        public ClipBox()
        {
            InitializeComponent();
            this.tracer.MouseDown += mouseButtonDown;
            this.tracer.MouseUp += mouseButtonUp;
            this.tracer.MouseMove += mouseMove;
            this.tracer.MouseLeave += mouseLeave;
        }

        /// <summary>
        /// Gets or sets the tracer object
        /// </summary>
        public DistributedTracer2D Tracer
        {
            get
            {
                return this.tracer.Tracer;
            }
            set
            {
                this.tracer.Tracer = value;
                this.w_Loaded(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// mouse leave event
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">mouse args</param>
        private void mouseLeave(object sender, MouseEventArgs e)
        {
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
                Point p = e.GetPosition(this.tracer);
                double dx = this.startedMousePosition.X - p.X;
                double dy = this.startedMousePosition.Y - p.Y;
                double x = (double)this.GetValue(TranslateXProperty);
                double y = (double)this.GetValue(TranslateYProperty);
                if (this.tracer.Translate(x + dx, y + dy))
                {
                    this.SetValue(TranslateXProperty, x + dx);
                    this.SetValue(TranslateYProperty, y + dy);
                }

            }
        }

        /// <summary>
        /// When the mouse button was pressed up
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">mouse args</param>
        private void mouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.duringMove = false;
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the mouse button was pressed down
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">mouse args</param>
        private void mouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.startedMousePosition = e.GetPosition(this.tracer);
            this.duringMove = true;
            this.Cursor = Cursors.ScrollAll;
        }

        /// <summary>
        /// When the zoom value increases
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">args</param>
        private void increaseZoom_Click(object sender, RoutedEventArgs e)
        {
            double d = (double)this.GetValue(ScaleProperty);
            if (this.tracer.Scale(d + 0.001))
                this.SetValue(ScaleProperty, d + 0.001);
        }

        /// <summary>
        /// When the zoom value decreases
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">args</param>
        private void decreaseZoom_Click(object sender, RoutedEventArgs e)
        {
            double d = (double)this.GetValue(ScaleProperty);
            if (this.tracer.Scale(d - 0.001))
                this.SetValue(ScaleProperty, d - 0.001);
        }

        /// <summary>
        /// When ui was loaded
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">args</param>
        private void w_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetValue(TranslateXProperty, 0.0d);
            this.SetValue(TranslateYProperty, 0.0d);
            this.SetValue(ScaleProperty, 1.0d);
        }

        private void tracer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.Tracer != null)
                this.tracer.Update(this.Tracer.VisualLocation, e.NewSize, this.Tracer.VisualScale);
        }
    }
}
