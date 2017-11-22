using PersistantModel;
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
        private uint maxDepth;

        /// <summary>
        /// y = Function(x)
        /// </summary>
        private Arithmetic function;

        /// <summary>
        /// x = Function(y)
        /// </summary>
        private Arithmetic[] inverted;

        /// <summary>
        /// Bounds of coordinates
        /// </summary>
        private MovingCoordinates bounds;

        /// <summary>
        /// Points
        /// </summary>
        private MovingVector points;

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
            this.maxDepth = depth;
            this.pixels = p;
            this.visualLocation = new Point(0.0d, 0.0d);
            this.visualSize = new Size(200.0d, 200.0d);
            this.areas = this.ComputeCuttedAreas();
            this.imageSize = new Size(this.areas[0, 0].Width * this.columnSize, this.areas[0, 0].Height * this.rowSize);
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
            return new Size(this.ComputeImageWidth(this.maxDepth + 1), this.ComputeImageHeight(this.maxDepth + 1));
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
            for(uint indexDepth = maxDepth; indexDepth > 0; --indexDepth)
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
            for (uint indexDepth = maxDepth; indexDepth > 0; --indexDepth)
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
            int depth = Convert.ToInt32(this.maxDepth);
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
        /// Draw point
        /// </summary>
        /// <param name="owner">dispatcher</param>
        /// <param name="container">content control</param>
        /// <param name="dg">drawing group</param>
        private void DrawPoints(Dispatcher owner, ContentControl container, DrawingGroup dg)
        {
            owner.Invoke(() =>
            {
                DrawingContext dc = dg.Append();
                Size size = this.areas[2, 2];
                this.points = this.bounds.GenerateMove();
                foreach (double w in this.points.Values)
                {
                    try
                    {
                        this.function.Let("x", w);
                        double y = Convert.ToDouble(this.function.Calculate(true));
                        Coordinates p = new Coordinates(w, y);
                        if (p.Euclidian.Value >= this.bounds.Vector.From.Euclidian.Value && p.Euclidian.Value < this.bounds.Vector.To.Euclidian.Value)
                        {
                            p = this.bounds.Place(p, this.imageSize.Width, this.imageSize.Height);
                            this.DrawPoint(dc, p.Value, p.Euclidian.Value, size.Width, size.Height);
                        }
                    }
                    catch (FormatException) { }
                }
                dc.Close();
            });
        }

        /// <summary>
        /// Draw grid
        /// </summary>
        /// <param name="owner">dispatcher</param>
        /// <param name="container">content control</param>
        /// <param name="dg">drawing group</param>
        /// <param name="depth">depth</param>
        /// <param name="start">starting point</param>
        private void DrawGrid(Dispatcher owner, ContentControl container, DrawingGroup dg, uint depth, Point start)
        {

            int level = Convert.ToInt32(depth + 1);

            DrawingContext dc = dg.Append();
            double size = (3 - level) / 3.0d;
            Size first = this.areas[depth, depth];

            Point current = new Point(start.X + first.Width / 2.0d, start.Y + first.Height / 2.0d);
            Point w = new Point(current.X, start.Y);
            Point h = new Point(start.X, current.Y);
            Size dw = new Size(0.0d, first.Height * (double)this.rowSize);
            Size dh = new Size(first.Width * (double)this.columnSize, 0.0d);

            //dc.DrawRectangle(Brushes.Beige, null, new Rect(start.X, start.Y, first.Width, first.Height));

            for (int indexRow = 0; indexRow < this.rowSize; ++indexRow)
            {
                for (int indexColumn = 0; indexColumn < this.columnSize; ++indexColumn)
                {
                    dc.DrawLine(new Pen(Brushes.Red, size), new Point(w.X, w.Y), new Point(w.X + dw.Width, w.Y + dw.Height));
                    dc.DrawLine(new Pen(Brushes.Red, size), new Point(h.X, h.Y), new Point(h.X + dh.Width, h.Y + dh.Height));
                    if (level < 2)
                        this.TaskDrawGrid(owner, container, dg, depth + 1, new Point(start.X + first.Width * indexColumn, start.Y + first.Height * indexRow));
                    w.X += first.Width;
                }
                w.X = current.X;
                h.Y += first.Height;
            }
            dc.Close();

        }

        /// <summary>
        /// Task Draw grid
        /// </summary>
        /// <param name="owner">owner dispatcher</param>
        /// <param name="container">container</param>
        /// <param name="dg">drawing group</param>
        private void TaskDrawPoints(Dispatcher owner, ContentControl container, DrawingGroup dg)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    this.DrawPoints(owner, container, dg);
                }
                catch (System.Threading.Tasks.TaskCanceledException) { }

            }).ContinueWith(t =>
            {
                try
                {
                    owner.Invoke(() =>
                    {
                        Rectangle re = container.Content as Rectangle;
                        re.Fill = new DrawingBrush(this.drawing);
                    });

                }
                catch (System.Threading.Tasks.TaskCanceledException) { }
            });
        }

        /// <summary>
        /// Task Draw grid
        /// </summary>
        /// <param name="owner">owner dispatcher</param>
        /// <param name="container">container</param>
        /// <param name="dg">drawing group</param>
        /// <param name="depth">depth</param>
        /// <param name="start">starting point</param>
        private void TaskDrawGrid(Dispatcher owner, ContentControl container, DrawingGroup dg, uint depth, Point start)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    owner.Invoke(() =>
                    {
                        this.DrawGrid(owner, container, dg, depth, start);
                    });
                }
                catch (System.Threading.Tasks.TaskCanceledException) { }

            }).ContinueWith(t =>
            {
                try
                {
                    owner.Invoke(() =>
                    {
                        Rectangle re = container.Content as Rectangle;
                        re.Fill = new DrawingBrush(this.drawing);
                    });

                }
                catch (System.Threading.Tasks.TaskCanceledException) { }
            });
        }

        /// <summary>
        /// A l'aide de la taille de l'image,
        /// du nombre de colonnes et du nombres de lignes,
        /// des bornes de la graduation et de
        /// la fonction mathématique, dessiner le graphique
        /// </summary>
        /// <param name="attach">content control</param>
        private DrawingGroup Draw(ContentControl attach)
        {
            DrawingGroup dg = new DrawingGroup();

            DrawingContext dc = dg.Open();
            dc.DrawRectangle(Brushes.Beige, new Pen(Brushes.Red, 1.0), new Rect(0, 0, this.imageSize.Width, this.imageSize.Height));
            dc.Close();

            this.TaskDrawGrid(Dispatcher.CurrentDispatcher, attach, dg, 0, new Point(0.0d, 0.0d));
            this.TaskDrawPoints(Dispatcher.CurrentDispatcher, attach, dg);

            return dg;
            
        }

        /// <summary>
        /// Draw a point onto an image
        /// </summary>
        /// <param name="dc">drawing context</param>
        /// <param name="x">x axis</param>
        /// <param name="y">y axis</param>
        /// <param name="pixelWidth">pixel width</param>
        /// <param name="pixelHeight">pixel height</param>
        private void DrawPoint(DrawingContext dc, double x, double y, double pixelWidth, double pixelHeight)
        {
            dc.DrawRectangle(Brushes.OrangeRed, null, new Rect(x, this.imageSize.Height - y - pixelHeight, pixelWidth, pixelHeight));
        }

        /// <summary>
        /// Set function(x) and rec(f)
        /// </summary>
        /// <param name="f">arithmetic function</param>
        /// <param name="inv">inverted</param>
        public void SetFunction(Arithmetic f, params Arithmetic[] inv)
        {
            this.function = f.Clone() as Arithmetic;
            this.inverted = new Arithmetic[inv.Length];
            for(int index = 0; index < inv.Length; ++index)
            {
                this.inverted[index] = inv[index].Clone() as Arithmetic;
            }
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
                        this.drawing = this.Draw(attach);
                        Rectangle r = new Rectangle();
                        r.Fill = new DrawingBrush(this.drawing);
                        r.Width = this.imageSize.Width;
                        r.Height = this.imageSize.Height;
                        r.RenderSize = this.imageSize;
                        r.CacheMode = new BitmapCache();
                        attach.Content = r;
                        this.boundsChanged = false;
                    }
                    Rectangle re = attach.Content as Rectangle;
                    TransformGroup tg = new TransformGroup();
                    ScaleTransform s = new ScaleTransform(scale, scale);
                    tg.Children.Add(s);
                    re.LayoutTransform = tg;
                });
            });
        }

        #endregion

    }
}
