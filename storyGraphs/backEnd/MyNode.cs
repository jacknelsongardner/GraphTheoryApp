using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace backEnd
{
    public class MyNode
    {
        public int nodeDegree;
        public int ID;
        public List<MyEdge> edges;

        public int nodeX;
        public int nodeY;

        public string label;
        public string content;

        public float r;
        public float g;
        public float b;

        public MyNode(int id, int x, int y)
        {
            ID = id;
            nodeDegree = 0;
            edges = new List<MyEdge>();
            updateXY(x, y);
        }

        public void addEdge(MyEdge e, int x, int y)
        {
            edges.Add(e);
            addDegree();
        }

        public void updateXY(int x, int y)
        {
            this.nodeX = x;
            this.nodeY = y;
        }

        public void removeEdgeReference(MyEdge e)
        {
            edges.Remove(e);
        }

        public void deleteAllEdges()
        {
            foreach(MyEdge elem in edges)
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
}
