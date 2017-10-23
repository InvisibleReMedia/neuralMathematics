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
using WpfMath;
using WpfMath.Controls;
using Maths;
using PersistantModel;
using Interfaces;

namespace NeuralMathematics
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void doc_Loaded(object sender, RoutedEventArgs r)
        {
            this.doc.Document = Applicatifs.Newton();
            this.doc.UpdateLayout();
            this.WindowState = WindowState.Maximized;
        }

        private void doc_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.doc.UpdateLayout();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            this.doc.UpdateLayout();
        }
    }
}
