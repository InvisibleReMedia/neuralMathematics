using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Maths
{
    /// <summary>
    /// Logique d'interaction pour ClipBox.xaml
    /// </summary>
    public partial class ClipBox : UserControl
    {

        /// <summary>
        /// Translation on x axis to show a partial of picture
        /// </summary>
        public static DependencyProperty TranslateXProperty = DependencyProperty.Register("TranslateX", typeof(Nullable<double>), typeof(ClipBox));

        /// <summary>
        /// Translation on y axis to show a partial of picture
        /// </summary>
        public static DependencyProperty TranslateYProperty = DependencyProperty.Register("TranslateY", typeof(Nullable<double>), typeof(ClipBox));

        /// <summary>
        /// Scale on x axis to zoom into picture
        /// </summary>
        public static DependencyProperty ScaleXProperty = DependencyProperty.Register("ScaleX", typeof(Nullable<double>), typeof(ClipBox));

        /// <summary>
        /// Scale on y axis to zoom into picture
        /// </summary>
        public static DependencyProperty ScaleYProperty = DependencyProperty.Register("ScaleY", typeof(Nullable<double>), typeof(ClipBox));

        /// <summary>
        /// Point where starting mouse moving arround
        /// </summary>
        private Point startedMousePosition;
        /// <summary>
        /// Enable mouse moving control
        /// </summary>
        private Boolean duringMove;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClipBox()
        {
            InitializeComponent();
            this.t.MouseDown += mouseButtonDown;
            this.t.MouseUp += mouseButtonUp;
            this.t.MouseMove += mouseMove;
            this.t.MouseLeave += mouseLeave;
            this.Width = this.t.Source.Width;
            this.Height = this.t.Source.Height;
        }

        /// <summary>
        /// mouse leave event
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">mouse args</param>
        private void mouseLeave(object sender, MouseEventArgs e)
        {
            this.duringMove = false;
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// mouse move arround control
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">mouse args</param>
        private void mouseMove(object sender, MouseEventArgs e)
        {
            if (this.duringMove)
            {
                Point p = e.GetPosition(this.t);
                double dx = this.startedMousePosition.X - p.X;
                double dy = this.startedMousePosition.Y - p.Y;
                Nullable<double> d = this.GetValue(TranslateXProperty) as Nullable<double>;
                if (d.HasValue && -(d - dx) < (this.t.Width - this.Width) && (d - dx) < 0)
                    this.SetValue(TranslateXProperty, d - dx);
                d = this.GetValue(TranslateYProperty) as Nullable<double>;
                if (d.HasValue && -(d - dy) < (this.t.Height - this.Height) && (d - dy) < 0)
                    this.SetValue(TranslateYProperty, d - dy);
                this.update();
            }
        }

        /// <summary>
        /// When the mouse button was pressed up
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">mouse args</param>
        private void mouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.duringMove = false;
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// When the mouse button was pressed down
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">mouse args</param>
        private void mouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.startedMousePosition = e.GetPosition(this.t);
            this.duringMove = true;
            this.Cursor = Cursors.ScrollAll;
        }

        /// <summary>
        /// When the zoom value increases
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">args</param>
        private void increaseZoom_Click(object sender, RoutedEventArgs e)
        {
            Nullable<double> dx = this.GetValue(ScaleXProperty) as Nullable<double>;
            if (dx.HasValue)
            {
                if (dx.Value < this.t.Width * 4.0d)
                    this.SetValue(ScaleXProperty, dx.Value + 5);
                else
                    this.SetValue(ScaleXProperty, this.t.Width * 4.0d);
            }
            Nullable<double> dy = this.GetValue(ScaleYProperty) as Nullable<double>;
            if (dy.HasValue)
            {
                if (dy.Value < this.t.Height * 4.0d)
                    this.SetValue(ScaleYProperty, dy.Value + 5);
                else
                    this.SetValue(ScaleYProperty, this.t.Height * 4.0d);
            }
            this.update();
        }

        /// <summary>
        /// When the zoom value decreases
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">args</param>
        private void decreaseZoom_Click(object sender, RoutedEventArgs e)
        {
            Nullable<double> dx = this.GetValue(ScaleXProperty) as Nullable<double>;
            if (dx.HasValue)
            {
                if (dx.Value > this.t.Width * 2.0d)
                {
                    this.SetValue(ScaleXProperty, dx.Value - 5);
                    this.update();
                    Nullable<double> d = this.GetValue(TranslateXProperty) as Nullable<double>;
                    if (d.HasValue && -d >= (this.t.Width - this.Width))
                        this.SetValue(TranslateXProperty, -(this.t.Width - this.Width));
                }
                else
                {
                    this.SetValue(ScaleXProperty, this.t.Width * 2.0d);
                    this.update();
                    Nullable<double> d = this.GetValue(TranslateXProperty) as Nullable<double>;
                    if (d.HasValue && -d >= (this.t.Width - this.Width))
                        this.SetValue(TranslateXProperty, -(this.t.Width - this.Width));
                }
            }
            Nullable<double> dy = this.GetValue(ScaleYProperty) as Nullable<double>;
            if (dy.HasValue)
            {
                if (dy.Value > this.t.Height * 2.0d)
                {
                    this.SetValue(ScaleYProperty, dy.Value - 5);
                    this.update();
                    Nullable<double> d = this.GetValue(TranslateYProperty) as Nullable<double>;
                    if (d.HasValue && -d >= (this.t.Height - this.Height))
                        this.SetValue(TranslateYProperty, -(this.t.Height - this.Height));
                }
                else
                {
                    this.SetValue(ScaleYProperty, this.t.Height * 2.0d);
                    this.update();
                    Nullable<double> d = this.GetValue(TranslateYProperty) as Nullable<double>;
                    if (d.HasValue && -d >= (this.t.Height - this.Height))
                        this.SetValue(TranslateYProperty, -(this.t.Height - this.Height));
                }
            }
            this.update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moveLeft_Click(object sender, RoutedEventArgs e)
        {
            Nullable<double> d = this.GetValue(TranslateXProperty) as Nullable<double>;
            if (d.HasValue && -d < this.t.Width)
                this.SetValue(TranslateXProperty, d - 10);
            this.update();
        }

        private void moveRight_Click(object sender, RoutedEventArgs e)
        {
            Nullable<double> d = this.GetValue(TranslateXProperty) as Nullable<double>;
            if (d.HasValue && d < 0)
                this.SetValue(TranslateXProperty, d + 10);
            this.update();
        }

        /// <summary>
        /// When ui was loaded
        /// </summary>
        /// <param name="sender">ui origin</param>
        /// <param name="e">args</param>
        private void w_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetValue(TranslateXProperty, 0.0d);
            this.SetValue(TranslateYProperty, 0.0d);
            this.SetValue(ScaleXProperty, this.Width * 2.0d);
            this.SetValue(ScaleYProperty, this.Height * 2.0d);
            this.update();
        }

        /// <summary>
        /// When ui has to be updated (zoom or moving changes)
        /// </summary>
        private void update()
        {
            Nullable<double> ix = this.GetValue(TranslateXProperty) as Nullable<double>;
            Nullable<double> iy = this.GetValue(TranslateYProperty) as Nullable<double>;
            if (ix.HasValue && iy.HasValue)
                this.t.Margin = new Thickness(ix.Value, iy.Value, 0, 0);
            Nullable<double> dx = this.GetValue(ScaleXProperty) as Nullable<double>;
            Nullable<double> dy = this.GetValue(ScaleYProperty) as Nullable<double>;
            if (dx.HasValue)
                this.t.Width = dx.Value;
            if (dy.HasValue)
                this.t.Height = dy.Value;
        }

    }
}
