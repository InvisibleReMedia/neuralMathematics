using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    /// <summary>
    /// Interface to handle a weight
    /// for an object identified by a unique hash code
    /// </summary>
    public interface IWeight
    {
        /// <summary>
        /// Gets the hash code value
        /// </summary>
        int HashCode { get; }
    }
}
