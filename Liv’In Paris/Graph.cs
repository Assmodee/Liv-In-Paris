using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Drawing.Design;
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
        int size;
        List<Node<T>> nodesList;
        List<List<double>> weights;
        Dictionary<T, int> reverseIdDic;


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


        public List<int> GetNodesByDegree()
        {
            List<int> nodeIdList = new List<int>();
            List<int> degrees = new List<int>();

            for (int i = 0; i < incidenceMatrix.Count; i++)
            {
                degrees.Add(incidenceMatrix[i].Count);
                nodeIdList.Add(i);
            }

            for (int i = 0; i < degrees.Count; i++)
            {
                for (int j = 0; j < degrees.Count - 1; j++)
                {
                    if (degrees[j] < degrees[j + 1])
                    {
                        int tempDegree = degrees[j];
                        degrees[j] = degrees[j + 1];
                        degrees[j + 1] = tempDegree;

                        int tempNode = nodeIdList[j];
                        nodeIdList[j] = nodeIdList[j + 1];
                        nodeIdList[j + 1] = tempNode;
                    }
                }
            }

            return nodeIdList;
        }


        public List<int[]> GetArcByWeight()
        {
            List<int[]> arcsList = new List<int[]>();
            List<double> weightsList = new List<double>();
            List<int> visitedNodes = new List<int>();

            for (int i = 0; i < incidenceMatrix.Count; i++)
            {
                for (int j = 0; j < incidenceMatrix[i].Count; j++)
                {
                    visitedNodes.Add(i);
                    if (!visitedNodes.Contains(incidenceMatrix[i][j]))
                    {
                        weightsList.Add(weights[i][j]);
                        arcsList.Add(new int[2] { i, incidenceMatrix[i][j] });
                    }
                }
            }

            for (int i = 0; i < weightsList.Count; i++)
            {
                for (int j = 0; j < weightsList.Count - 1; j++)
                {
                    if (weightsList[j] > weightsList[j + 1])
                    {
                        double tempWeight = weightsList[j];
                        weightsList[j] = weightsList[j + 1];
                        weightsList[j + 1] = tempWeight;

                        int[] tempArc = arcsList[j];
                        arcsList[j] = arcsList[j + 1];
                        arcsList[j + 1] = tempArc;
                    }
                }
            }

            return arcsList;
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
            if (visitedNodes.Count < nodesList.Count)
            {
                if (!visitedNodes.Contains(currentNodeId))
                {
                    visitedNodes.Add(currentNodeId);
                    foreach (int nodeId in incidenceMatrix[currentNodeId])
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
                    if (!visitedNodes.Contains(nodeId) && !nodesToVisit.Contains(nodeId))
                    {
                        nodesToVisit.Enqueue(nodeId);
                    }
                }
                visitedNodes.Add(currentNodeId);
                if (nodesToVisit.Count > 0)
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
            double[] distances = new double[nodesList.Count];
            int[] predecessors = new int[nodesList.Count];
            List<int> visitedNodes = new List<int>();
            for (int i = 0; i < distances.Length; i++)
                distances[i] = double.MaxValue - 1;
            distances[0] = double.MaxValue;
            distances[startingNodeId] = 0;
            int currentNodeId = 0;

            while (currentNodeId != endingNodeId)
            {
                currentNodeId = GetMinElementId(distances);
                distances[currentNodeId] = double.MaxValue;
                for (int i = 0; i < incidenceMatrix[currentNodeId].Count; i++)
                {
                    if (!visitedNodes.Contains(incidenceMatrix[currentNodeId][i]) && distances[incidenceMatrix[currentNodeId][i]] > weights[currentNodeId][i])
                    {
                        distances[incidenceMatrix[currentNodeId][i]] = weights[currentNodeId][i];
                        predecessors[incidenceMatrix[currentNodeId][i]] = currentNodeId;
                    }
                }
                visitedNodes.Add(currentNodeId);
            }

            visitedNodes = new List<int>() { endingNodeId };
            while (currentNodeId != startingNodeId)
            {
                currentNodeId = predecessors[currentNodeId];
                visitedNodes.Add(currentNodeId);
            }
            visitedNodes.Reverse();

            return visitedNodes;
        }


        public List<int> BellmanFord(int startingNodeId, int endingNodeId)
        {
            double[] distances = new double[nodesList.Count];
            int[] predecessors = new int[nodesList.Count];
            List<int> visitedNodes = new List<int>() { endingNodeId };
            for (int i = 0; i < distances.Length; i++)
                distances[i] = double.MaxValue - 1;
            distances[0] = double.MaxValue;
            distances[startingNodeId] = 0;

            for (int cpt = 0; cpt < size; cpt++)
            {
                for (int i = 1; i < incidenceMatrix.Count; i++)
                {
                    for (int j = 0; j < incidenceMatrix[i].Count; j++)
                    {
                        if (distances[i] + Weights[i][j] < distances[incidenceMatrix[i][j]])
                        {
                            distances[incidenceMatrix[i][j]] = distances[i] + Weights[i][j];
                            predecessors[incidenceMatrix[i][j]] = i;
                        }
                    }
                }
            }

            int currentNodeId = endingNodeId;
            while (currentNodeId != startingNodeId)
            {
                currentNodeId = predecessors[currentNodeId];
                visitedNodes.Add(currentNodeId);
            }
            visitedNodes.Reverse();

            return visitedNodes;
        }


        public List<int> FloydWarshall(int startingNodeId, int endingNodeId)
        {
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

            for (int k = 0; k < nodesCount; k++)
            {
                for (int i = 0; i < nodesCount; i++)
                {
                    for (int j = 0; j < nodesCount; j++)
                    {
                        if (distances[i, k] + distances[k, j] < distances[i, j])
                        {
                            distances[i, j] = distances[i, k] + distances[k, j];
                            pathMatrix[i, j] = pathMatrix[i, k];
                        }
                    }
                }
            }

            int currentNodeId = startingNodeId;
            while (currentNodeId != endingNodeId)
            {
                visitedNodes.Add(currentNodeId);
                currentNodeId = pathMatrix[currentNodeId, endingNodeId];
            }
            visitedNodes.Add(endingNodeId);

            return visitedNodes;
        }


        #endregion


        #region coloration de graphe


        public Dictionary<int, List<int>> Welsh_Powell()
        {
            /// <summary>
            /// Implémentation de l'algorithme de Welsh-Powell. La fonction renvoie
            /// un dictionnaire qui associe chaque couleur à une liste d'Id de noeuds.
            /// </summary>
            Dictionary<int, List<int>> colorMap = new Dictionary<int, List<int>>();
            List<int> nodesIdList = GetNodesByDegree();
            int index = 0;
            foreach (int nodeId in nodesIdList)
            {
                bool colored = false;
                foreach (int k in colorMap.Keys)
                {
                    if (!colored) /// <remarks>Boucle sur les couleurs déjà attribuées pour savoir si le noeud courant peut avoir la même couleur.</remarks>
                    {
                        colored = true;
                        for (int i = 0; i < colorMap[k].Count && colored; i++)
                        {
                            if (incidenceMatrix[colorMap[k][i]].Contains(nodeId) || incidenceMatrix[nodeId].Contains(colorMap[k][i]))
                            {
                                colored = false;
                            }
                        }
                        if (colored) /// <remarks>Si cette variable est true, alors aucun le noeud courant n'est voisin d'aucun noeud de la couleur courante.</remarks>
                        {
                            colorMap[k].Add(nodeId);
                        }
                    }
                }
                if (!colored) /// <remarks>Ce cas corrspond à un noeud voisin de toutes les couleurs. ON en crée donc une nouvelle</remarks>
                {
                    colorMap[colorMap.Keys.Count] = new List<int>() { nodeId };
                }
            }
            return colorMap;
        }


        #endregion


        #region arbres couvrants

        public List<int[]> Kruskal()
        {
            /// <summary>
            /// Implémente l'algorithme de Kruskal. Les arêtes sont récupérées
            /// directement triées par poids. 
            /// </summary>
            List<int[]> treeArcs = new List<int[]>();
            List<int[]> sortedArcs = GetArcByWeight();
            List<int> visitedNodes = new List<int>();

            foreach (int[] arc in sortedArcs)
            {
                if (!visitedNodes.Contains(arc[0]) || !visitedNodes.Contains(arc[1]))
                {
                    treeArcs.Add(arc);
                    visitedNodes.Add(arc[0]);
                    visitedNodes.Add(arc[1]);
                }
            }

            return treeArcs;
        }


        public List<int[]> Prim()
        {
            /// <summary>
            /// Implémente l'algorithme de Prim pas nécessairement de la façon la plus
            /// optimisée possible.
            /// </summary>
            List<int[]> treeArcs = new List<int[]>();
            List<int> visitedNodes = new List<int>() { 0 };
            List<int[]> currentArcs = new List<int[]>();

            while (visitedNodes.Count < nodesList.Count)
            {
                int index = 0;
                while (index < currentArcs.Count)
                {
                    if (visitedNodes.Contains(currentArcs[index][0]) && visitedNodes.Contains(currentArcs[index][1]))
                    {
                        currentArcs.Remove(currentArcs[index]);
                    }
                    else
                    {
                        index++;
                    }
                }

                for (int i = 0; i < incidenceMatrix[visitedNodes[visitedNodes.Count - 1]].Count; i++)
                {
                    if (!visitedNodes.Contains(incidenceMatrix[visitedNodes[visitedNodes.Count - 1]][i]))
                    {
                        currentArcs.Add(new int[2] { visitedNodes[visitedNodes.Count - 1], incidenceMatrix[visitedNodes[visitedNodes.Count - 1]][i] });
                    }
                }

                int minWeightIndex = 0;
                double minWeight = double.MaxValue;

                for (int i = 0; i < currentArcs.Count; i++)
                {
                    int currentArcIndex = 0;
                    while (incidenceMatrix[currentArcs[i][0]][currentArcIndex] != currentArcs[i][1])
                    {
                        currentArcIndex++;
                    }
                    if (weights[currentArcs[i][0]][currentArcIndex] < minWeight)
                    {
                        minWeight = weights[currentArcs[i][0]][currentArcIndex];
                        minWeightIndex = i;
                    }
                }

                treeArcs.Add(currentArcs[minWeightIndex]); ;
                visitedNodes.Add(currentArcs[minWeightIndex][1]);
            }

            return treeArcs;
        }

        #endregion
    }
}
