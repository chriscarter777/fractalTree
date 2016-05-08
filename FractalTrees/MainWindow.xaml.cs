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

namespace FractalTrees
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnDraw_Click(object sender, RoutedEventArgs e)
        {
            int gen = int.Parse(txtNumGenerations.Text);
            double ang = double.Parse(txtAngle.Text);
            double len = double.Parse(txtProportionateLength.Text);
            CreateFractalTree(gen, ang, len);
        }

        private void CreateFractalTree(int gen, double ang, double len)
        {
            int generations = gen;
            double relativeAngle = ang;
            double relativeLength = len;

            //starting parameters
            double segLength = 120;
            double ang0 = 0;
            int x0 = 500;
            int y0 = 800;
            int wt0 = generations;

            //calculate tree dimensions and instantiate the array of line segments
            int numLeaves = (int)(Math.Pow(2, generations));
            int numSegs = numLeaves * generations;
            Segment[,] segments = new Segment[generations + 1,numLeaves];
            Console.WriteLine("segments is initialized to " + segments.GetUpperBound(0) + " by " + segments.GetUpperBound(1));

            //initialize the original segment
            Segment initSegment = new Segment(segLength, ang0, relativeAngle, x0, y0, wt0);
            segments[0, 0] = initSegment;

            //loop through each generation
            for (int generation=0; generation <= generations; generation++)
            {
                //set attributes for the child generation
                segLength *= relativeLength;
                int segWeight = generations - generation;

                //loop through each segment in this generation... 
                int i = 0;
                int childGenerationcounter = 0;
                while (i <= (numLeaves - 1) && segments[generation, i] != null)
                {
                    Segment thisSegment = segments[generation, i];
                    //...draw it 
                    Console.WriteLine("DRAWING--Generation:" + generation + "/ Segment:" + i);
                    Console.WriteLine(thisSegment.ToString());
                    DrawSegment(thisSegment);
                    //...and initialize its two child segments
                    if (generation < generations)
                    {
                        segments[generation + 1, childGenerationcounter] = new Segment(segLength, thisSegment.ExitAngleLeft, relativeAngle, thisSegment.X2, thisSegment.Y2, segWeight);
                        Console.WriteLine("Created " + "[" + (generation + 1) + "] [" + childGenerationcounter + "]");
                        childGenerationcounter ++;
                        segments[generation + 1, childGenerationcounter] = new Segment(segLength, thisSegment.ExitAngleRight, relativeAngle, thisSegment.X2, thisSegment.Y2, segWeight);
                        Console.WriteLine("Created " + "[" + (generation + 1) + "] [" + childGenerationcounter + "]");
                        childGenerationcounter++;
                    }
                    i ++;
                }
                Console.WriteLine("Finished processing for generation " + generation);
                Console.WriteLine("-----------------------------------------");
            }
            Console.WriteLine("Finished processing");
        }

        public void DrawSegment(Segment segment)
        {
            Line line = new Line();
            line.X1 = segment.X1;
            line.Y1 = segment.Y1;
            line.X2 = segment.X2;
            line.Y2 = segment.Y2;

            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.Black;

            // Set Line's width and color
            line.StrokeThickness = segment.Weight;
            line.Stroke = brush;

            // Add line to the Grid.
            DrawSpace.Children.Add(line);
        }
    }

    public class Segment
    {
        private double length;
        private double angle;
        private double relAngle;
        private int x1;
        private int y1;
        private int weight;

        public Segment(double len, double ang, double relAng, int x, int y, int wt)
        {
            length = len;
            angle = ang;
            relAngle = relAng;
            x1 = x;
            y1 = y;
            weight = wt;
        }

        public double Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
            }
        }
        public double Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }
        public double ExitAngleLeft
        {
            get
            {
                return angle - relAngle;
            }
        }

        public double ExitAngleRight
        {
            get
            {
                return angle + relAngle;
            }
        }

        public int X1
        {
            get
            {
                return x1;
            }
            set
            {
                x1 = value;
            }
        }

        public int Y1
        {
            get
            {
                return y1;
            }
            set
            {
                y1 = value;
            }
        }

        public int X2
        {
            get
            {
                return x1 + GetDeltaX(length, angle);
            }
        }

        public int Y2
        {
            get
            {
                return y1 - GetDeltaY(length, angle);
            }
        }

        public int Weight
        {
            get
            {
                return weight;
            }
        }



        private int GetDeltaX (double length, double angle)
        {
            return (int)Math.Round((length * Math.Sin(angle)));
        }

        private int GetDeltaY(double length, double angle)
        {
            return (int)Math.Round((length * Math.Cos(angle)));
        }

        public override string ToString()
        {
            string descr = "Segment length " + Length + " Angle " + Angle + " ExitAngleLeft " + ExitAngleLeft + " ExitAngleRight " + ExitAngleRight + " Weight " + Weight + " from " + X1 + " , " + Y1 + " to " + X2 + " , " + Y2;
            return descr;
        }
    }
}
