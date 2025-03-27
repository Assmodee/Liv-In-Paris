﻿using System;
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


        public Graph(List<List<int>> incidenceMatrix, List<Node<T>> nodesList, List<List<double>> weights)
        {
            this.incidenceMatrix = incidenceMatrix;
            this.nodesList = nodesList;
            this.weights = weights;
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
    }
}
