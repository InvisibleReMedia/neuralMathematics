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

        public SequenceProof(params IArithmetic[] eq)
        {
            this.Set(listName, new List<IArithmetic>(eq));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets all items
        /// </summary>
        public IEnumerable<IArithmetic> Items
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
        public void Add(params IArithmetic[] items)
        {
            List<IArithmetic> list = this.Get(listName);
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
            foreach(IArithmetic a in this.Get(listName))
            {
                if (a is Texte)
                {
                    ListItem li = new ListItem(new Paragraph(new Run(a.ToString())));
                    l.ListItems.Add(li);
                }
                else
                {
                    ListItem li = new ListItem();
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
            foreach (IArithmetic a in this.Get(listName))
            {
                output += a.ToString();
                output += "\n";
            }
            return output;
        }

        #endregion

    }
}
