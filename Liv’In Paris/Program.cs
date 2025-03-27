using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Liv_In_Paris
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string stationsFile = File.ReadAllText("Stations.txt");
            string connexionsFile = File.ReadAllText("Connexions.txt");

            List<Node<string>> stations = GetStationsList(stationsFile);
            double[,] connexions = GetConnexions(connexionsFile);

            Console.ReadKey();
        }


        static List<Node<string>> GetStationsList(string stations)
        {
            List<Node<string>> nodes = new List<Node<string>>();
            nodes.Add(new Node<string>(0, ""));
            string[] lines = stations.Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(';');
                int id = int.Parse(tokens[0]);
                string stationName = "";
                foreach (char c in tokens[1])
                {
                    if (c != '\r')
                    {
                        stationName += c;
                    }
                }
                nodes.Add(new Node<string>(id, stationName));
            }
            return nodes;
        }


        static double[,] GetConnexions(string connexions)
        {
            double[,] links = new double[302, 302];
            string[] lines = connexions.Split('\n');
            for (int i = 1; i < lines.Length-1; i++)
            {
                string[] tokens = lines[i].Split(';');
                int id1 = int.Parse(tokens[0]);
                int id2 = int.Parse(tokens[1]);
                double weight = Convert.ToDouble(tokens[3]);
                links[id1, id2] = weight;
            }
            return links;
        }
    }
}