using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace PersistantModel
{
    /// <summary>
    /// Recording zone to handle objects
    /// </summary>
    public class RecordZone<T> : PersistentDataObject where T : PersistentDataObject
    {

        #region Fields

        /// <summary>
        /// Store objects
        /// </summary>
        private static readonly string objectListName = "objects";

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public RecordZone()
        {
            this.Set(objectListName, new Dictionary<int, T>());
        }

        /// <summary>
        /// Constructor
        /// given an existing dictionary of objects
        /// </summary>
        /// <param name="keys"></param>
        public RecordZone(params KeyValuePair<int, T>[] keys)
        {
            this.Set(objectListName, keys.ToDictionary(x => x.Key, y => y.Value));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets all records
        /// </summary>
        public IEnumerable<T> Records
        {
            get
            {
                return this.Get(objectListName).Values;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Register a new object into records
        /// </summary>
        /// <param name="h">hash code</param>
        /// <param name="obj">object to register</param>
        /// <returns>true if registered</returns>
        public bool Add(int h, T obj)
        {
            bool added = false;
            if (!Exists(h))
            {
                added = true;
                this.Get(objectListName).Add(h, obj);
            }
            return added;
        }

        /// <summary>
        /// Ask to get a recorded object
        /// identified by a hash code
        /// </summary>
        /// <param name="hashCode">hash code</param>
        /// <returns>object</returns>
        public T Ask(int hashCode)
        {
            Dictionary<int, T> dict = this.Get(objectListName);
            if (dict.ContainsKey(hashCode))
            {
                return dict[hashCode];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Test if a specific hash code already exists
        /// </summary>
        /// <param name="hashCode">hash code</param>
        /// <returns>true if already exists</returns>
        public bool Exists(int hashCode)
        {
            return this.Get(objectListName).ContainsKey(hashCode);
        }

        #endregion
    }
}
