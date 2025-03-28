using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    class Graph<T>
    {
        List<List<int>> incidenceMatrix;
        List<Node<T>> nodesList;
        List<List<double>> weights;
        Dictionary<string, int> reverseIdDic;


        public Graph(List<List<int>> incidenceMatrix, List<Node<T>> nodesList, List<List<double>> weights, Dictionary<string, int> reverseIdDic)
        {
            this.incidenceMatrix = incidenceMatrix;
            this.nodesList = nodesList;
            this.weights = weights;
            this.reverseIdDic = reverseIdDic;
        }


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


        public Dictionary<string, int> ReverseIdDic
        {
            get { return reverseIdDic; }
        }
    }
}
