using Interfaces;
using PersistantModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths
{
    /// <summary>
    /// This is the top level arithmetic
    /// model to be save or load from a binary file
    /// </summary>
    [Serializable]
    public class TopLevelArithmeticModel : PersistentDataObject
    {

        #region Fields

        /// <summary>
        /// Version model
        /// </summary>
        private static readonly string versionName = "version";
        /// <summary>
        /// Equation list
        /// </summary>
        private static readonly string wordingListName = "wordingList";
        /// <summary>
        /// title of the file
        /// </summary>
        private static readonly string titleName = "title";
        /// <summary>
        /// Revision number
        /// </summary>
        private static readonly string revisionName = "rev";
        /// <summary>
        /// Hash code dictionary
        /// </summary>
        private static readonly string hashCodeDictName = "hash";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        protected TopLevelArithmeticModel()
        {
            this.Set(versionName,"1.0.0.0");
            this.Set(wordingListName, new List<Wording>());
            this.Set(titleName, "New");
            this.Set(revisionName, UInt32.MinValue);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        protected TopLevelArithmeticModel(string title) : this()
        {
            this.Set(titleName, title);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets equation list
        /// </summary>
        public List<Wording> WordingList
        {
            get
            {
                return this.Get(wordingListName);
            }
        }

        /// <summary>
        /// Gets or sets version number
        /// </summary>
        public string Version
        {
            get
            {
                return this.Get(versionName);
            }
            set
            {
                this.Set(versionName, value);
            }
        }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title
        {
            get
            {
                return this.Get(titleName);
            }
            set
            {
                this.Set(titleName, value);
            }
        }

        /// <summary>
        /// Gets or sets the revision number
        /// </summary>
        public uint Revision
        {
            get
            {
                return this.Get(revisionName);
            }
            set
            {
                this.Set(revisionName, value);
            }
        }

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
            if (PersistentDataObject.Load(fi, out t))
            {
            }
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
            uint rev = this.Get(revisionName);
            this.Set(revisionName, rev + 1);
            PersistentDataObject.Save(fi, this);
        }

        #endregion

    }
}
