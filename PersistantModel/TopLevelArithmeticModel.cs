using Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// This is the top level arithmetic
    /// model to be save or load from a binary file
    /// </summary>
    [Serializable]
    class TopLevelArithmeticModel : PersistentDataObject
    {

        #region Fields

        /// <summary>
        /// Version model
        /// </summary>
        private static string versionName = "version";
        /// <summary>
        /// Equation list
        /// </summary>
        private static string equationListName = "equations";
        /// <summary>
        /// title of the file
        /// </summary>
        private static string titleName = "title";
        /// <summary>
        /// Revision number
        /// </summary>
        private static string revisionName = "rev";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        protected TopLevelArithmeticModel()
        {
            this.Set(versionName,"1.0.0.0");
            this.Set(equationListName, new List<Arithmetic>());
            this.Set(titleName, "New");
            this.Set(revisionName, 0);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        protected TopLevelArithmeticModel(string title) : base()
        {
            this.Set(titleName, title);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Create a new top level class
        /// </summary>
        public static TopLevelArithmeticModel Create(string title)
        {
            TopLevelArithmeticModel t = new TopLevelArithmeticModel(title);
            return t;
        }

        /// <summary>
        /// Load a top level arithmetic model
        /// from an existing file
        /// </summary>
        /// <param name="fi">file info to take</param>
        /// <returns>object deserialized</returns>
        public static TopLevelArithmeticModel Load(FileInfo fi)
        {
            PersistentDataObject t = null;
            PersistentDataObject.Load(fi, out t);
            return t as TopLevelArithmeticModel;
        }

        /// <summary>
        /// Load a top level arithmetic model
        /// from an existing file
        /// </summary>
        /// <param name="fi">file info to take</param>
        /// <returns>object deserialized</returns>
        public void Save(FileInfo fi)
        {
            int rev = this.Get(revisionName);
            this.Set(revisionName, rev + 1);
            PersistentDataObject.Save(fi, this);
        }

        #endregion

    }
}
