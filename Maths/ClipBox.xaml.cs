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

        public static DependencyProperty TranslateXProperty = DependencyProperty.Register("TranslateX", typeof(Nullable<double>), typeof(ClipBox));

        public static DependencyProperty TranslateYProperty = DependencyProperty.Register("TranslateY", typeof(Nullable<double>), typeof(ClipBox));

        public static DependencyProperty ScaleXProperty = DependencyProperty.Register("ScaleX", typeof(Nullable<double>), typeof(ClipBox));

        public static DependencyProperty ScaleYProperty = DependencyProperty.Register("ScaleY", typeof(Nullable<double>), typeof(ClipBox));

        private Point startedMousePosition;
        private Boolean duringMove;

        public ClipBox()
        {
            InitializeComponent();
            this.t.MouseDown += mouseButtonDown;
            this.t.MouseUp += mouseButtonUp;
            this.t.MouseMove += mouseMove;
            this.t.MouseLeave += mouseLeave;
        }

        private void mouseLeave(object sender, MouseEventArgs e)
        {
            this.duringMove = false;
            this.Cursor = Cursors.Arrow;
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            if (this.duringMove)
            {
                Point p = e.GetPosition(this.t);
                double dx = this.startedMousePosition.X - p.X;
                double dy = this.startedMousePosition.Y - p.Y;
                Nullable<double> d = this.GetValue(TranslateXProperty) as Nullable<double>;
                if (d.HasValue && -(d - dx) < this.t.Width && (d - dx) < 0)
                    this.SetValue(TranslateXProperty, d - dx);
                d = this.GetValue(TranslateYProperty) as Nullable<double>;
                if (d.HasValue && -(d - dy) < this.t.Height && (d - dy) < 0)
                    this.SetValue(TranslateYProperty, d - dy);
                this.update();
            }
        }

        private void mouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.duringMove = false;
            this.Cursor = Cursors.Arrow;
        }

        private void mouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.startedMousePosition = e.GetPosition(this.t);
            this.duringMove = true;
            this.Cursor = Cursors.Hand;
        }

        private void increaseZoom_Click(object sender, RoutedEventArgs e)
        {
            Nullable<double> dx = this.GetValue(ScaleXProperty) as Nullable<double>;
            if (dx.HasValue)
            {
                this.SetValue(ScaleXProperty, dx.Value + 5);
            }
            Nullable<double> dy = this.GetValue(ScaleYProperty) as Nullable<double>;
            if (dy.HasValue)
            {
                this.SetValue(ScaleYProperty, dy.Value + 5);
            }
            this.update();
        }

        private void decreaseZoom_Click(object sender, RoutedEventArgs e)
        {
            Nullable<double> dx = this.GetValue(ScaleXProperty) as Nullable<double>;
            if (dx.HasValue && dx.Value > 5)
            {
                this.SetValue(ScaleXProperty, dx.Value - 5);
            }
            Nullable<double> dy = this.GetValue(ScaleYProperty) as Nullable<double>;
            if (dy.HasValue && dy.Value > 5)
            {
                this.SetValue(ScaleYProperty, dy.Value - 5);
            }
            this.update();
        }

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

        private void w_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetValue(TranslateXProperty, 0.0d);
            this.SetValue(TranslateYProperty, 0.0d);
            this.SetValue(ScaleXProperty, this.Width * 2.0d);
            this.SetValue(ScaleYProperty, this.Height * 2.0d);
            this.update();
        }

        private void update()
        {
            Nullable<double> dx = this.GetValue(TranslateXProperty) as Nullable<double>;
            Nullable<double> dy = this.GetValue(TranslateYProperty) as Nullable<double>;
            if (dx.HasValue && dy.HasValue)
                this.t.Margin = new Thickness(dx.Value, dy.Value, 0, 0);
            dx = this.GetValue(ScaleXProperty) as Nullable<double>;
            dy = this.GetValue(ScaleYProperty) as Nullable<double>;
            if (dx.HasValue)
                this.t.Width = dx.Value;
            if (dy.HasValue)
                this.t.Height = dy.Value;
        }

    }
}
