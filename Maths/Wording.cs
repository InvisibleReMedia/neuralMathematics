using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistantModel;
using System.Windows.Documents;
using Interfaces;
using WpfMath.Controls;

namespace Maths
{
    /// <summary>
    /// Classe de description d'un énonçé
    /// de mathématique
    /// </summary>
    [Serializable]
    public class Wording : PersistentDataObject, IDocument
    {

        #region Fields

        /// <summary>
        /// Titre de l'énonçé
        /// </summary>
        private static readonly string titleName = "title";
        /// <summary>
        /// Description de l'énonçé
        /// </summary>
        private static readonly string descName = "description";
        /// <summary>
        /// Contenu de présentation (équations et textes)
        /// </summary>
        private static readonly string contentName = "content";
        /// <summary>
        /// Ensemble des exercices
        /// </summary>
        private static readonly string exercicesName = "exercices";
        /// <summary>
        /// Index name pour le mode
        /// </summary>
        private static readonly string texModeName = "texMode";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="title">titre</param>
        /// <param name="desc">description</param>
        /// <param name="mode">mode</param>
        /// <param name="ex">exercices</param>
        public Wording(string title, string desc, bool mode, params Exercice[] ex)
        {
            this.Set(titleName, title);
            this.Set(descName, desc);
            this.Set(exercicesName, ex.ToList());
            this.Set(contentName, new SequenceProof());
            this.Set(texModeName, mode);
        }

        /// <summary>
        /// General constructor
        /// </summary>
        /// <param name="title">titre</param>
        /// <param name="desc">description</param>
        /// <param name="ex">exercices</param>
        public Wording(string title, string desc, params Exercice[] ex) : this(title, desc, false, ex)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the tex mode
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
        /// Gets Sequence proof
        /// </summary>
        public SequenceProof Content
        {
            get
            {
                return this.Get(contentName) as SequenceProof;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a new exercice
        /// </summary>
        /// <param name="e">exercice</param>
        public void Add(Exercice e)
        {
            this.Get(exercicesName).Add(e);
        }

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <param name="f">flow document</param>
        public void ToDocument(FlowDocument f)
        {
            if (this.IsTexMode)
            {
                Paragraph p = new Paragraph();
                this.InsertPhraseIntoDocument(p, this.Get(titleName));
                p.Inlines.Add(new LineBreak());
                this.InsertPhraseIntoDocument(p, this.Get(descName));
                f.Blocks.Add(p);
            }
            else
            {
                Paragraph p = new Paragraph();
                p.Inlines.Add(new Run(this.Get(titleName)));
                p.Inlines.Add(new LineBreak());
                p.Inlines.Add(new Run(this.Get(descName)));
                p.Inlines.Add(new LineBreak());
                f.Blocks.Add(p);
                this.Content.ToDocument(f.Blocks);

                List l = new List();
                l.MarkerStyle = System.Windows.TextMarkerStyle.Decimal;
                foreach (Exercice e in this.Get(exercicesName))
                {
                    e.ToDocument(l);
                }
                f.Blocks.Add(l);
            }
        }

        /// <summary>
        /// Transforms equation object into a string representation
        /// </summary>
        /// <returns>string representation</returns>
        public override string ToString()
        {
            string output = string.Empty;
            output += this.Get(titleName);
            output += Environment.NewLine;
            output += this.Get(descName);
            output += Environment.NewLine;
            output += this.Content.ToString();
            foreach (Exercice e in this.Get(exercicesName))
            {
                output += e.ToString();
            }
            return output;
        }

        #endregion


        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        public string ToTex()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Insert one line into document
        /// </summary>
        /// <param name="p">paragraph</param>
        /// <param name="phrase">text</param>
        private void InsertPhraseIntoDocument(Paragraph p, string phrase)
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"(\{[^\}]*\})");
            foreach (string s in r.Split(phrase))
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
        }

        /// <summary>
        /// Insert text element into document list
        /// </summary>
        /// <param name="list">container</param>
        public void InsertIntoDocument(List list)
        {
            throw new NotSupportedException();
        }
    }
}
