using PersistantModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Maths
{
    /// <summary>
    /// Tracer en deux dimensions
    /// </summary>
    public class TemplateTracer2D : ItemsPanelTemplate
    {

        #region Fields

        /// <summary>
        /// Système de coordonnées
        /// </summary>
        private CoordinateSystem sys;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor as a coordinate system
        /// </summary>
        public TemplateTracer2D()
        {
        }

        #endregion

    }
}
