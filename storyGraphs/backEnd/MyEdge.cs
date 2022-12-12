using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd
{
    public class MyEdge
    {

        //the nodes that this line connects
        public MyNode node1;
        public MyNode node2;

        public int node1EndX;
        public int node1EndY;
        public int node2EndX;
        public int node2EndY;

        public string label;

        public float r;
        public float g;
        public float b;

        //CONSTRUCTORS
        public MyEdge()
        {

        }

        public MyEdge(MyNode n1, MyNode n2)
        {
            node1 = n1;
            node2 = n2;
            updateXY();
        }

        public void setNode(MyNode n1, MyNode n2)
        {
            node1 = n1;
            node2 = n2;
            updateXY();
        }

        public void deleteSelfFromNodes()
        {
            node1.removeEdgeReference(this);
            node2.removeEdgeReference(this);
        }

        public void updateXY()
        {
            node1EndX = node1.nodeX;
            node1EndY = node1.nodeY;

            node2EndX = node2.nodeX;
            node2EndY = node2.nodeY;
        }
    }
}
