using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    class Graph<T>
    {
        double[,] incidenceMatrix;
        List<Node<T>> nodesList;


        public Graph(double[,] incidenceMatrix, List<Node<T>> nodesList)
        {
            this.incidenceMatrix = incidenceMatrix;
            this.nodesList = nodesList;
        }
    }
}
