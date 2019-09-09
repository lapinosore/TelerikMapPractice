using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Map;

namespace TelerikWpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region ObservableCollection<TechMapPath> tMapPath        
        //[DefaultValue(null)]
        public ObservableCollection<TechMapPath> tMapPath
        {
            get { return (ObservableCollection<TechMapPath>)GetValue(tMapPathProperty); }
            set { SetValue(tMapPathProperty, value); }
        }
        public static readonly DependencyProperty tMapPathProperty =
            DependencyProperty.Register(
                "tMapPath",
                typeof(ObservableCollection<TechMapPath>),
                typeof(MainWindow),
                null);
        #endregion ObservableCollection<TechMapPath> tMapPath

        #region ObservableCollection<TechMapPath> tMapPath2        
        //[DefaultValue(null)]
        public ObservableCollection<TechMapPath> tMapPath2
        {
            get { return (ObservableCollection<TechMapPath>)GetValue(tMapPathProperty2); }
            set { SetValue(tMapPathProperty2, value); }
        }
        public static readonly DependencyProperty tMapPathProperty2 =
            DependencyProperty.Register(
                "tMapPath2",
                typeof(ObservableCollection<TechMapPath>),
                typeof(MainWindow),
                null);
        #endregion ObservableCollection<TechMapPath> tMapPath2


        public MainWindow()
        {
            InitializeComponent();

            //Information Layer DRAWING BUT NOT SELECTABLE
            tMapPath = new ObservableCollection<TechMapPath>();

            tMapPath.Add(new TechMapPath(new Location(-5, 10), new Location(0, 20), 1));
            tMapPath.Add(new TechMapPath(new Location(7, 8), new Location(0, 10), 2));


            //Visualization Layer NOT DRAWING BUT SELECTABLE
            tMapPath2 = new ObservableCollection<TechMapPath>();

            tMapPath2.Add(new TechMapPath(new Location(-3, 20), new Location(0, 20), 1));
            tMapPath2.Add(new TechMapPath(new Location(4, 5), new Location(2, 15), 2));

        }

        private void VisLayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Items: "+e.AddedItems.Count.ToString());
        }
    }
         
    public class TechMapPath : PathData
    {

        public TechMapPath(Location p_startPoint, Location p_endPoint, byte p_groupIndex = 1)
        {
            this.startPoint = p_startPoint;
            this.endPoint = p_endPoint;
            this.groupIndex = p_groupIndex;

            QuadraticBezierSegmentData mapQuadraticBezierSegment = new QuadraticBezierSegmentData() { Point1 = this.middlePoint, Point2 = p_endPoint };
            PathFigureData mapPathFigure = new PathFigureData() { StartPoint = p_startPoint, IsClosed = false };
            mapPathFigure.Segments.Add(mapQuadraticBezierSegment);

            PathGeometryData mapPathGeometry = new PathGeometryData();
            mapPathGeometry.Figures.Add(mapPathFigure);

            Brush stroke = Brushes.Transparent;
            this.Data = mapPathGeometry;
            switch (groupIndex)
            {
                case 1: { stroke = new SolidColorBrush(Colors.Red); break; }
                case 2: { stroke = new SolidColorBrush(Colors.Orange); break; }
                case 3: { stroke = new SolidColorBrush(Colors.Yellow); break; }
                case 4: { stroke = new SolidColorBrush(Colors.Green); break; }
                case 5: { stroke = new SolidColorBrush(Colors.LightBlue); break; }
                case 6: { stroke = new SolidColorBrush(Colors.Blue); break; }
                case 7: { stroke = new SolidColorBrush(Colors.Violet); break; }
                case 8: { stroke = new SolidColorBrush(Colors.Black); break; }
            }
            this.HighlightFill = new MapShapeFill() { Stroke = new SolidColorBrush(Colors.Green), StrokeThickness = 2 };
            this.ShapeFill = new MapShapeFill() { Stroke = stroke, StrokeThickness = 10 };
            this.SelectedFill = new MapShapeFill() { Stroke = Brushes.Black, StrokeThickness = 10 };
        }



        public Location startPoint { get; set; }
        public Location endPoint { get; set; }

        private byte groupIndex;
        private Location middlePoint
        {
            get
            {
                return CalculateMiddleLocation(this.startPoint, this.endPoint, this.groupIndex);
            }
        }

        public static Location CalculateMiddleLocation(Location p_start, Location p_end, double p_tension)
        {
            float delta = -8;
            switch (p_tension)
            {
                case 1: { delta = -50; break; } // Red
                case 2: { delta = -16; break; } // Orange
                case 3: { delta = -10; break; } // Yellow
                case 4: { delta = -7; break; }   // Green)
                                                
                case 5: { delta = 7; break; }   // LightB
                case 6: { delta = 10; break; }  // Blue);
                case 7: { delta = 16; break; }  // Violet
                case 8: { delta = 50; break; }  // Black
                default: { delta = 100; break; }
            }


            Location controlPoint = new Location();
            controlPoint.Longitude = (p_start.Longitude + p_end.Longitude) / 2;
            controlPoint.Latitude = Math.Max(p_start.Latitude, p_end.Latitude) +
                                    Math.Abs(p_start.Longitude - p_end.Longitude) / delta;
            return controlPoint;
        }
    }


}
