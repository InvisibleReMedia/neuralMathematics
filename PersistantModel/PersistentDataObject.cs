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
    class PersistentDataObject
    {
        /// <summary>
        /// Field to store data information to serialize
        /// </summary>
        private Dictionary<string, dynamic> dict;

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
                    o = bf.Deserialize(file.Open(FileMode.Open));
                } catch(SerializationException)
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
                    } else
                    {
                        return false;
                    }
                } else
                {
                    return false;
                }
            } else
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
                PersistentDataObject p;
                BinaryFormatter bf = new BinaryFormatter();
                Object o;

                try
                {
                    bf.Serialize(f, data);
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
