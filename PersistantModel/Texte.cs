using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace PersistantModel
{
    /// <summary>
    /// Classe supportant un texte
    /// ou un document rtf
    /// </summary>
    public class Texte : Arithmetic
    {

        #region Fields

        private static readonly string textName = "text";

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
        {
            this.Set(textName, t);
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

        #endregion

        #region Methods

        /// <summary>
        /// Converts content to string
        /// </summary>
        /// <returns></returns>
        protected override IArithmetic Create()
        {
            return new Texte();
        }

        /// <summary>
        /// Converts content to tex
        /// </summary>
        /// <returns></returns>
        public override string ToTex()
        {
            return this.Content;
        }

        /// <summary>
        /// Affiche le texte
        /// </summary>
        /// <returns>contenu du texte</returns>
        public override string ToString()
        {
            return this.Get(textName);
        }

        #endregion
    }
}
