using Interfaces;
using Maths;
using PersistantModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace NeuralMathematics
{
    /// <summary>
    /// Logique d'interaction pour Tracer.xaml
    /// </summary>
    public partial class Graphique : UserControl, IDocument
    {

        /// <summary>
        /// Tracing function
        /// </summary>
        private Func<DistributedTracer2D> tracingFunction;

        /// <summary>
        /// Tracer
        /// </summary>
        public Graphique(Func<DistributedTracer2D> d)
        {
            this.tracingFunction = d;
            InitializeComponent();
        }

        /// <summary>
        /// Gets the tex mode
        /// </summary>
        public bool IsTexMode
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Insert text element into document list
        /// </summary>
        /// <param name="list">container</param>
        public void InsertIntoDocument(List list)
        {
            Paragraph p = new Paragraph();

            p.Inlines.Add(new InlineUIContainer(this));

            ListItem li = new ListItem(p);
            list.ListItems.Add(li);
        }

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        public string ToTex()
        {
            throw new NotImplementedException();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher owner = Dispatcher.CurrentDispatcher;
            Task.Factory.StartNew(this.tracingFunction).ContinueWith((t) =>
            {
                owner.Invoke(() =>
                {
                    Tracer tracer = new Tracer(t.Result);
                    tracer.ShowDialog();
                });
            });
        }

        /// <summary>
        /// Creates a new graphics tuner
        /// </summary>
        /// <param name="init">init function</param>
        /// <returns>graphic button</returns>
        public static Graphique Create(Func<DistributedTracer2D> init)
        {
            return new Graphique(init);
        }
    }

}
