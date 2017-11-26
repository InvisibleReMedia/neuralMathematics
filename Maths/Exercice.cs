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
    /// Implémente un exercice
    /// à résoudre
    /// </summary>
    [Serializable]
    public class Exercice : PersistentDataObject, IDocument
    {

        #region Fields

        /// <summary>
        /// Numéro d'exercice
        /// </summary>
        private static readonly string idName = "number";
        /// <summary>
        /// Index name for question name
        /// </summary>
        private static readonly string questionName = "question";
        /// <summary>
        /// Index name for help
        /// </summary>
        private static readonly string indiceName = "nota-bene";
        /// <summary>
        /// Index name for answer name
        /// </summary>
        private static readonly string answerName = "answer";
        /// <summary>
        /// Index name for tex switch
        /// </summary>
        private static readonly string texModeName = "texMode";
        /// <summary>
        /// Index name for the left delimiter
        /// </summary>
        private static readonly string leftDelimiterName = "delimOn";
        /// <summary>
        /// Index name for the right delimiter
        /// </summary>
        private static readonly string rightDelimiterName = "delimOff";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// given a number, the question phrase and a short notice
        /// </summary>
        /// <param name="numero">number</param>
        /// <param name="q">question</param>
        /// <param name="note">notice</param>
        /// <param name="a">answer</param>
        public Exercice(uint numero, string q, string note, Answer a)
            : this(numero, q, note, false, '{', '}', a)
        {
        }

        /// <summary>
        /// constructor for tex mode
        /// given a number, the question phrase and a short notice
        /// </summary>
        /// <param name="numero">number</param>
        /// <param name="q">question</param>
        /// <param name="note">notice</param>
        /// <param name="mode">tex mode switch</param>
        /// <param name="a">answer</param>
        public Exercice(uint numero, string q, string note, bool mode, Answer a)
            : this(numero, q, note, mode, '{', '}', a)
        {
        }

        /// <summary>
        /// General constructor
        /// given a number, the question phrase and a short notice
        /// mode is true if something contains equations
        /// </summary>
        /// <param name="numero">number</param>
        /// <param name="q">question</param>
        /// <param name="note">notice</param>
        /// <param name="mode">mode</param>
        /// <param name="don">delimiter on</param>
        /// <param name="doff">delimiter off</param>
        /// <param name="a">answer</param>
        public Exercice(uint numero, string q, string note, bool mode, char don, char doff, Answer a)
        {
            this.Set(idName, numero);
            this.Set(questionName, q);
            this.Set(indiceName, note);
            this.Set(answerName, a);
            this.Set(texModeName, mode);
            this.Set(leftDelimiterName, don);
            this.Set(rightDelimiterName, doff);
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
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <param name="l">list document</param>
        public void ToDocument(List l)
        {
            Paragraph p = new Paragraph();
            if (this.IsTexMode)
            {
                this.InsertPhraseIntoDocument(p, this.Get(questionName), System.Windows.Media.Brushes.Red);
                p.Inlines.Add(new LineBreak());
                this.InsertPhraseIntoDocument(p, "Aide: ", System.Windows.Media.Brushes.SandyBrown, true);
                this.InsertPhraseIntoDocument(p, this.Get(indiceName), null);
                p.Inlines.Add(new LineBreak());
            }
            else
            {
                Run r = new Run(this.Get(questionName));
                r.Foreground = System.Windows.Media.Brushes.Red;
                p.Inlines.Add(r);
                p.Inlines.Add(new LineBreak());
                r = new Run("Aide: ");
                r.Foreground = System.Windows.Media.Brushes.SandyBrown;
                p.Inlines.Add(new Underline(r));
                p.Inlines.Add(new Run(this.Get(indiceName)));
                p.Inlines.Add(new LineBreak());
            }
            ListItem li = new ListItem(p);
            this.Get(answerName).ToDocument(li.Blocks);
            li.Margin = new System.Windows.Thickness(0, 0, 0, 50.0d);
            l.ListItems.Add(li);
        }

        /// <summary>
        /// Transforms equation object into a string representation
        /// </summary>
        /// <returns>string representation</returns>
        public override string ToString()
        {
            string output = string.Empty;
            output += this.Get(idName).ToString();
            output += ". \t";
            output += this.Get(questionName);
            output += Environment.NewLine;
            output += this.Get(indiceName);
            return output;
        }

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
        /// <param name="foreground">foreground color</param>
        /// <param name="underlined">true if underlined</param>
        private void InsertPhraseIntoDocument(Paragraph p, string phrase, System.Windows.Media.Brush foreground, bool underlined = false)
        {
            foreach (string s in phrase.SplitForTex(this.DelimiterLeft, this.DelimiterRight))
            {
                if (s.StartsWith(this.DelimiterLeft.ToString()) && s.EndsWith(this.DelimiterRight.ToString()))
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
                    if (underlined)
                        p.Inlines.Add(new Underline(t));
                    else
                        p.Inlines.Add(t);
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
        #endregion

    }
}
