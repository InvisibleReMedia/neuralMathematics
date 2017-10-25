using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistantModel;
using System.Windows.Documents;

namespace Maths
{
    /// <summary>
    /// Reponse to a question from an exercice
    /// </summary>
    [Serializable]
    public class Answer : PersistentDataObject
    {

        #region Fields

        private static string proposalName = "proposal";
        private static string showName = "show";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="p">proposal</param>
        /// <param name="s">sequence</param>
        public Answer(string p, SequenceProof s)
        {
            this.Set(proposalName, p);
            this.Set(showName, s);
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
            p.Inlines.Add(new Underline(new Run("Réponse:")));
            p.Inlines.Add(new LineBreak());
            p.Inlines.Add(this.Get(proposalName));
            p.Inlines.Add(new LineBreak());
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

        #endregion

    }
}
