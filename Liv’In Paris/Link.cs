using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    internal class Link
    {
        Node node1;
        Node node2;


        public Link(Node node1, Node node2)
        {
            this.node1 = node1;
            this.node2 = node2;
        }


        public Node Node1 { get { return node1; } }


        public Node Node2 { get { return node2; } }


        public string toString()
        {
            return node1.toString() + " -> " + node2.toString();
        }
    }
}
