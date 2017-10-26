using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    /// <summary>
    /// Concerns a document
    /// </summary>
    public interface IDocument
    {
        /// <summary>
        /// Gets true if equation is calculable
        /// </summary>
        bool IsCalculable { get; }
        /// <summary>
        /// Transforms equation object into a string representation
        /// </summary>
        /// <returns>string representation</returns>
        string ToString();
        /// <summary>
        /// Transforms equation object into a tex representation
        /// </summary>
        /// <returns>tex representation</returns>
        string ToTex();
    }
}
