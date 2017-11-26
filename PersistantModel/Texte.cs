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
        /// <summary>
        /// Index name for the left delimiter
        /// </summary>
        private static readonly string leftDelimiterName = "delimOn";
        /// <summary>
        /// Index name for the right delimiter
        /// </summary>
        private static readonly string rightDelimiterName = "delimOff";

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
        public Texte(string t)
            : this(t, false, '{', '}')
        {
        }

        /// <summary>
        /// Constructor
        /// given a text to be written in tex mode
        /// </summary>
        /// <param name="t">text</param>
        /// <param name="tex">mode</param>
        public Texte(string t, bool tex)
            : this(t, tex, '{', '}')
        {
        }

        /// <summary>
        /// Constructor
        /// given a text to be written in tex mode
        /// </summary>
        /// <param name="t">text</param>
        /// <param name="tex">mode</param>
        /// <param name="don">left delimiter</param>
        /// <param name="doff">right delimiter</param>
        public Texte(string t, bool tex, char don, char doff)
        {
            this.Set(textName, t);
            this.Set(texModeName, tex);
            this.Set(leftDelimiterName, don);
            this.Set(rightDelimiterName, doff);
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

        /// <summary>
        /// Gets or sets the delimiter
        /// </summary>
        public char DelimiterLeft
        {
            get
            {
                return this.Get(leftDelimiterName);
            }
            set
            {
                this.Set(leftDelimiterName, value);
            }
        }

        /// <summary>
        /// Gets or sets the delimiter
        /// </summary>
        public char DelimiterRight
        {
            get
            {
                return this.Get(rightDelimiterName);
            }
            set
            {
                this.Set(rightDelimiterName, value);
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

            this.Content.InsertIntoDocument(this.DelimiterLeft, this.DelimiterRight, p);

            ListItem li = new ListItem(p);
            list.ListItems.Add(li);
        }

        #endregion

    }
}
