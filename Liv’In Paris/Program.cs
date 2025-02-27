namespace Liv_In_Paris
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<List<int>> adjacenceMatrix = new List<List<int>>(); 
            List<List<int>> incidenceMatrix = new List<List<int>>();
            List<Node> nodes = new List<Node>();
            FillAdjacenceMatrix(adjacenceMatrix);
            
            for (int i = 0; i < adjacenceMatrix.Count; i++)
            {
                List<int> incidenceList = new List<int>(); /// Remplit la matrice d'incidence en créant des listes d'incidence
                for (int j = 0; j < adjacenceMatrix[i].Count; j++)
                {
                    if (adjacenceMatrix[i][j] == 1)
                    {
                        incidenceList.Add(j);
                    }
                }
                incidenceMatrix.Add(incidenceList);
                nodes.Add(new Node(i + 1, incidenceList)); /// <remarks>Les listes d'incidences sont aussi données en paramètres des noeuds (plus facile pour les parcours de graphe)</remarks>
            }

            Graph g = new Graph(nodes, adjacenceMatrix, incidenceMatrix);
            Console.WriteLine("BFS : ");
            g.BreadthFirstSearch(new Queue<int>());
            Console.WriteLine("\n\nDFS : ");
            g.DepthFirstSearch();
            Console.Write("\n\nConnexité : " + g.isConnected() + "\n\n");
            Console.Write("\n\nCycles : ");
            g.CyclesSearch(new List<int>());
        }


        static void FillAdjacenceMatrix(List<List<int>> adjacenceMatrix)
        {
            /// <summary>Extrait les informations du fichier 'soc-karate.mtx' pour remplir une matrice d'adjacence</summary>
            string content = File.ReadAllText("soc-karate.mtx");
            string[] lines = content.Split('\n');
            int nodesCount = 0;

            for (int i = 24; i < lines.Length && lines[i].Length > 0; i++) /// Extrait à partir de la 24e ligne du document
            {
                int node1 = 0;
                int node2 = 0;
                int temp;
                int j = 0;
                while (j < lines[i].Length && int.TryParse(lines[i][j].ToString(), out temp)) /// Lit chaque caratère et l'enregistre si c'est un entier
                {
                    node1 = node1 * 10 + temp; /// Ajoute l'entier pour reconstruire la valeur du noeud
                    j++;
                }
                j++; /// Saute l'espace au milieu
                while (j < lines[i].Length && int.TryParse(lines[i][j].ToString(), out temp)) /// Idem pour le deuxième noaud de la ligne
                {
                    node2 = node2 * 10 + temp;
                    j++;
                }
                if (Math.Max(node1, node2) > nodesCount)
                {
                    for (int k = 0; k < Math.Max(node1, node2); k++)
                    {
                        if (k < nodesCount)
                        {
                            for (int l = 0; l < Math.Max(node1, node2) - nodesCount; l++)
                            {
                                adjacenceMatrix[k].Add(0); /// Etend la matrice d'adjacence sur les colonnes si un nouveau noeud est rencontré
                            }
                        }
                        else
                        {
                            adjacenceMatrix.Add(new List<int>()); /// Etend sur les lignes
                            for (int l = 0; l < Math.Max(node1, node2); l++)
                            {
                                adjacenceMatrix[k].Add(0); /// Remplit sur les lignes
                            }
                        }
                    }
                    nodesCount = Math.Max(node1, node2);
                }
                adjacenceMatrix[node1-1][node2-1] = 1; /// Met à jour la matrice d'adjacence
                adjacenceMatrix[node2-1][node1-1] = 1;
            }
        }


        static void DisplayMatrix(List<List<int>> matrix)
        {
            /// <summary>Permet d'afficher la matrice d'adjacence/incidence</summary>
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    Console.Write(matrix[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}