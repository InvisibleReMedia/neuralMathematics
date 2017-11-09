using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

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
        /// Position
        /// </summary>
        private Point visualLocation;

        /// <summary>
        /// Taille du visual
        /// </summary>
        private Size visualSize;

        /// <summary>
        /// scale for zooming
        /// </summary>
        private double visualScale;

        /// <summary>
        /// Areas of each image part
        /// </summary>
        private Size[,] areas;

        /// <summary>
        /// Random numbers
        /// </summary>
        private Random ra;

        /// <summary>
        /// if bounds has changed
        /// </summary>
        private bool boundsChanged;

        /// <summary>
        /// Drawing object to handle the curve
        /// </summary>
        private DrawingGroup drawing;

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
            this.boundsChanged = true;
            this.columnSize = column;
            this.rowSize = row;
            this.depth = depth;
            this.pixels = p;
            this.visualLocation = new Point(0.0d, 0.0d);
            this.visualSize = new Size(200.0d, 200.0d);
            this.imageSize = this.ComputeImageSize();
            this.areas = this.ComputeCuttedAreas();
            ra = new Random();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Position de l'image (en nombre de pixels)
        /// </summary>
        public Point VisualLocation
        {
            get
            {
                return this.visualLocation;
            }
        }

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
        }

        /// <summary>
        /// Gets or sets visual scale
        /// </summary>
        public double VisualScale
        {
            get
            {
                return this.visualScale;
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
        public DrawingGroup Draw()
        {
            DrawingGroup dg = new DrawingGroup();
            DrawingContext dc = dg.Open();

            Size first = this.ComputeCuttedAreas()[0, 0];
            // Background
            dc.DrawRectangle(Brushes.GreenYellow, new Pen(Brushes.Black, 1), new Rect(0, 0, first.Width * this.columnSize, first.Height * this.rowSize));

            for (int indexRow = 0; indexRow < this.rowSize; ++indexRow)
            {
                for (int indexColumn = 0; indexColumn < this.columnSize; ++indexColumn)
                {
                    dc.DrawRectangle(new SolidColorBrush(Color.FromArgb(255, Convert.ToByte(ra.Next(255)),
                                                                Convert.ToByte(ra.Next(255)),
                                                                Convert.ToByte(ra.Next(255)))),
                                                         new Pen(Brushes.Black, 1.0),
                                                         new Rect(first.Width * (double)indexColumn, first.Height * (double)indexRow, first.Width, first.Height));
                    //DrawingBrush db = new DrawingBrush();
                    //db.Stretch = Stretch.Fill;
                    //LineGeometry[] lines = new LineGeometry[2];

                    //lines[0] = new LineGeometry(new Point(first.Width / 2.0d, 0.0d), new Point(first.Width / 2.0d, first.Height));
                    //lines[1] = new LineGeometry(new Point(0.0d, first.Height / 2.0d), new Point(first.Width, first.Height / 2.0d));

                    //GeometryGroup g = new GeometryGroup();
                    //for(int i = 0; i < 2; ++i) g.Children.Add(lines[i]);
                    //GeometryDrawing gd = new GeometryDrawing();
                    //gd.Geometry = g;
                    //gd.Pen = new Pen(Brushes.Red, 1.0d);
                    //db.Drawing = gd;
                }
            }
            dc.Close();
            return dg;
            
        }

        /// <summary>
        /// Récupère une image du visual créé
        /// </summary>
        /// <param name="left">translation horizontale</param>
        /// <param name="right">translation verticale</param>
        /// <param name="desiredWidth">largeur demandée</param>
        /// <param name="desiredHeight">hauteur demandée</param>
        /// <param name="scale">scale value</param>
        /// <param name="attach">container</param>
        public void GetThumbnail(double left, double right, double desiredWidth, double desiredHeight, double scale, ContentControl attach)
        {
            this.visualLocation = new Point(left, right);
            this.visualSize = new Size(desiredWidth, desiredHeight);
            this.visualScale = scale;
            Dispatcher owner = Dispatcher.CurrentDispatcher;
            Task.Factory.StartNew(() =>
            {
                owner.Invoke(() => {
                    if (this.boundsChanged)
                    {
                        this.drawing = this.Draw();
                        Rectangle r = new Rectangle();
                        r.RenderSize = this.drawing.Bounds.Size;
                        r.Fill = new DrawingBrush(this.drawing);
                        attach.Content = r;
                        this.boundsChanged = false;
                    }
                    else
                    {
                    }
                });
            });
        }

        #endregion

    }
}
