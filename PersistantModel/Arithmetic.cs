using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Classe permettant
    /// d'enregistrer des éléments
    /// arithmetiques
    /// </summary>
    [Serializable]
    public class Arithmetic : PersistentDataObject, ICloneable
    {

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected Arithmetic()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value from
        /// persistent data
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>value</returns>
        public dynamic this[string name]
        {
            get
            {
                if (this.Data.ContainsKey(name))
                {
                    return this.Data[name];
                }
                else
                {
                    this.Data.Add(name, null);
                }
                return this.Data[name];
            }
            set
            {
                if (this.Data.ContainsKey(name))
                {
                    this.Data[name] = value;
                }
                else
                {
                    this.Data.Add(name, value);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new arithmetic class
        /// </summary>
        protected virtual Arithmetic Create()
        {
            return new Arithmetic();
        }

        /// <summary>
        /// Clone this arithmetic object
        /// </summary>
        /// <returns>new arithmetic object</returns>
        public object Clone()
        {
            Arithmetic p = this.Create();
            foreach (string key in this.Data.Keys)
            {
                p.Data.Add(key, this.Data[key].Clone());
            }
            return p;
        }

        #endregion
    }
}
