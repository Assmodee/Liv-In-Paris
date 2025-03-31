using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    class Tests<T>
    {
        Graph<T> testGraph;


        public Tests(Graph<T> testGraph)
        {
            this.testGraph = testGraph;
        }


        public void TestFunction()
        {

            Console.WriteLine("\nDFS test : ");
            List<int> DFSNodes = new List<int>(); /// La fonction DFS remplit une liste de noeuds
            testGraph.DFS(DFSNodes);
            foreach (int i in DFSNodes)
            {
                Console.Write(testGraph.NodesList[i].toString() + " | "); /// Affiche la liste des noeuds par DFS
            }
            Console.WriteLine("\n" + DFSNodes.Count + " Stations");

            Console.WriteLine("\nBFS test : ");
            List<int> BFSNodes = new List<int>();
            testGraph.BFS(BFSNodes); /// Remplit la liste par BFS
            foreach (int i in BFSNodes)
            {
                Console.Write(testGraph.NodesList[i].toString() + " | "); /// Affiche la liste des noeuds par BFS
            }
            Console.WriteLine("\n" + BFSNodes.Count + " Stations\n");

            List<int> dijkstraPath = testGraph.Dijkstra(1, 15);
            foreach (int stationId in dijkstraPath)
            {
                Console.Write(testGraph.NodesList[stationId].toString() + " ");
            }
        }
    }
}
