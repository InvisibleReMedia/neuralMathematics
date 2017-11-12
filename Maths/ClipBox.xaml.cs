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
        /// Scale on x and y axis to zoom into picture
        /// </summary>
        public static DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(double), typeof(ClipBox));

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClipBox()
        {
            InitializeComponent();
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
        /// When the zoom value increases
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">args</param>
        private void increaseZoom_Click(object sender, RoutedEventArgs e)
        {
            double d = (double)this.GetValue(ScaleProperty);
            if (this.tracer.Scale(d + 0.01))
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
            if (this.tracer.Scale(d - 0.01))
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
