using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    /// <summary>
    /// Interface to get a variable
    /// </summary>
    public interface IVariable
    {
        /// <summary>
        /// Gets the name of this variable
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets or sets the value of this variable
        /// </summary>
        dynamic Value { get; set; } 
    }
}
