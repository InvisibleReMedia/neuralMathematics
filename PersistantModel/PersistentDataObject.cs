using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PersistantModel
{
    /// <summary>
    /// Classe contenant la séquence
    /// de sérialisation/déserialisation
    /// </summary>
    [Serializable]
    public class PersistentDataObject
    {
        /// <summary>
        /// Field to store data information to serialize
        /// </summary>
        private Dictionary<string, dynamic> dict;


        /// <summary>
        /// Default constructor
        /// </summary>
        protected PersistentDataObject()
        {
            this.dict = new Dictionary<string, dynamic>();
        }

        /// <summary>
        /// Data to be used
        /// </summary>
        protected Dictionary<string, dynamic> Data
        {
            get
            {
                return this.dict;
            }
        }

        /// <summary>
        /// Lecture d'un document
        /// </summary>
        /// <param name="file">information de fichier</param>
        /// <param name="result">objet obtenu</param>
        /// <returns>true if success</returns>
        public static bool Load(FileInfo file, out PersistentDataObject result)
        {
            result = null;
            if (file.Exists)
            {
                PersistentDataObject p;
                BinaryFormatter bf = new BinaryFormatter();
                Object o;

                try
                {
                    using(Stream s = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        o = bf.Deserialize(s);
                        s.Close();
                    }

                }
                catch (SerializationException)
                {
                    return false;
                }

                if (o != null)
                {
                    p = o as PersistentDataObject;
                    if (p != null)
                    {
                        result = p;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Sauvegarde d'un document
        /// </summary>
        /// <param name="file">information de fichier</param>
        /// <param name="data">objet à sérialiser</param>
        /// <returns>true if success</returns>
        public static bool Save(FileInfo file, PersistentDataObject data)
        {
            using(FileStream f = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();

                try
                {
                    bf.Serialize(f, data);
                    f.Close();
                    return true;
                }
                catch (SerializationException)
                {
                    return false;
                }

            }
        }

    }
}
