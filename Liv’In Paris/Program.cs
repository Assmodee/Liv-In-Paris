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

            test();

            //string stationsFile = File.ReadAllText("Stations.txt");
            //string connexionsFile = File.ReadAllText("Connexions.txt");
            //Dictionary<string, int> rStationsDic = new Dictionary<string, int>();

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
            // List<Node<string>> stations = GetStationsList(stationsFile, rStationsDic);
            // List<List<int>> connexions = GetConnexions(connexionsFile);
            // List<List<double>> commuteTime = GetCommuteTime(connexionsFile);

            /// Instancie le graphe avec les données récupérées.
            //Graph<string> metroGraph = new Graph<string>(connexions, stations, commuteTime, rStationsDic);

            //Tests<string> t = new Tests<string>(metroGraph);
            //t.TestFunction();

            //Console.WriteLine("\n\nProgram finished ...");
            //Console.ReadKey();
        }


        static void test()
        {
            SQL sql = new SQL();

            try
            {
                // Exemple d'utilisation des fonctions



                // Ajouter un compte
                Console.WriteLine("Ajout d'un compte...");
                sql.AjouterCompte("securepassword", true);

                // Ajouter un client
                Console.WriteLine("Ajout d'un client...");
                sql.AjouterClient(1, "Doe", "John", "john.doe@example.com", "0768243263", "chatelet");

                // Afficher tous les clients
                Console.WriteLine("Liste des clients :");
                sql.AfficherClients();

                // Modifier un client
                Console.WriteLine("Modification d'un client...");
                sql.ModifierClient(1, "Smith", "Jane", "jane.smith@example.com", "0768243263", "chatelet");

                // Afficher tous les clients après modification
                Console.WriteLine("Liste des clients après modification :");
                sql.AfficherClients();

                // Supprimer un client
                Console.WriteLine("Suppression d'un client...");
                sql.SupprimerClient(1);

                // Afficher tous les clients après suppression
                Console.WriteLine("Liste des clients après suppression :");
                sql.AfficherClients();

                // -------------------------------------

                // Ajouter une entreprise
                Console.WriteLine("Ajout d'une entreprise...");
                sql.AjouterEntreprise("Entreprise Test", "Referent Test", 1, "Station B");

                // Afficher toutes les entreprises
                Console.WriteLine("Liste des entreprises :");
                sql.AfficherEntreprises("nom_entreprise");

                //on supp tout pour repartir sur une meilleur base pour la suite

               sql.SupprimerEntreprise( "Entreprise Test" );


                // ------------------------------------
                // on pose les base pour les prochains test :
                // Ajouter un compte
                Console.WriteLine("Ajout d'un compte...");
                sql.AjouterCompte("azerty", true);

                sql.AjouterClient(1, "alex", "fath", "alex@example.com", "0768243263", "chatelet");
                sql.AjouterClient(2, "Matthieu", "fecamp", "matt@example.com", "0768243263", "la defense");




                // Ajouter un consommateur
                Console.WriteLine("Ajout d'un consommateur...");
                sql.AjouterConsommateur(1);

                // Afficher tous les consommateurs
                Console.WriteLine("Liste des consommateurs :");
                sql.AfficherConsommateurs("id_consommateur");

                // Ajouter un cuisinier
                Console.WriteLine("Ajout d'un cuisinier...");
                sql.AjouterCuisinier("Chef A", 2);

                // Afficher tous les cuisiniers
                Console.WriteLine("Liste des cuisiniers :");
                var cuisiniers = sql.AfficherCuisiniers();
                foreach (var cuisinier in cuisiniers)
                {
                    Console.WriteLine(cuisinier);
                }

                // Ajouter un ingrédient
                Console.WriteLine("Ajout d'un ingrédient...");
                sql.AjouterIngredient("Tomate");

                // Ajouter un plat
                Console.WriteLine("Ajout d'un plat...");
                sql.AjouterMet("Pizza", 10, "Plat principal", "Végétarien", "Italienne", 1 , 1);

                // Associer un ingrédient à un plat
                Console.WriteLine("Association d'un ingrédient à un plat...");
                sql.AssocierIngredientAMet(1, "Tomate");

                // Afficher les ingrédients d'un plat
                Console.WriteLine("Ingrédients du plat :");
                var ingredients = sql.AfficherIngredientsParPlat(1);
                foreach (var ingredient in ingredients)
                {
                    Console.WriteLine(ingredient);
                }

                // Ajouter une commande
                Console.WriteLine("Ajout d'une commande...");
                sql.AjouterCommande(20, 1, DateTime.Now, DateTime.Now.AddDays(5), 1, 1);

                // Afficher toutes les commandes
                Console.WriteLine("Liste des commandes :");
                var commandes = sql.AfficherCommandes();
                foreach (var commande in commandes)
                {
                    Console.WriteLine(commande);
                }

                // Ajouter un plat dans une commande
                Console.WriteLine("Ajout d'un plat dans une commande...");
                sql.AjouterPlatDansCommande(1, 1, 1);

                // Afficher toutes les commandes
                Console.WriteLine("Liste des commandes :");
                
                foreach (var commande in commandes)
                {
                    Console.WriteLine(commande);
                }

                // Noter une commande
                Console.WriteLine("Noter une commande...");
                sql.NoterCommande(1, 1, 1, 2, "manque dingredient cest un peu simple non ?", 4, "Merci");

                // Afficher des statistiques
                Console.WriteLine("Statistiques :");
                sql.AfficherLivraisonsParCuisinier();
                sql.AfficherCommandesParPeriode(DateTime.Now.AddDays(-7), DateTime.Now);
                sql.AfficherMoyennePrixCommandes();
                sql.AfficherMoyenneAchatsClients();
                sql.AfficherCommandesConsomateurParOrigineEtPeriode(1, "Italienne", DateTime.Now.AddDays(-7), DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite : " + ex.Message);
            }
            finally
            {
                // Fermer la connexion
                sql.Close();
            }
        }




        #region Récupération de données

        static List<Node<string>> GetStationsList(string stations, Dictionary<string, int> dic)
        {
            List<Node<string>> nodes = new List<Node<string>>();
            nodes.Add(new Node<string>(0, "")); /// Le premier noeud ne correspond à aucune station (problème d'index), il est donc vide
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
                nodes.Add(new Node<string>(id, stationName));
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
            for (int i = 1; i < lines.Length-1; i++)
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
                double weight = Convert.ToDouble(tokens[3]);
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