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
    public class Wording : PersistentDataObject
    {

        #region Fields

        /// <summary>
        /// Titre de l'énonçé
        /// </summary>
        private static string titleName = "title";
        /// <summary>
        /// Description de l'énonçé
        /// </summary>
        private static string descName = "description";
        /// <summary>
        /// Ensemble des exercices
        /// </summary>
        private static string exercicesName = "exercices";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="title">titre</param>
        /// <param name="desc">description</param>
        /// <param name="ex">exercices</param>
        public Wording(string title, string desc, params Exercice[] ex)
        {
            this.Set(titleName, title);
            this.Set(descName, desc);
            this.Set(exercicesName, ex.ToList());
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
            Paragraph p = new Paragraph();
            p.Inlines.Add(new Run(this.Get(titleName)));
            p.Inlines.Add(new LineBreak());
            p.Inlines.Add(new Run(this.Get(descName)));
            p.Inlines.Add(new LineBreak());
            f.Blocks.Add(p);

            List l = new List();
            l.MarkerStyle = System.Windows.TextMarkerStyle.Decimal;
            foreach (Exercice e in this.Get(exercicesName))
            {
                e.ToDocument(l);

            }
            f.Blocks.Add(l);
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
            foreach (Exercice e in this.Get(exercicesName))
            {
                output += e.ToString();
            }
            return output;
        }

        #endregion

    }
}
