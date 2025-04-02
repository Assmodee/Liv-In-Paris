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
        double[,] adjacenceMatrix;
        int size; /// # of connexions
        List<Node<T>> nodesList; /// Liste des noeuds par id croissant
        List<List<double>> weights; /// Liste des poids des connexions
        Dictionary<T, int> reverseIdDic; /// Dictionnaire value vers id pour opérer sur les noeuds depuis leur valeur


        public Graph(List<List<int>> incidenceMatrix, List<Node<T>> nodesList, List<List<double>> weights, Dictionary<T, int> reverseIdDic)
        {
            this.incidenceMatrix = incidenceMatrix;
            this.nodesList = nodesList;
            this.weights = weights;
            this.reverseIdDic = reverseIdDic;
            for (int i = 0; i < incidenceMatrix.Count; i++)
            {
                for (int j = 0; j < incidenceMatrix[i].Count; j++)
                {
                    size++;
                }
            }
            adjacenceMatrix = new double[incidenceMatrix.Count, incidenceMatrix.Count];
            for (int i = 0; i < incidenceMatrix.Count; i++)
            {
                for (int j = 0; j < incidenceMatrix.Count; j++)
                {
                    adjacenceMatrix[i, j] = double.MaxValue;
                }
                for (int j = 0; j < incidenceMatrix[i].Count; j++)
                {
                    adjacenceMatrix[i, incidenceMatrix[i][j]] = weights[i][j];
                }
            }
        }


        public void CopyElements(double[,] m1, double[,] m2)
        {
            for (int i = 0; i < m1.GetLength(0); i++)
            {
                for (int j = 0; j < m1.GetLength(1); j++)
                {
                    m2[i, j] = m1[i, j];
                }
            }
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


        public List<int> Dijkstra(int startingNodeId, int endingNodeId)
        {
            /// Initialisation
            double[] distances = new double[nodesList.Count];
            int[] predecessors = new int[nodesList.Count];
            List<int> visitedNodes = new List<int>();
            for (int i = 0; i < distances.Length; i++)
                distances[i] = double.MaxValue - 1;
            distances[0] = double.MaxValue;
            distances[startingNodeId] = 0;
            int currentNodeId = 0;

            /// Dijkstra commence ici
            while (currentNodeId != endingNodeId)
            {
                currentNodeId = GetMinElementId(distances); /// Récupère la station la plus proche
                distances[currentNodeId] = double.MaxValue; /// Assigne la station à +inf
                for (int i = 0; i < incidenceMatrix[currentNodeId].Count; i++)
                {
                    /// Mise à jour des distances des stations
                    if (!visitedNodes.Contains(incidenceMatrix[currentNodeId][i]) && distances[incidenceMatrix[currentNodeId][i]] > weights[currentNodeId][i])
                    {
                        distances[incidenceMatrix[currentNodeId][i]] = weights[currentNodeId][i];
                        predecessors[incidenceMatrix[currentNodeId][i]] = currentNodeId; /// Marque le prédecesseur pour retrouver le plus court chemin
                    }
                }
                visitedNodes.Add(currentNodeId); /// Ajoute la station aux stations visitées
            } /// Fin algo

            /// Remplit la liste des noeuds à visiter pour le plus court chemin
            visitedNodes = new List<int>() { endingNodeId };
            while (currentNodeId != startingNodeId)
            {
                currentNodeId = predecessors[currentNodeId]; /// Parcourt le chemin à l'envers
                visitedNodes.Add(currentNodeId);
            }
            visitedNodes.Reverse();

            return visitedNodes; /// Liste des stations pour le plus court chemin
        }


        public List<int> BellmanFord(int startingNodeId, int endingNodeId)
        {
            /// Initialisation
            double[] distances = new double[nodesList.Count];
            int[] predecessors = new int[nodesList.Count];
            List<int> visitedNodes = new List<int>() { endingNodeId };
            for (int i = 0; i < distances.Length; i++)
                distances[i] = double.MaxValue - 1;
            distances[0] = double.MaxValue;
            distances[startingNodeId] = 0;

            /// Bellman-Ford
            for (int cpt = 0; cpt < size; cpt++)
            {
                for (int i = 1; i < incidenceMatrix.Count; i++)
                {
                    for (int j = 0; j < incidenceMatrix[i].Count; j++)
                    {
                        if (distances[i] + Weights[i][j] < distances[incidenceMatrix[i][j]])
                        {
                            /// Mets à jour les distances et les prédecesseurs
                            distances[incidenceMatrix[i][j]] = distances[i] + Weights[i][j];
                            predecessors[incidenceMatrix[i][j]] = i;
                        }
                    }
                }
            }

            /// Remplit la liste des noeuds du plus court chemin
            int currentNodeId = endingNodeId;
            while (currentNodeId != startingNodeId)
            {
                currentNodeId = predecessors[currentNodeId];
                visitedNodes.Add(currentNodeId);
            }
            visitedNodes.Reverse(); /// Remet la liste à l'endroit

            return visitedNodes;
        }


        public List<int> FloydWarshall(int startingNodeId, int endingNodeId)
        {
            /// Initialisation
            List<int> visitedNodes = new List<int>();
            double[,] distances = new double[adjacenceMatrix.GetLength(0), adjacenceMatrix.GetLength(1)];
            int[,] pathMatrix = new int[adjacenceMatrix.GetLength(0), adjacenceMatrix.GetLength(1)];
            int nodesCount = nodesList.Count;
            CopyElements(adjacenceMatrix, distances);
            for (int i = 0; i < nodesCount; i++)
            {
                for (int j = 0; j < nodesCount; j++)
                {
                    pathMatrix[i, j] = j;
                }
            }

            /// Commence ici
            for (int k = 0; k < nodesCount; k++)
            {
                for (int i = 0; i < nodesCount; i++)
                {
                    for (int j = 0; j < nodesCount; j++)
                    {
                        if (distances[i, k] + distances[k, j] < distances[i, j])
                        {
                            distances[i, j] = distances[i, k] + distances[k, j]; /// Met à jour la distance
                            pathMatrix[i, j] = pathMatrix[i, k]; /// Met à jour la matrice de correspondance des plus courts chemins pour pouvoir lister les noeuds empruntés lors d'un chemin
                        }
                    }
                }
            }

            int currentNodeId = startingNodeId;
            while (currentNodeId != endingNodeId)
            {
                visitedNodes.Add(currentNodeId);
                currentNodeId = pathMatrix[currentNodeId, endingNodeId]; /// Reconstruit la liste des noeuds empruntés pour un plus court chemin
            }
            visitedNodes.Add(endingNodeId);

            return visitedNodes;
        }


        # endregion
    }
}
