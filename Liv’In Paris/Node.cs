using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    class Node<T>
    {
        int id;
        T value;


        public Node(int id, T value)
        {
            this.id = id;
            this.value = value;
        }


        public int Id
        {
            get { return id; }
        }


        public T Value
        {
            get { return value; }
        }


        public string toString()
        {
            return "[" + value.ToString() + "]";
        }
    }
}
