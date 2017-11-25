using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using System.Windows.Documents;
using System.Windows;

namespace PersistantModel
{
    /// <summary>
    /// Classe supportant un texte
    /// ou un document rtf
    /// </summary>
    [Serializable]
    public class Texte : PersistentDataObject, IDocument
    {

        #region Fields
        /// <summary>
        /// Index name for text
        /// </summary>
        private static readonly string textName = "text";
        /// <summary>
        /// Index name for mode
        /// </summary>
        private static readonly string texModeName = "mode";

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// Empty data
        /// </summary>
        protected Texte()
        {

        }

        /// <summary>
        /// Constructor
        /// given a specific text
        /// </summary>
        /// <param name="t">text</param>
        public Texte(string t) : this(t, false)
        {
        }

        /// <summary>
        /// Constructor
        /// given a text to be written in tex mode
        /// </summary>
        /// <param name="t">text</param>
        /// <param name="tex">mode</param>
        public Texte(string t, bool tex)
        {
            this.Set(textName, t);
            this.Set(texModeName, tex);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the content
        /// </summary>
        public string Content
        {
            get
            {
                return this.Get(textName);
            }
            set
            {
                this.Set(textName, value);
            }
        }

        /// <summary>
        /// Gets or sets the mode
        /// </summary>
        public bool IsTexMode
        {
            get
            {
                return this.Get(texModeName);
            }
            set
            {
                this.Set(texModeName, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts content to tex
        /// </summary>
        /// <returns>tex mode representation</returns>
        public string ToTex()
        {
            return this.Content;
        }

        /// <summary>
        /// Affiche le texte
        /// </summary>
        /// <returns>contenu du texte</returns>
        public override string ToString()
        {
            return this.Content;
        }

        /// <summary>
        /// Insert text elements into a list
        /// </summary>
        /// <param name="list"></param>
        public void InsertIntoDocument(List list)
        {
            Paragraph p = new Paragraph();
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"(\{[^\}]*\})");
            foreach (string s in r.Split(this.Content))
            {
                if (s.StartsWith("{") && s.EndsWith("}"))
                {
                    string tex = s.Substring(1, s.Length - 2);
                    WpfMath.Controls.FormulaControl fc = new WpfMath.Controls.FormulaControl();
                    fc.Formula = tex;
                    p.Inlines.Add(new InlineUIContainer(fc));
                }
                else
                {
                    p.Inlines.Add(new Run(s));
                }
            }
            ListItem li = new ListItem(p);
            list.ListItems.Add(li);
        }

        #endregion

    }
}
