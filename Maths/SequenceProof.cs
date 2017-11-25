using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistantModel;
using Interfaces;
using System.Windows.Documents;
using WpfMath.Controls;

namespace Maths
{
    /// <summary>
    /// Une séquence pour prouver un résultat
    /// au cours d'un exercice
    /// </summary>
    [Serializable]
    public class SequenceProof : PersistentDataObject
    {

        #region Fields

        private static string listName = "list";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected SequenceProof()
        {
        }

        /// <summary>
        /// Constructor
        /// given a list of arithmetic equation
        /// </summary>
        /// <param name="eq">document element</param>
        public SequenceProof(params IDocument[] eq)
        {
            this.Set(listName, new List<IDocument>(eq));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets all items
        /// </summary>
        public IEnumerable<IDocument> Items
        {
            get
            {
                return this.Get(listName);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Ajout d'éléments dans la liste
        /// </summary>
        /// <param name="items">éléments à ajouter</param>
        public void Add(params IDocument[] items)
        {
            List<IDocument> list = this.Get(listName);
            list.AddRange(items);
            this.Set(listName, list);
        }

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <param name="c">block collection</param>
        public void ToDocument(BlockCollection c)
        {
            List l = new List();
            l.Margin = new System.Windows.Thickness(50.0d, 0, 0, 0);
            l.MarkerStyle = System.Windows.TextMarkerStyle.None;
            foreach(IDocument a in this.Get(listName))
            {
                if (a is Texte)
                {
                    Texte t = a as Texte;
                    if (t.IsTexMode)
                    {
                        t.InsertIntoDocument(l);
                    }
                    else
                    {
                        ListItem li = new ListItem(new Paragraph(new Run(t.ToString())));
                        l.ListItems.Add(li);
                    }
                }
                else
                {
                    ListItem li = new ListItem();
                    li.TextAlignment = System.Windows.TextAlignment.Center;
                    FormulaControl fc = new FormulaControl();
                    fc.Formula = a.ToTex();
                    li.Blocks.Add(new BlockUIContainer(fc));
                    l.ListItems.Add(li);
                }
            }
            c.Add(l);
        }

        /// <summary>
        /// Transforms equation object into a string representation
        /// </summary>
        /// <returns>string representation</returns>
        public override string ToString()
        {
            string output = string.Empty;
            foreach (IDocument a in this.Get(listName))
            {
                output += a.ToString();
                output += "\n";
            }
            return output;
        }

        #endregion

    }
}
