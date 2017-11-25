using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistantModel;
using System.Windows.Documents;
using Interfaces;

namespace Maths
{
    /// <summary>
    /// Reponse to a question from an exercice
    /// </summary>
    [Serializable]
    public class Answer : PersistentDataObject, IDocument
    {

        #region Fields

        /// <summary>
        /// Proposal text
        /// </summary>
        private static readonly string proposalName = "proposal";
        /// <summary>
        /// This is the work to finish the exercice
        /// </summary>
        private static readonly string showName = "show";
        /// <summary>
        /// Tex mode
        /// </summary>
        private static readonly string texModeName = "texMode";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="p">proposal</param>
        /// <param name="s">sequence</param>
        public Answer(string p, SequenceProof s) : this(p, false, s)
        {
        }

        /// <summary>
        /// General constructor
        /// </summary>
        /// <param name="p">proposal</param>
        /// <param name="mode">tex mode</param>
        /// <param name="s">sequence</param>
        public Answer(string p, bool mode, SequenceProof s)
        {
            this.Set(proposalName, p);
            this.Set(texModeName, mode);
            this.Set(showName, s);
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

        #endregion

        #region Methods

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <param name="c">block collection</param>
        public void ToDocument(BlockCollection c)
        {
            Paragraph p = new Paragraph();
            p.Margin = new System.Windows.Thickness(50.0d, 0, 0, 0);
            if (this.IsTexMode)
            {
                Run r = new Run("Réponse:");
                r.Foreground = System.Windows.Media.Brushes.GreenYellow;
                p.Inlines.Add(new Underline(r));
                p.Inlines.Add(new LineBreak());
                this.InsertPhraseIntoDocument(p, this.Get(proposalName), null);
                p.Inlines.Add(new LineBreak());
            }
            else
            {
                Run r = new Run("Réponse:");
                r.Foreground = System.Windows.Media.Brushes.GreenYellow;
                p.Inlines.Add(new Underline(r));
                p.Inlines.Add(new LineBreak());
                p.Inlines.Add(this.Get(proposalName));
                p.Inlines.Add(new LineBreak());
            }
            c.Add(p);
            this.Get(showName).ToDocument(c);
        }

        /// <summary>
        /// Transforms equation object into a string representation
        /// </summary>
        /// <returns>string representation</returns>
        public override string ToString()
        {
            string output = string.Empty;
            output += this.Get(proposalName);
            output += Environment.NewLine;
            output += this.Get(showName).ToTex();
            return output;
        }

        /// <summary>
        /// Insert one line into document
        /// </summary>
        /// <param name="p">paragraph</param>
        /// <param name="phrase">text</param>
        /// <param name="foreground">foreground color</param>
        private void InsertPhraseIntoDocument(Paragraph p, string phrase, System.Windows.Media.Brush foreground)
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"(\{[^\}]*\})");
            foreach (string s in r.Split(phrase))
            {
                if (s.StartsWith("{") && s.EndsWith("}"))
                {
                    string tex = s.Substring(1, s.Length - 2);
                    WpfMath.Controls.FormulaControl fc = new WpfMath.Controls.FormulaControl();
                    fc.Formula = tex;
                    InlineUIContainer i = new InlineUIContainer(fc);
                    if (foreground != null)
                        i.Foreground = foreground;
                    p.Inlines.Add(i);
                }
                else
                {
                    Run t = new Run(s);
                    if (foreground != null)
                        t.Foreground = foreground;
                    p.Inlines.Add(t);
                }
            }
        }

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        public string ToTex()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert text element into document list
        /// </summary>
        /// <param name="list">container</param>
        public void InsertIntoDocument(List list)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
