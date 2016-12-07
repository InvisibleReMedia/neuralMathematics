using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    /// <summary>
    /// Type de neurone
    /// </summary>
    public enum INeuroneType
    {
        /// <summary>
        /// Une constante numérique
        /// </summary>
        Constant,
        /// <summary>
        /// Un opérateur arithmétique
        /// </summary>
        Operator,
        /// <summary>
        /// Variable arithmétique
        /// </summary>
        Variable
    }
}
