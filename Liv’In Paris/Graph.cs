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
        List<List<int>> adjacenceMatrix;
        List<List<int>> incidenceMatrix;


        public Graph(List<Node> nodes, List<List<int>> adjacenceMatrix, List<List<int>> incidenceMatrix)
        {
            this.nodes = nodes;
            this.adjacenceMatrix = adjacenceMatrix;
            this.incidenceMatrix = incidenceMatrix;
        }


        public bool isConnected()
        {
            /// <summary>Indique si un graphe est connexe.</summary>
            bool connected = true;

            DepthFirstSearch(0, 0, false); /// DFS sans affichage
            for (int i = 0; i < nodes.Count && connected; i++)
            {
                /// Parcourt chaque noeaud et si un noeud n'est pas visité, le graphe n'est pas connexe (seule une composante est visitée)
                if (!nodes[i].Visited)
                {
                    connected = false;
                }
            }

            return connected;
        }


        public bool CyclesSearch(List<int> indexList, int nodeIndex=0, int prevNodeIndex=0)
        {
            /// Effectue une DFS et s'arrête dès qu'un noeud déjà visité l'est à nouveau
            indexList.Add(nodeIndex);
            foreach (int index in nodes[nodeIndex].Links)
            {
                if (index != prevNodeIndex) /// Evite le cas où l'on repasse sur le noeud que l'on vient de visiter
                {
                    if (indexList.Contains(index))
                    {
                        foreach (int visitedNodeIndex in indexList)
                        {
                            Console.Write(nodes[visitedNodeIndex].toString() + " "); /// Affiche le circuit
                        }
                        return true;
                    }
                    CyclesSearch(indexList, index, nodeIndex);
                }
            }
            return false;
        }


        public void ResetNodes()
        {
            /// <summary>Remet chaque noeaud à un état non-visité, sert uniquement pour les parcours</summary>
            foreach (Node n in nodes)
            {
                n.Visited = false;
            }
        }


        public void DepthFirstSearch(int currentNodeIndex=0, int counter=0, bool display=true)
        {
            /// <summary>Explore en profondeur le graphe à partir d'un noeud donné (par défaut le premier)</summary>
            /// <remarks>NE PAS TOUCHER AU COMPTEUR, il sert à réinitialiser l'état des noeuds</remarks>
            if (counter == 0)
            {
                Console.WriteLine();
                ResetNodes();
            }
            counter++;
            nodes[currentNodeIndex].Visited = true; /// Dès qu'un noeud est visité, il est marqué comme tel
            if (display)
            Console.Write(nodes[currentNodeIndex].toString() + " ");
            foreach (int nodeIndex in nodes[currentNodeIndex].Links) /// Boucle sur les voisins du noeud courant
            {
                if (!nodes[nodeIndex].Visited) /// Explore uniquement les noeuds non visités, évitant les cycles
                {
                    DepthFirstSearch(nodeIndex, counter, display);
                }
            }
        }


        public void BreadthFirstSearch(Queue<int> nodesQueue, int currentNodeIndex=0, int counter=0) 
        {
            /// <summary>Parcours en largeur le graphe à partir d'un noeud donné (par défaut le premier)</summary>
            /// <remarks>Pareil, il faut pas toucher au compteur</remarks>
            if (counter == 0)
            {
                Console.WriteLine();
                ResetNodes();
            }
            counter++;
            Console.Write(nodes[currentNodeIndex].toString() + " ");
            nodes[currentNodeIndex].Visited = true;
            foreach (int nodeIndex in nodes[currentNodeIndex].Links) /// Boucle parmi les voisins du noeud courant
            {
                if (!nodes[nodeIndex].Visited)
                {
                    nodes[nodeIndex].Visited = true; /// Marque un noeud visité comme tel
                    nodesQueue.Enqueue(nodeIndex);
                }
            }
            if (nodesQueue.Count > 0)
            BreadthFirstSearch(nodesQueue, nodesQueue.Dequeue(), counter); /// Passe au noeud suivant dans la file si elle n'est pas vide
        }
    }
}
