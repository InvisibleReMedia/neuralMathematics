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
    /// Implémente un exercice
    /// à résoudre
    /// </summary>
    [Serializable]
    public class Exercice : PersistentDataObject
    {

        #region Fields

        /// <summary>
        /// Numéro d'exercice
        /// </summary>
        private static string idName = "number";
        private static string questionName = "question";
        private static string indiceName = "nota-bene";
        private static string answerName = "answer";

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
        {
            this.Set(idName, numero);
            this.Set(questionName, q);
            this.Set(indiceName, note);
            this.Set(answerName, a);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <param name="l">list document</param>
        public void ToDocument(List l)
        {
            Paragraph p = new Paragraph(new Run(this.Get(questionName)));
            p.Inlines.Add(new LineBreak());
            p.Inlines.Add(this.Get(indiceName));
            ListItem li = new ListItem(p);
            this.Get(answerName).ToDocument(li.Blocks);
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

        #endregion

    }
}
