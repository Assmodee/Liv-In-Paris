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


        public Node(int value)
        {
            this.value = value;
        }


        public int Value { get { return value; } }


        public string toString()
        {
            return '[' + value.ToString() + ']';
        }
    }
}
