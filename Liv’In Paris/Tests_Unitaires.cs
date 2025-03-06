using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    class Tests_Unitaires
    {
        List<List<int>> adjacenceMatrix;
        List<List<int>> incidenceMatrix;
        List<Node> nodes;


        public Tests_Unitaires(List<List<int>> adjacenceMatrix, List<List<int>> incidenceMatrix, List<Node> nodes)
        {
            this.adjacenceMatrix = adjacenceMatrix;
            this.incidenceMatrix = incidenceMatrix;
            this.nodes = nodes;
        }


        static int[] CalcDeg(List<List<int>> adjacenceMatrix)
        {
            int nodes = adjacenceMatrix.Count;
            int[] degrees = new int[nodes];
            for (int i = 0; i < nodes; i++)
            {
                int degree = 0;
                for (int j = 0; j < nodes; j++)
                {
                    degree += adjacenceMatrix[i][j]; /// Chaque 1 représente une connexion
                }
                degrees[i] = degree;
            }
            return degrees;
        }


        public void Test()
        {
            Graph g = new Graph(nodes, adjacenceMatrix, incidenceMatrix);
            Console.WriteLine("Tests : \n\nBFS : ");
            g.BreadthFirstSearch(new Queue<int>());
            Console.WriteLine("\n\nDFS : ");
            g.DepthFirstSearch();
            Console.Write("\n\nConnexité : " + g.isConnected() + "\n\n");
            Console.Write("\n\nCycles : ");
            List<int> circuits = g.CircuitsSearch(new List<int>());
            foreach (int index in circuits)
            {
                Console.Write(nodes[index].toString() + " ");
            }

            int[] degrees = CalcDeg(adjacenceMatrix);
            Console.Write("\n\nDegré pour chaque noeaud: ");
            foreach (int deg in degrees)
            {
                Console.Write(deg + " ");
            }

            string filename = "graph.png";
            Drawing.DrawGraph(adjacenceMatrix, degrees, filename);

            Console.WriteLine($"\n\nImage générée : {Path.GetFullPath(filename)}");
            Console.WriteLine("\nVoir l'image géneré dans le repertoire dit en haut");
        }
    }
}
