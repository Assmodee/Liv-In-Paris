using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    internal class Node
    {
        int value;
        List<int> links;


        public Node(int value, List<int> links)
        {
            this.value = value;
            this.links = links;
        }


        public int Value { get { return value; } }


        public List<int> Links { get { return links; } }


        public string toString()
        {
            return '[' + value.ToString() + ']';
        }
    }
}
