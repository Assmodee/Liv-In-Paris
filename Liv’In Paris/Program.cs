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

            /// <summary>Récupère les différentes informations sur le métro (stations, connexions, et temps de changement
            /// entre deux stations, et les enregistre dans les structures de données correspondantes. 
            /// 'stations' enregistre les stations sous forme de 'Node' avec un id correspondant à l'indice de la liste 
            /// et une value correspondant au nom de la station.
            /// Ex : stations[1] -> Node(id=1, value="La Defense").
            /// 'connexions' enregistre les liens entre les stations avec l'indice de la ligne correspondant à l'indice
            /// de la stationqui partage les liens.
            /// Ex : connexions[1] -> 2 (1 correspond à 'La Defense' et 2 correspond à 'Esplanade de la Defense')
            /// commuteTime enregistre les temps de changement entre chaque station en minutes.
            /// Ex : commuteTime[1] -> 0.47 (Correspond à connexion[1], il faut 47 secondes pour changer de 'La Defense'
            /// à 'Esplanade de la Defense'.
            /// </summary>
            List<Node<string>> stations = GetStationsList(stationsFile);
            List<List<int>> connexions = GetConnexions(connexionsFile);
            List<List<double>> commuteTime = GetCommuteTime(connexionsFile);

            /// Instancie le graphe avec les données récupérées.
            Graph<string> metroGraph = new Graph<string>(connexions, stations, commuteTime);

            Console.WriteLine("Program finished ...");
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


        static List<List<int>> GetConnexions(string connexions)
        {
            List<List<int>> links = new List<List<int>>();
            links.Add(new List<int>());
            links[0].Add(0);
            string[] lines = connexions.Split('\n');
            for (int i = 1; i < lines.Length-1; i++)
            {
                string[] tokens = lines[i].Split(';');
                int id1 = int.Parse(tokens[0]);
                int id2 = int.Parse(tokens[1]);
                if (id1 < links.Count)
                {
                    links[id1].Add(id2);
                }
                else
                {
                    int count = links.Count;
                    for (int j = 0; j < id1 - count + 1; j++)
                    {
                        links.Add(new List<int>());
                    }
                    links[links.Count - 1].Add(id2);
                }
            }
            return links;
        }


        static List<List<double>> GetCommuteTime(string connexions)
        {
            List<List<double>> weights = new List<List<double>>();
            weights.Add(new List<double>());
            weights[0].Add(0.0);
            string[] lines = connexions.Split('\n');
            for (int i = 1; i < lines.Length - 1; i++)
            {
                string[] tokens = lines[i].Split(';');
                int id1 = int.Parse(tokens[0]);
                double weight = Convert.ToDouble(tokens[3]);
                if (id1 < weights.Count)
                {
                    weights[id1].Add(weight);
                }
                else
                {
                    int count = weights.Count;
                    for (int j = 0; j < id1 - count + 1; j++)
                    {
                        weights.Add(new List<double>());
                    }
                    weights[weights.Count - 1].Add(weight);
                }
            }
            return weights;
        }
    }
}