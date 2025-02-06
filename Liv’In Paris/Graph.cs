using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    internal class Graph
    {
        List<Node> nodes;
        List<Link> links;
        int[,] adjacenceMatrix;


        public Graph(List<Node> nodes, List<Link> links, int[,] adjacenceMatrix)
        {
            this.nodes = nodes;
            this.links = links;
            this.adjacenceMatrix = adjacenceMatrix;
        }


        public List<Node> Nodes { get { return nodes; } }


        public List<Link> Links { get {  return links; } }


        public int[,] AdjacenceMtrix { get { return adjacenceMatrix; } }


        public void DepthFirstSearch(List<int> nodesList, int currentNode=0)
        {
            nodesList.Add(currentNode);
            for (int i = 0; i < 34; i++)
            {
                if (adjacenceMatrix[currentNode, i] == 1)
                {
                    if (!nodesList.Contains(i))
                    {
                        DepthFirstSearch(nodesList, i);
                    }
                }
            }
            Console.Write(nodes[currentNode].toString() + " ");
        }


        public void BreadthFirstSearch(Queue<int> nodesQueue, int currentNode=0)
        {
            for (int i = 0; i < 34; i++)
            {
                if (adjacenceMatrix[currentNode, i] == 1)
                {
                    if (!nodesQueue.Contains(i))
                    {
                        nodesQueue.Enqueue(i);
                    }
                }
            }
        }
    }
}
