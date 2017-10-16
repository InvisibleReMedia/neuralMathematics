using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Maths
{
    class GenericNeurone : INeurone
    {

        #region Neurone interface

        /// <summary>
        /// Identifiant unique
        /// </summary>
        public uint Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Nom du neurone
        /// </summary>
        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Type du neurone
        /// </summary>
        public INeuroneType Type
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Exécute le travail du neurone
        /// </summary>
        public void Exec()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Apprentissage pour la construction
        /// des neurones
        /// </summary>
        public void Learn()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
