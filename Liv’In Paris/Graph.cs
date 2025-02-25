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


        public void ResetNodes()
        {
            /// <summary>Remet chaque noeaud à un état non-visité, sert uniquement pour les parcours</summary>
            foreach (Node n in nodes)
            {
                n.Visited = false;
            }
        }


        public void DepthFirstSearch(int currentNodeIndex=0, int counter=0)
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
            Console.Write(nodes[currentNodeIndex].toString() + " ");
            foreach (int nodeIndex in nodes[currentNodeIndex].Links) /// Boucle sur les voisins du noeud courant
            {
                if (!nodes[nodeIndex].Visited) /// Explore uniquement les noeuds non visités, évitant les cycles
                {
                    DepthFirstSearch(nodeIndex, counter);
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
