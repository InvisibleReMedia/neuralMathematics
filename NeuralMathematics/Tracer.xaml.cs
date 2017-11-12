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
using System.Windows.Shapes;
using Maths;

namespace NeuralMathematics
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Tracer : Window
    {
        /// <summary>
        /// Construction de la logique d'un tracer
        /// </summary>
        public Tracer()
        {
            InitializeComponent();
            Coordinates[] bornes = new Coordinates[2];
            bornes[0] = new Coordinates(-5.0d, -5.0d);
            bornes[1] = new Coordinates(5.0d, 5.0d);
            Maths.Vector v = new Maths.Vector(bornes[0], bornes[1]);
            Coordinates s = new Coordinates(0.1d, 0.1d);
            MovingCoordinates mc = new MovingCoordinates(v, s);
            DistributedTracer2D d = new DistributedTracer2D(mc, 2, 2, 1, new Size(1.0d, 1.0d));
            this.c.Tracer = d;
        }

    }
}
