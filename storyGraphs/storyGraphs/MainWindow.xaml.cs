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

namespace storyGraphs
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

        Brush StandardBrush;
        Random r = new Random();

        //when the user clicks with the left mousebutton on the Graph/Node canvas
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
                // this is the event that will check if we clicked on a rectangle or if we clicked on the canvas
                // if we clicked on a rectangle then it will do the following

                if (e.OriginalSource is Rectangle)
                {
                    // if the click source is a rectangle then we will create a new rectangle
                    // and link it to the rectangle that sent the click event
                    Rectangle activeRec = (Rectangle)e.OriginalSource; // create the link between the sender rectangle

                    NodesEdgesCanvas.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
                }

                // if we clicked on the canvas then we do the following
                else
                {
                    // generate a random colour and save it inside the custom brush variable
                    StandardBrush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255),
                    (byte)r.Next(1, 255), (byte)r.Next(1, 233)));

                    // create a re rectangle and give it the following properties
                    // height and width 50 pixels
                    // border thickness 3 pixels, fill colour set to the custom brush created above
                    // border colour set to black
                    Rectangle newRec = new Rectangle
                    {
                        Width = 50,
                        Height = 50,
                        StrokeThickness = 3,
                        Fill = StandardBrush,
                        Stroke = Brushes.Black
                    };

                    // once the rectangle is set we need to give a X and Y position for the new object
                    // we will calculate the mouse click location and add it there
                    Canvas.SetLeft(newRec, Mouse.GetPosition(NodesEdgesCanvas).X); // set the left position of rectangle to mouse X
                    Canvas.SetTop(newRec, Mouse.GetPosition(NodesEdgesCanvas).Y); // set the top position of rectangle to mouse Y

                    NodesEdgesCanvas.Children.Add(newRec); // add the new rectangle to the canvas
                }
            
        }






    }

    public class Node 
    { 
        
    }




}
