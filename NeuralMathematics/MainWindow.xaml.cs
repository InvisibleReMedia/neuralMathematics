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
            Wording w = new Wording("Résolution des polynômes", "La résolution des polynômes est un sujet encore mathématiquement non élucidé");
            SequenceProof s1 = new SequenceProof();
            Texte t1 = new Texte("Degré 2");
            IArithmetic eq1 = new Equal(new Power(new Addition(new Coefficient("a"), new Coefficient("b")), new NumericValue(2.0d)),
                                        new Sum(new Power(new Coefficient("a"), new NumericValue(2.0d)), new Product(new NumericValue(2.0d), new Coefficient("a"), new Coefficient("b")), new Power(new Coefficient("b"), new NumericValue(2.0d))));
            s1.Add(t1);
            s1.Add(eq1);
            Answer a = new Answer("Expressions avec la formule de Newton", s1);
            Exercice e = new Exercice(1, "Formule de Newton", "Décrivez la production de la formule de newton à l'ordre 2 et 3", a);
            w.Add(e);
            FlowDocument fd = new FlowDocument();
            w.ToDocument(fd);

            this.doc.Document = fd;
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
