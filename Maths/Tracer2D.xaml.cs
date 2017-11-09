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
        /// Property to set the graphical tracer algorithm
        /// </summary>
        public static DependencyProperty DistributedTracerProperty = DependencyProperty.Register("DistributedTracer", typeof(DistributedTracer2D), typeof(Tracer2D));

        /// <summary>
        /// Default constructor
        /// </summary>
        public Tracer2D()
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
                return this.GetValue(DistributedTracerProperty) as DistributedTracer2D;
            }
            set
            {
                this.SetValue(DistributedTracerProperty, value);
                Window p = Window.GetWindow(this);
                this.RenderSize = value.ImageSize;
                this.Update(new Point(0.0, 0.0),  new Size(p.Width, p.Height), 1.0);
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
            if ((dx < 0 || dx >= this.RenderSize.Width) || (dy < 0 || dy >= this.RenderSize.Height)) return false;
            if (this.Tracer != null)
                this.Update(new Point(dx, dy), new Size(this.Tracer.VisualSize.Width + dx, this.Tracer.VisualSize.Height + dy), this.Tracer.VisualScale);
            return true;
        }

        /// <summary>
        /// Scale image
        /// </summary>
        /// <param name="d">number &lt; 1.0</param>
        /// <returns>true if done</returns>
        public bool Scale(double d)
        {
            if (d < 0.0 || d > 1.0) return false;
            if (this.Tracer != null)
                this.Update(this.Tracer.VisualLocation, this.Tracer.VisualSize, d);
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

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.Tracer != null)
                this.Update(this.Tracer.VisualLocation, e.NewSize, this.Tracer.VisualScale);
        }
    }
}
