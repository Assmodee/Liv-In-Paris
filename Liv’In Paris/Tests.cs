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


        public string minutesToTime(double minutes)
        {
            double time = minutes;
            int hours = (int)time / 60;
            time -= hours * 60;
            int min = (int)time;
            time = (int)((time - (int)time) * 100);
            min += (int)time / 60;
            time -= (int)(time / 60) * 60;
            int seconds = (int)time;
            return "" + hours + "h " + min + "m " + seconds + "s;";
        }


        public void displayPathInfo(List<int> path)
        {
            double totalWeight = 0;
            for (int i = 0; i < path.Count; i++)
            {
                Console.Write(testGraph.NodesList[path[i]].toString() + " "); /// Affiche la prochaine station
                if (i != path.Count - 1)
                {
                    for (int j = 0; j < testGraph.IncidenceMatrix[path[i]].Count; j++)
                    {
                        if (testGraph.IncidenceMatrix[path[i]][j] == path[i + 1])
                        {
                            totalWeight += testGraph.Weights[path[i]][j]; /// Récupère le poids associé à la connexion
                        }
                    }
                }
            }
            Console.WriteLine("\n\nTotal : " + minutesToTime(totalWeight)); /// AFfiche le poids total au format date
        }


        public void Separator()
        {
            Console.WriteLine();
            for (int i = 0; i < 209; i++)
            {
                Console.Write("=");
            }
            Console.WriteLine();
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

            Separator();

            Console.WriteLine("\nBFS test : ");
            List<int> BFSNodes = new List<int>();
            testGraph.BFS(BFSNodes); /// Remplit la liste par BFS
            foreach (int i in BFSNodes)
            {
                Console.Write(testGraph.NodesList[i].toString() + " | "); /// Affiche la liste des noeuds par BFS
            }
            Console.WriteLine("\n" + BFSNodes.Count + " Stations");

            Separator();
            Separator();

            List<int> dijkstraPath, bellmanFordPath;
            Console.WriteLine("\nPlus court chemin de République à Saint-Mandé : (Dijkstra)\n");
            dijkstraPath = testGraph.Dijkstra(67, 23); /// Applique l'algorithme de Dijkstra
            displayPathInfo(dijkstraPath);

            Separator();

            Console.WriteLine("\nPlus court chemin de Porte de La Défense à Châteu de Vincennes : (Dijkstra)\n");
            dijkstraPath = testGraph.Dijkstra(1, 25);
            displayPathInfo(dijkstraPath);

            Separator();

            Console.WriteLine("\nPlus court chemin de République à Saint-Mandé : (Bellman-Ford)\n");
            bellmanFordPath = testGraph.BellmanFord(67, 23); /// Applique Bellman-Ford
            displayPathInfo(bellmanFordPath);

            Separator();

            Console.WriteLine("\nPlus court chemin de La Défense à Château de Vincennes : (Bellman-Ford)\n");
            bellmanFordPath = testGraph.BellmanFord(1, 25);
            displayPathInfo(bellmanFordPath);
        }



        


    }
}
