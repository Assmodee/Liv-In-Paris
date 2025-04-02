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
        double longitude;
        double latitude;


        public Node(int id, T value, double longitude, double latitude)
        {
            this.id = id;
            this.value = value;
            this.longitude = longitude;
            this.latitude = latitude;
        }


        public int Id
        {
            get { return id; }
        }


        public T Value
        {
            get { return value; }
        }


        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }


        public string toString()
        {
            return "[" + value.ToString() + ", " + " Lon : "+longitude + " Lat : " + latitude + "]";
        }
    }
}
