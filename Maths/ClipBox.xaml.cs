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
        /// Point where starting mouse moving arround
        /// </summary>
        private Point startedMousePosition;
        /// <summary>
        /// Point where translation has started
        /// </summary>
        private Point currentTranslation;
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
            ((UIElement)this.container.Content).MouseDown += mouseOn;
            ((UIElement)this.container.Content).MouseUp += mouseOff;
            ((UIElement)this.container.Content).MouseMove += mouseMove;
            ((UIElement)this.container.Content).MouseLeave += mouseOff;
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
        private void mouseOff(object sender, MouseEventArgs e)
        {
            if (this.duringMove && this.Tracer != null)
            {
                this.SetValue(TranslateXProperty, this.currentTranslation.X);
                this.SetValue(TranslateYProperty, this.currentTranslation.Y);
                this.duringMove = false;
                this.Cursor = Cursors.Arrow;
            }
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
                Point p = e.GetPosition(this.container);
                double dx = this.startedMousePosition.X - p.X;
                double dy = this.startedMousePosition.Y - p.Y;
                double x = (double)this.GetValue(TranslateXProperty);
                double y = (double)this.GetValue(TranslateYProperty);
                this.Translate(x + dx, y + dy);
            }
        }

        /// <summary>
        /// Translation on x and y axis
        /// </summary>
        /// <param name="dx">x move</param>
        /// <param name="dy">y move</param>
        /// <returns>true if not out of bounds</returns>
        private bool Translate(double dx, double dy)
        {
            double cx, cy;
            bool moved;
            if (dx >= 0 && dx <= this.container.ScrollableWidth)
            {
                this.container.ScrollToHorizontalOffset(dx);
                cx = dx;
                moved = true;
            }
            else
            {
                cx = this.currentTranslation.X;
                moved = false;
            }
            if (dy >= 0 && dy <= this.container.ScrollableHeight)
            {
                this.container.ScrollToVerticalOffset(dy);
                cy = dy;
                moved = true;
            }
            else
            {
                cy = this.currentTranslation.Y;
                moved = false;
            }
            this.currentTranslation = new Point(cx, cy);
            return moved;
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
                this.startedMousePosition = e.GetPosition(this.container);
                double x = (double)this.GetValue(TranslateXProperty);
                double y = (double)this.GetValue(TranslateYProperty);
                this.currentTranslation = new Point(x, y);
                this.duringMove = true;
                this.Cursor = Cursors.ScrollAll;
            }
        }

        /// <summary>
        /// Scale image
        /// </summary>
        /// <param name="d">number &gt; 0.0</param>
        /// <returns>true if not out of bounds</returns>
        public bool Scale(double d)
        {
            if (d <= 0.0) return false;
            if (this.Tracer != null)
            {
                this.tracer.Update(this.Tracer.VisualLocation, this.Tracer.VisualSize, d);
            }
            return true;
        }

        /// <summary>
        /// When the zoom value increases
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">args</param>
        private void increaseZoom_Click(object sender, RoutedEventArgs e)
        {
            double d = (double)this.GetValue(ScaleProperty);
            if (this.Scale(d + 0.01))
                this.SetValue(ScaleProperty, d + 0.01);
        }

        /// <summary>
        /// When the zoom value decreases
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">args</param>
        private void decreaseZoom_Click(object sender, RoutedEventArgs e)
        {
            double d = (double)this.GetValue(ScaleProperty);
            if (this.Scale(d - 0.01))
                this.SetValue(ScaleProperty, d - 0.01);
        }

        /// <summary>
        /// When ui was loaded
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">args</param>
        private void w_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetValue(ScaleProperty, 1.0d);
            this.SetValue(TranslateXProperty, 0.0d);
            this.SetValue(TranslateYProperty, 0.0d);
        }

        /// <summary>
        /// When ui is resized
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">args</param>
        private void w_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.Tracer != null)
                this.tracer.Update(this.Tracer.VisualLocation, e.NewSize, this.Tracer.VisualScale);
        }
    }
}
