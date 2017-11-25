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
        /// Gets or sets the tex mode
        /// </summary>
        bool IsTexMode { get; set; }
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
        /// <summary>
        /// Insert text element into document list
        /// </summary>
        /// <param name="list">container</param>
        void InsertIntoDocument(System.Windows.Documents.List list);
    }
}
