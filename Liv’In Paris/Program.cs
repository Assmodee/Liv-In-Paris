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
            Dictionary<string, int> rStationsDic = new Dictionary<string, int>();

            /// <summary>Récupère les différentes informations sur le métro (stations, connexions, et temps de changement
            /// entre deux stations), et les enregistre dans les structures de données correspondantes. 
            /// 'stations' enregistre les stations sous forme de 'Node' avec un id correspondant à l'indice de la liste 
            /// et une value correspondant au nom de la station.
            /// Ex : stations[1] -> Node(id=1, value="La Defense").
            /// 'connexions' enregistre les liens entre les stations avec l'indice de la ligne correspondant à l'indice
            /// de la station qui partage les liens.
            /// Ex : connexions[1] -> 2 (1 correspond à 'La Defense' et 2 correspond à 'Esplanade de la Defense')
            /// commuteTime enregistre les temps de changement entre chaque station en minutes.
            /// Ex : commuteTime[1] -> 0.47 (Correspond à connexion[1], il faut 47 secondes pour changer de 'La Defense'
            /// à 'Esplanade de la Defense'.
            /// </summary>
            List<Node<string>> stations = GetStationsList(stationsFile, rStationsDic);
            List<List<int>> connexions = GetConnexions(connexionsFile);
            List<List<double>> commuteTime = GetCommuteTime(connexionsFile);

            /// Instancie le graphe avec les données récupérées.
            Graph<string> metroGraph = new Graph<string>(connexions, stations, commuteTime, rStationsDic);

            Tests<string> t = new Tests<string>(metroGraph);
            t.TestFunction();

            Drawing.DrawGraphFromCoordinates(stations, connexions, "graphe_oriente.png");

            Console.WriteLine("\n\nProgram finished ...");
            Console.ReadKey();
        }

        #region Récupération de données

        static List<Node<string>> GetStationsList(string stations, Dictionary<string, int> dic)
        {
            List<Node<string>> nodes = new List<Node<string>>();
            nodes.Add(new Node<string>(0, "", 0, 0)); /// Le premier noeud ne correspond à aucune station (problème d'index), il est donc vide
            string[] lines = stations.Split('\n'); /// Sépare le texte selon les lignes
            for (int i = 1; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(';'); /// Sépare les lignes selon les ; pour obtenir les différentes informations
                int id = int.Parse(tokens[0]); /// Correspond à l'id de la station
                string stationName = "";
                foreach (char c in tokens[1])
                {
                    if (c != '\r') /// Evite le retour chariot sinon ça foire l'affichage
                    {
                        stationName += c;
                    }
                }


                double lon = -1.0;
                double lat = -1.0;

                double lonVal;
                if (double.TryParse(tokens[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out lonVal))
                {
                    lon = lonVal;
                }

                double latVal;
                if (double.TryParse(tokens[3], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out latVal))
                {
                    lat = latVal;
                }

                nodes.Add(new Node<string>(id, stationName, lon, lat));
                dic[stationName] = id;
            }
            return nodes;
        }


        static List<List<int>> GetConnexions(string connexions)
        {
            List<List<int>> links = new List<List<int>>();
            links.Add(new List<int>());
            links[0].Add(0); /// Le premier noeud étant vide, il pointe vers lui-même
            string[] lines = connexions.Split('\n'); /// Même fonctionnement que pour obtenir les stations
            for (int i = 1; i < lines.Length - 1; i++)
            {
                string[] tokens = lines[i].Split(';');
                int id1 = int.Parse(tokens[0]); /// id de la station de départ
                int id2 = int.Parse(tokens[1]); /// id de la station d'arrivée
                if (id1 < links.Count)
                {
                    links[id1].Add(id2);
                }
                else /// Ajoute autant de List que nécessaire pour la matrice d'incidence (triée par ordre croissant d'id)
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
            weights[0].Add(0.0); /// La première connexion est inutile, on intialise à 0.0
            string[] lines = connexions.Split('\n'); /// Encore meêm fonctionnement
            for (int i = 1; i < lines.Length - 1; i++)
            {
                string[] tokens = lines[i].Split(';');
                int id1 = int.Parse(tokens[0]); /// Récupère l'id de la station de départ qui correspond à l'indice dans la matrice d'incidence
                                                ///double weight = Convert.ToDouble(tokens[3]);/// probleme avec virgule et point( systéme en français ok mais espagnol, divisé par virgule donc pas ok
                double weight = double.Parse(tokens[3], System.Globalization.CultureInfo.InvariantCulture);///solution probleme separateur virgule
                if (id1 < weights.Count)
                {
                    weights[id1].Add(weight);
                }
                else /// Même système que pour récupérer les connexions
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

        #endregion
    }
}