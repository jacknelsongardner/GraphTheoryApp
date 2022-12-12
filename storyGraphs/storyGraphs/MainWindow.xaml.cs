using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace storyGraphs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Object selectedObject = new Object();

        //whether the mouse is being dragged
        bool mouseDragging;

        //whether we are in edge mode or node mode
        bool edgeMode;
        bool nodeMode;


        Brush StandardBrush;
        Random r = new Random();

        //variables for storing selected rectangle, node, etc
        Node selectedNode = new Node();

        Edge selectedEdge = new Edge();

        Rectangle selectedRectangle = new Rectangle();

        Line selectedLine = new Line();

        LinkedList<Node> nodeHistory = new LinkedList<Node>();
        LinkedList<Rectangle> rectangleHistory = new LinkedList<Rectangle>();

        //nodes and rectangles that are selected to be connected
        Node selectedNodeToConnectOne = new Node();
        Node selectedNodeToConnectTwo = new Node();

        Rectangle selectedRectangleToConnectOne = new Rectangle();
        Rectangle selectedRectangleToConnectTwo = new Rectangle();

        //how many times the user has clicked on nodes to connect them
        int clickCounter = 0;

        //dictionary for storing all the rectangles, so they can be called and deleted later on
        Dictionary<Rectangle, Node> rectanglesToNodes = new Dictionary<Rectangle, Node>();
        Dictionary<Line, Edge> linesToEdges = new Dictionary<Line, Edge>();

        Dictionary<Node, Rectangle> nodesToRectangles = new Dictionary<Node, Rectangle>();
        Dictionary<Edge, Line> edgesToLines = new Dictionary<Edge, Line>();

        List<Rectangle> rectanglesOnCanvas = new List<Rectangle>();
        List<Line> linesOnCanvas = new List<Line>();

        List<Node> nodesOnCanvas = new List<Node>();
        List<Edge> edgesOnCanvas = new List<Edge>();

        public MainWindow()
        {
            InitializeComponent();
            nodeMode = true;
            edgeMode = false;
        }

        //WPF EVENTS

        //when the user clicks with the RIGHT mousebutton on the Graph/Node canvas
        private void MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (nodeMode == true && edgeMode == false)
            {
                // If we're clicking on a rectangle
                if (e.OriginalSource is Rectangle)
                {
                    Rectangle activeRec = (Rectangle)e.OriginalSource;
                    Node activeNode = rectanglesToNodes[activeRec];

                    //deleting all lines and edges connected to rectangle
                    foreach(Edge elem in activeNode.edges)
                    {
                        NodesEdgesCanvas.Children.Remove(edgesToLines[elem]);
                        edgesOnCanvas.Remove(elem);

                        linesToEdges.Remove(edgesToLines[elem]);
                        edgesToLines.Remove(elem);
                    }

                    //deleting the rectangle itself
                    activeNode.deleteAllEdges();

                    NodesEdgesCanvas.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
                    
                    rectanglesToNodes.Remove(activeRec);
                    nodesToRectangles.Remove(activeNode);
                    
                    rectanglesOnCanvas.Remove(activeRec);
                    nodesOnCanvas.Remove(activeNode);

                }
                // Else if we clicked on the canvas 
                else
                {
                    //creating the new rectangle

                    StandardBrush = new SolidColorBrush(Colors.Blue);

                    //creating new rectangle
                    Rectangle newRec = new Rectangle
                    {
                        Width = 50,
                        Height = 50,
                        StrokeThickness = 3,
                        Fill = StandardBrush,
                        Stroke = Brushes.Black
                    };

                    Node newNode = new Node();

                    rectanglesToNodes.Add(newRec, newNode);
                    nodesToRectangles.Add(newNode, newRec);

                    rectanglesOnCanvas.Add(newRec);
                    nodesOnCanvas.Add(newNode);

                    //getting mouse x and y for rectangle placement
                    Canvas.SetLeft(newRec, Mouse.GetPosition(NodesEdgesCanvas).X); // set the left position of rectangle to mouse X
                    Canvas.SetTop(newRec, Mouse.GetPosition(NodesEdgesCanvas).Y); // set the top position of rectangle to mouse Y

                    NodesEdgesCanvas.Children.Add(newRec); // add the new rectangle to the canvas

                }
            }

            else if (nodeMode == false && edgeMode == true)
            {
                if (e.OriginalSource is Rectangle)
                {
                    Rectangle activeRec = (Rectangle)e.OriginalSource;
                    Node activeNode = rectanglesToNodes[activeRec];

                    if (clickCounter == 0)
                    {
                        //setting the first node to connect
                        selectedNodeToConnectOne = activeNode;
                        selectedRectangleToConnectOne = activeRec;

                        //adding one to click counter
                        clickCounter++;

                    }
                    else if (clickCounter == 1)
                    {
                        //setting the first node to connect
                        selectedNodeToConnectTwo = activeNode;
                        selectedRectangleToConnectTwo = activeRec;

                        //make a line connecting the two
                        Line activeLine = new Line();

                        //setting position of the line
                        activeLine.X1 = Canvas.GetLeft(selectedRectangleToConnectOne);
                        activeLine.Y1 = Canvas.GetTop(selectedRectangleToConnectOne);

                        activeLine.X2 = Canvas.GetLeft(selectedRectangleToConnectTwo);
                        activeLine.Y2 = Canvas.GetTop(selectedRectangleToConnectTwo);

                        //setting stroke of the line
                        SolidColorBrush lineBrush = new SolidColorBrush();
                        lineBrush.Color = Colors.Red;

                        activeLine.StrokeThickness = 4;
                        activeLine.Stroke = lineBrush;

                        Edge activeEdge = new Edge(selectedNodeToConnectOne, selectedNodeToConnectTwo);

                        //making new edge and putting it in the dictionary
                        linesToEdges.Add(activeLine, activeEdge);
                        edgesToLines.Add(activeEdge, activeLine);

                        edgesOnCanvas.Add(activeEdge);
                        linesOnCanvas.Add(activeLine);

                        NodesEdgesCanvas.Children.Add(activeLine);

                        //adding the edge/line to the selected nodes
                        selectedNodeToConnectOne.addEdge(activeEdge);
                        selectedNodeToConnectTwo.addEdge(activeEdge);

                        //reset clickcounter
                        clickCounter = 0;
                    }
                }
                else if (e.OriginalSource is Line)
                {
                    //delete the line and the attached node
                    Line activeLine = (Line)e.OriginalSource;
                    Edge activeEdge = linesToEdges[activeLine];

                    //remove visual representation of the line
                    NodesEdgesCanvas.Children.Remove(activeLine);

                    linesToEdges[activeLine].deleteSelfFromNodes();

                    // removing all edges/lines permanently
                    linesToEdges.Remove(activeLine);
                    edgesToLines.Remove(activeEdge);
                    edgesOnCanvas.Remove(activeEdge);
                    linesOnCanvas.Remove(activeLine);
                }
            }

        }

        //when the user clicks with the LEFT mousebutton on the GRAPHNode canvas
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            // If we're clicking on a rectangle
            if (e.OriginalSource is Rectangle)
            {
                //making it the selected rectangle and node
                Rectangle activeRec = (Rectangle)e.OriginalSource;

                selectedRectangle = activeRec;
                selectedNode = rectanglesToNodes[activeRec];
                selectedLine = new Line();
                selectedEdge = new Edge();

                //grabbing the node to drag
                mouseDragging = true;
                activeRec.CaptureMouse();
            }
            else if (e.OriginalSource is Line)
            {
                //making it the selected line and edge
                Line activeLine = (Line)e.OriginalSource;

                selectedRectangle = new Rectangle();
                selectedNode = new Node();
                selectedLine = activeLine;
                selectedEdge = linesToEdges[activeLine];


            }
        }

        //when the user clicks with the LEFT mousebutton on the GRAPHNode canvas
        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // If we're clicking on a rectangle
            if (e.OriginalSource is Rectangle)
            {

                //releasing the krakken!...er, the rectangle
                Rectangle activeRec = (Rectangle)e.OriginalSource;
                Node activeNode = rectanglesToNodes[activeRec];
                
                mouseDragging = false;
                activeRec.ReleaseMouseCapture();
            }
        }

        //when the users mouse moves
        private void MouseMove(object sender, MouseEventArgs e)
        {
            //if we're moving a rectangle
            if (e.OriginalSource is Rectangle)
            {

                //if the mouse has a rectangle in its grasp
                if (!mouseDragging) return;
                else
                {
                    //the rectangle we're dealing with
                    Rectangle activeRec = (Rectangle)e.OriginalSource;
                    Node activeNode = rectanglesToNodes[activeRec];

                    // get the position of the mouse relative to the Canvas
                    var mousePos = e.GetPosition(NodesEdgesCanvas);

                    // put the rectangle on the mouse
                    double left = mousePos.X - (activeRec.ActualWidth / 2);
                    double top = mousePos.Y - (activeRec.ActualHeight / 2);
                    Canvas.SetLeft(activeRec, left);
                    Canvas.SetTop(activeRec, top);

                    
                    //updating the location of the edges/lines that are attached to the nodde we are dragging
                    foreach (Edge edge in activeNode.edges)
                    {
                        edgesToLines[edge].X1 = Canvas.GetLeft(nodesToRectangles[edge.nodeOne]);
                        edgesToLines[edge].X2 = Canvas.GetLeft(nodesToRectangles[edge.nodeTwo]);
                        edgesToLines[edge].Y1 = Canvas.GetTop(nodesToRectangles[edge.nodeOne]);
                        edgesToLines[edge].Y2 = Canvas.GetTop(nodesToRectangles[edge.nodeTwo]);
                    }
                }
            }
        }

        //when the edge mode button is clicked
        private void OnEdgeModeClick(object sender, RoutedEventArgs e)
        {
            //putting us in edge mode
            nodeMode = false;
            edgeMode = true;
        }

        //when the node mode button is clicked
        private void OnNodeModeClick(object sender, RoutedEventArgs e)
        {
            //putting us in node mode
            nodeMode = true;
            edgeMode = false;
        }


    }
    
    //CLASSES (later to be put in another text file, for organization purposes)

    //node class
    public class Node
    {

        public int nodeDegree;
        public int ID;
        public List<Edge> edges;

        public int nodeX;
        public int nodeY;

        public string label;
        public string content;

        public float r;
        public float g;
        public float b;

        public Node()
        {
            edges = new List<Edge>();
        }
         
        public Node(int id, int x, int y)
        {
            ID = id;
            nodeDegree = 0;
            edges = new List<Edge>();
            updateXY(x, y);
        }

        public void addEdge(Edge e)
        {
            edges.Add(e);
            addDegree();
        }

        public void updateXY(int x, int y)
        {
            this.nodeX = x;
            this.nodeY = y;
        }

        public void removeEdgeReference(Edge e)
        {
            edges.Remove(e);
        }

        public void deleteAllEdges()
        {
            foreach (Edge elem in edges.ToArray())
            {
                elem.deleteSelfFromNodes();
                edges.Remove(elem);
            }
        }

        public void addDegree()
        {
            this.nodeDegree++;
        }

        public void minusDegree()
        {
            this.nodeDegree--;
        }

        public void incrementID()
        {
            ID++;
        }
    }

    //edge class
    public class Edge
    {
        
        //the nodes that this line connects
        public Node nodeOne;
        public Node nodeTwo;

        public int node1EndX;
        public int node1EndY;
        public int node2EndX;
        public int node2EndY;

        public string label;

        public float r;
        public float g;
        public float b;

        //CONSTRUCTORS
        public Edge()
        {

        }

        public Edge(Node n1, Node n2)
        {
            nodeOne = n1;
            nodeTwo = n2;
            updateXY();
        }

        public void setNode(Node n1, Node n2)
        {
            nodeOne = n1;
            nodeTwo = n2;
            updateXY();
        }

        public void deleteSelfFromNodes()
        {
            nodeOne.removeEdgeReference(this);
            nodeTwo.removeEdgeReference(this);
        }

        public void updateXY()
        {
            node1EndX = nodeOne.nodeX;
            node1EndY = nodeOne.nodeY;

            node2EndX = nodeTwo.nodeX;
            node2EndY = nodeTwo.nodeY;
        }
    }


}
