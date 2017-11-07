using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Maths
{
    /// <summary>
    /// Distributed Tracer
    /// Distribute squares parts to
    /// multiple tasks
    /// Each task traces points
    /// </summary>
    public class DistributedTracer2D
    {

        #region Fields

        /// <summary>
        /// profondeur de tâches
        /// </summary>
        private uint depth;

        /// <summary>
        /// Bounds of coordinates
        /// </summary>
        private MovingCoordinates bounds;

        /// <summary>
        /// Number of columns
        /// </summary>
        private uint columnSize;

        /// <summary>
        /// Number of lines
        /// </summary>
        private uint rowSize;

        /// <summary>
        /// Pixelization on graphics
        /// </summary>
        private Size pixels;

        /// <summary>
        /// Size of the resulting image
        /// </summary>
        private Size imageSize;

        /// <summary>
        /// Taille du visual
        /// </summary>
        private Size visualSize;

        /// <summary>
        /// Areas of each image part
        /// </summary>
        private Size[,] areas;

        private Random ra;

        #endregion


        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="mc">Moving Coordinates</param>
        /// <param name="column">column size</param>
        /// <param name="row">row size</param>
        /// <param name="depth">depth</param>
        /// <param name="p">pixels size</param>
        public DistributedTracer2D(MovingCoordinates mc, uint column, uint row, uint depth, Size p)
        {
            this.bounds = mc;
            this.columnSize = column;
            this.rowSize = row;
            this.depth = depth;
            this.pixels = p;
            this.imageSize = this.ComputeImageSize();
            this.areas = this.ComputeCuttedAreas();
            this.visualSize = new Size(200.0d, 200.0d);
            ra = new Random();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Taille de l'image (en nombre de pixels)
        /// </summary>
        public Size ImageSize
        {
            get
            {
                return this.imageSize;
            }
        }

        /// <summary>
        /// Gets area set
        /// </summary>
        public Size[,] Areas
        {
            get
            {
                return this.areas;
            }
        }

        /// <summary>
        /// Gets or sets visual size
        /// </summary>
        public Size VisualSize
        {
            get
            {
                return this.visualSize;
            }
            set
            {
                this.visualSize = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Function to compute the height of image
        /// </summary>
        /// <param name="depth">depth image definition</param>
        /// <returns>total height of image</returns>
        protected double ComputeImageHeight(uint depth)
        {
            if (depth > 0)
            {
                return this.ComputeImageHeight(depth - 1) * this.rowSize;
            }
            else
            {
                return this.pixels.Height;
            }
        }

        /// <summary>
        /// Function to compute the width of image
        /// </summary>
        /// <param name="depth">depth image definition</param>
        /// <returns>total width of image</returns>
        protected double ComputeImageWidth(uint depth)
        {
            if (depth > 0)
            {
                return this.ComputeImageWidth(depth - 1) * this.columnSize;
            }
            else
            {
                return this.pixels.Width;
            }
        }


        /// <summary>
        /// Computes the size of image
        /// </summary>
        /// <returns>total size of image</returns>
        protected Size ComputeImageSize()
        {
            return new Size(this.ComputeImageWidth(this.depth), this.ComputeImageHeight(this.depth));
        }

        /// <summary>
        /// Compute the exact height of the total image height in pixels
        /// </summary>
        /// <returns>image height in pixels</returns>
        protected double ComputeHeightSteps()
        {
            // nombre total de lignes
            double rowTotalCount = (this.bounds.Vector.To.Value - this.bounds.Vector.From.Value) / this.bounds.Steps.Value;
            // partie décimale restante
            double partRowTotalCount = rowTotalCount - Math.Ceiling(rowTotalCount);
            rowTotalCount = Math.Ceiling(rowTotalCount);
            return rowTotalCount * this.pixels.Height;
        }

        /// <summary>
        /// Compute the exact height of the total image height in pixels
        /// </summary>
        /// <returns>image height in pixels</returns>
        protected double ComputeWidthSteps()
        {
            // nombre total de colonnes
            double columnTotalCount = (this.bounds.Vector.Euclidian.To.Value - this.bounds.Vector.Euclidian.From.Value) / this.bounds.Steps.Euclidian.Value;
            // partie décimale restante
            double partColumnTotalCount = columnTotalCount - Math.Ceiling(columnTotalCount);
            columnTotalCount = Math.Ceiling(columnTotalCount);
            return columnTotalCount * this.pixels.Width;
        }

        /// <summary>
        /// Compute each area size for recursive image uniform height
        /// </summary>
        /// <returns>list of area</returns>
        protected IEnumerable<double> ComputeCuttedHeightAreas()
        {
            double height = this.ComputeHeightSteps();
            double dividedHeight = height;
            List<double> dividedList = new List<double>();
            for(uint indexDepth = depth; indexDepth > 0; --indexDepth)
            {
                dividedList.Add(dividedHeight);
                dividedHeight = dividedHeight / (double)this.rowSize;
            }
            return dividedList;
        }

        /// <summary>
        /// Compute each area size for recursive image uniform width
        /// </summary>
        /// <returns>list of area</returns>
        protected IEnumerable<double> ComputeCuttedWidthAreas()
        {
            double width = this.ComputeWidthSteps();
            double dividedWidth = width;
            List<double> dividedList = new List<double>();
            for (uint indexDepth = depth; indexDepth > 0; --indexDepth)
            {
                dividedList.Add(dividedWidth);
                dividedWidth = dividedWidth / (double)this.columnSize;
            }
            return dividedList;
        }

        /// <summary>
        /// Compute a list of areas
        /// </summary>
        /// <returns>size of each area</returns>
        protected Size[,] ComputeCuttedAreas()
        {
            int depth = Convert.ToInt32(this.depth);
            Size[,] tab = new Size[depth, depth];
            IEnumerable<double> widthAreas = this.ComputeCuttedWidthAreas();
            IEnumerable<double> heightAreas = this.ComputeCuttedHeightAreas();
            for(int indexWidthDepth = 0; indexWidthDepth < depth; ++indexWidthDepth)
            {
                for(int indexHeightDepth = 0; indexHeightDepth < depth; ++indexHeightDepth)
                {
                    double width = widthAreas.ElementAt(indexWidthDepth);
                    double height = heightAreas.ElementAt(indexHeightDepth);
                    tab[indexHeightDepth, indexWidthDepth] = new Size(width, height);
                }
            }
            return tab;
        }

        /// <summary>
        /// A l'aide de la taille de l'image,
        /// du nombre de colonnes et du nombres de lignes,
        /// des bornes de la graduation et de
        /// la fonction mathématique, créer une image
        /// </summary>
        public Grid ThumbnailImage()
        {
            Size first = this.ComputeCuttedAreas()[0, 0];
            Grid grid = new Grid();
            grid.Width = first.Width * this.columnSize;
            grid.Height = first.Height * this.rowSize;
            grid.Background = Brushes.Green;
            for(int indexRow = 0; indexRow < this.rowSize; ++indexRow)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(first.Height);
                grid.RowDefinitions.Add(rd);
            }
            for (int indexColumn = 0; indexColumn < this.columnSize; ++indexColumn)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(first.Width);
                grid.ColumnDefinitions.Add(cd);
            }
            for (int indexRow = 0; indexRow < this.rowSize; ++indexRow)
            {
                for (int indexColumn = 0; indexColumn < this.columnSize; ++indexColumn)
                {
                    Rectangle r = new Rectangle();
                    Grid.SetRow(r, indexRow);
                    Grid.SetColumn(r, indexColumn);
                    r.Width = first.Width;
                    r.Height = first.Height;
                    DrawingBrush db = new DrawingBrush();
                    db.Stretch = Stretch.Fill;
                    LineGeometry[] lines = new LineGeometry[2];

                    lines[0] = new LineGeometry(new Point(first.Width / 2.0d, 0.0d), new Point(first.Width / 2.0d, first.Height));
                    lines[1] = new LineGeometry(new Point(0.0d, first.Height / 2.0d), new Point(first.Width, first.Height / 2.0d));

                    GeometryGroup g = new GeometryGroup();
                    for(int i = 0; i < 2; ++i) g.Children.Add(lines[i]);
                    GeometryDrawing gd = new GeometryDrawing();
                    gd.Geometry = g;
                    gd.Pen = new Pen(Brushes.Red, 1.0d);
                    db.Drawing = gd;
                    r.Fill = new SolidColorBrush(Color.FromArgb(255, Convert.ToByte(ra.Next(255)),
                                                                Convert.ToByte(ra.Next(255)),
                                                                Convert.ToByte(ra.Next(255))));
                    grid.Children.Add(r);
                }
            }
            return grid;
            
        }

        #endregion

    }
}
