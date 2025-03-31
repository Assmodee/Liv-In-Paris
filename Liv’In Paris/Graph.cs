using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    class Graph<T>
    {
        List<List<int>> incidenceMatrix;
        List<Node<T>> nodesList; /// Liste des noeuds par id croissant
        List<List<double>> weights; /// Liste des poids des connexions
        Dictionary<T, int> reverseIdDic; /// Dictionnaire value vers id pour opérer sur les noeuds depuis leur valeur


        public Graph(List<List<int>> incidenceMatrix, List<Node<T>> nodesList, List<List<double>> weights, Dictionary<T, int> reverseIdDic)
        {
            this.incidenceMatrix = incidenceMatrix;
            this.nodesList = nodesList;
            this.weights = weights;
            this.reverseIdDic = reverseIdDic;
        }


        # region Proprietes attributs

        public List<List<int>> IncidenceMatrix
        {
            get { return incidenceMatrix; }
        }


        public List<List<double>> Weights
        {
            get { return weights; }
        }


        public List<Node<T>> NodesList
        {
            get { return nodesList; }
        }

        public Dictionary<T, int> ReverseIdDic
        {
            get { return reverseIdDic; }
        }

        #endregion


        #region Parcours et plus court chemin

        /// <summary>
        /// Les DFS et BFS supposent le graphe connexe. Si ce n'est pas le cas, les deux algorithmes
        /// vont parcourir la composante connexe de laquelle ils partent.
        /// </summary>

        public void DFS(List<int> visitedNodes, int currentNodeId=1)
        {
            if (visitedNodes.Count < nodesList.Count) /// S'arrête quand tous les noeuds sont visités
            {
                if (!visitedNodes.Contains(currentNodeId)) /// Evite les circuits
                {
                    visitedNodes.Add(currentNodeId);
                    foreach (int nodeId in incidenceMatrix[currentNodeId]) /// Visite tous les voisins
                    {
                        DFS(visitedNodes, nodeId);
                    }
                }
            }
        }


        public void BFS(List<int> visitedNodes, Queue<int> nodesToVisit=null, int currentNodeId=1)
        {
            if (nodesToVisit == null)
            {
                nodesToVisit = new Queue<int>();
            }
            if (visitedNodes.Count < nodesList.Count)
            {
                foreach (int nodeId in incidenceMatrix[currentNodeId])
                {
                    if (!visitedNodes.Contains(nodeId) && !nodesToVisit.Contains(nodeId)) /// Ajoute les prochains noeuds à visiter
                    {
                        nodesToVisit.Enqueue(nodeId);
                    }
                }
                visitedNodes.Add(currentNodeId);
                if (nodesToVisit.Count > 0) /// Continue uniquement s'il reste des noeuds à visiter
                    BFS(visitedNodes, nodesToVisit, nodesToVisit.Dequeue());
            }
        }


        public int GetMinElementId(double[] arr)
        {
            int index = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] < arr[index])
                {
                    index = i;
                }
            }

            return index;
        }


        public void Dijkstra(int startingNodeId, int endingNodeId, int currentNodeId=0, List<int> visitedNodes=null, int[] predecessors=null, double[] distances=null, double cumulDistance=0)
        {
            if (currentNodeId == 0)
            {
                visitedNodes = new List<int>();
                predecessors = new int[nodesList.Count];
                distances = new double[nodesList.Count];
                currentNodeId = startingNodeId;
                for (int i = 0; i < distances.Length; i++)
                {
                    distances[i] = int.MaxValue-1;
                }
                distances[currentNodeId] = 0;
                distances[0] = int.MaxValue;
            }
            currentNodeId = GetMinElementId(distances);
            distances[currentNodeId] = int.MaxValue;
            Console.WriteLine(currentNodeId);
            if (currentNodeId != endingNodeId)
            {
                Dijkstra(startingNodeId, endingNodeId, currentNodeId, visitedNodes, predecessors, distances, cumulDistance);
            }
        }

        # endregion
    }
}
