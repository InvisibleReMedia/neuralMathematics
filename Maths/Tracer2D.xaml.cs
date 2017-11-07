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
                this.AddChild(value.ThumbnailImage(p.Width, p.Height));
            }
        }



    }
}
