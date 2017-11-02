using Interfaces;
using PersistantModel;
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
    /// Logique d'interaction pour Tracer.xaml
    /// </summary>
    public partial class Tracer : UserControl
    {
        /// <summary>
        /// Tracer
        /// </summary>
        public Tracer()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<int>[] grid = new List<int>[10];
            for (int x = 0; x < 10; ++x)
            {
                grid[x] = new List<int>();
                for (int y = 0; y < 10; ++y)
                {
                    grid[x].Add(x + y);
                }
            }
            this.lst.ItemsSource = grid;
        }
    }

}
