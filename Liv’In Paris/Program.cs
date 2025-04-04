using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Liv_In_Paris
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SQL sql = new SQL();

           

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
            Drawing.DrawGraphFromCoordinates(stations, connexions, "graphe_oriente.png");
            t.TestFunction();
            Console.WriteLine("Tests SQL ...\n");
            t.Test_SQL();
            Console.WriteLine("\nTests SQL terminés, la prochaine partie concerne l'interaction avec l'application. Appuyer pour continuer ...");
            Console.ReadKey();

            menu1(sql, metroGraph);

           

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
                stationName.ToLower();


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


        static void menu1(SQL sql, Graph<string> graph)
        {
            Console.Clear();
            int nb = 0;
            do
            {
                Console.Clear();

                string display = "[1] Créer un compte\n[2] se connecter\n\n\n\n\nchosir option: ";
                Console.WriteLine(display);
                nb = int.Parse(Console.ReadLine());

            } while (nb != 1 && nb != 2);

            if (nb == 1)
            {
                CreerCompte(sql, graph);
            }
            else
            {
                

                Connect(sql);
            }

        }


        static void CreerCompte(SQL sql, Graph<string> stationsGraph)
        {
            string mdp;
            bool estUtilisateur;

            bool test = false;
            Console.Clear();
            do
            {

                Console.WriteLine("mot de passe :");
                mdp = Console.ReadLine();

                Console.WriteLine(" etes vous un utilisateur (true) ou une entreprise (false):");
                estUtilisateur = Convert.ToBoolean(Console.ReadLine());
                sql.AjouterCompte(mdp, estUtilisateur);

                if (estUtilisateur == true)
                {
                        string nom;
                        string prenom;
                        string email;
                        string Tel;
                        string Metro_le_plus_proche="";

                        Console.WriteLine("nom:");
                        nom = Console.ReadLine();
                    Console.WriteLine("prenom:");
                    prenom = Console.ReadLine();
                    Console.WriteLine("email:");
                    email = Console.ReadLine();
                    bool correct = false;
                    do
                    {
                        Console.WriteLine("numero de telephone :");
                        Tel = Console.ReadLine();
                        int response;
                        correct = int.TryParse(Tel, out response);

                    } while (!correct);
                    //do
                    //{
                        Console.WriteLine("Station de metro la plus proche:");
                        Metro_le_plus_proche = Console.ReadLine().ToLower();

                    
                    //} while (stationsGraph.ReverseIdDic.Keys.Contains(Metro_le_plus_proche));



                    sql.AjouterClient(sql.DernierID(), nom, prenom, email, Tel, Metro_le_plus_proche);

                    test = true;
                }
                else
                {
                    string nomEntreprise; string nomReferent; int ID; string Metro_le_plus_proche="";
                    Console.WriteLine("nom Entreprise :");
                    nomEntreprise = Console.ReadLine();
                    Console.WriteLine("nom referent :");
                    nomReferent = Console.ReadLine();
                    //do
                    //{
                        Console.WriteLine("Station de metro la plus proche:");
                        Metro_le_plus_proche = Console.ReadLine().ToLower();

                    //} while (stationsGraph.ReverseIdDic.Keys.Contains(Metro_le_plus_proche));

                    sql.AjouterEntreprise(nomEntreprise,nomReferent, sql.DernierID(), Metro_le_plus_proche);
                    test = true;
                }


            } while (!test);

            Console.WriteLine(sql.DernierID());
            Console.ReadLine();

            menu1(sql, stationsGraph);
        }

        static void Connect(SQL sql)
        {
            Console.Clear();
            int ID=0;
            string verif;
            bool correct = false;
            string mdp = "";
            bool existe = false;

            do
            {

                do
                {
                    Console.WriteLine("ID :");
                    verif = Console.ReadLine();

                } while (!int.TryParse(verif, out ID));

                Console.WriteLine("mot de passe :");
                mdp = Console.ReadLine();

                existe = sql.VerifierCompte(ID, mdp);

            } while (!existe);

            Menu2(sql,ID);

        }


        static void Menu2(SQL sql,int ID)
        {
            Console.Clear();
            bool est_cuisinier = sql.rolecuisinier(ID);
            bool est_consommateur = sql.roleconsommateur(ID);
            

            if (est_cuisinier == false && est_consommateur == false) /// ni consomateur ni cuisinier  Menu00
            {
             Menu00(sql,ID);
            }

            if (est_cuisinier == true && est_consommateur == false) /// que cuisinier  Menu10
            {
                Menu10(sql,ID);
            }

            if (est_cuisinier == false && est_consommateur == true) /// que client       Menu01
            {
                Menu01(sql,ID);
            }

            if (est_cuisinier == true && est_consommateur == true) /// les deux      Menu11
            {
                Menu11(sql,ID);
            }



        }

        static void Menu00(SQL sql, int ID)
        {
              
            string display = "[1] devenir cuisinier: \n[2]devenir consommateur:";
            int action = 0;
            do {
                Console.WriteLine(display);
                action = int.Parse(Console.ReadLine()); 
            } while (action != 1 && action != 2);

            if (action == 1)
            {
                devenirCuisinier( sql,  ID);
                Menu2(sql, ID);
            }
            if (action == 2)
            {
                devenirConsommateur( sql,  ID);
                Menu2(sql, ID);
            }

        }

        static void Menu01(SQL sql, int ID)
        {
            Console.Clear();

            string display = "[1] plat disponible: \n[2]rechercher plat:\n[3]voir ses commandes\n[4]ajouter element a commande\n[5]passer une commandes\n[6]noter commandes\n[7]devenir cuisinier";
            int action = 0;

            do { 
                Console.WriteLine(display);
                action = int.Parse(Console.ReadLine()); 
            
            } while (action <1 && action >7);

            if (action == 1)
            {
                sql.AfficherTousLesMets();
                Console.WriteLine("retour appuyer entrer");
                Console.ReadLine();
                Menu2(sql, ID);

            }
            if (action == 2)
            {
                string nomplat = "";
                Console.WriteLine("chercher plat:");
                nomplat = Console.ReadLine();

                sql.ChercherEtAfficherPlat(nomplat);
                Console.WriteLine("retour appuyer entrer");
                Console.ReadLine();
                Menu2(sql, ID);
            }
            if (action == 3)
            {
                sql.AfficherCommandesParCompte(ID);
                Console.WriteLine("retour appuyer entrer");
                Console.ReadLine();
                Menu2(sql, ID);

            }
            if (action == 4)
            {
                int idCommande; int idMet; int quantite;
                do
                {
                    Console.Write("Entrez l'ID de la commande : ");
                } while (!int.TryParse(Console.ReadLine(), out idCommande));

                do
                {
                    Console.Write("Entrez l'ID du met : ");
                } while (!int.TryParse(Console.ReadLine(), out idMet));

                do
                {
                    Console.Write("Entrez la quantité : ");
                } while (!int.TryParse(Console.ReadLine(), out quantite) || quantite <= 0);



                sql.AjouterPlatDansCommande(idCommande,idMet,quantite);
                Console.WriteLine("retour appuyer entrer");
                Console.ReadLine();
                Menu2(sql, ID);

            }
            if (action == 5)
            {
                DateTime fabrication;  int idConsommateur; int idCuisinier;
                do
                {
                    Console.Write(" Entrez la date de fabrication (yyyy-MM-dd) : ");
                } while (!DateTime.TryParse(Console.ReadLine(), out fabrication));

         
               /// Demande l'ID du consommateur
                do
                {
                    Console.Write(" Entrez l'ID du consommateur : ");
                } while (!int.TryParse(Console.ReadLine(), out idConsommateur));

                /// Demande l'ID du cuisinier
                do
                {
                    Console.Write(" Entrez l'ID du cuisinier : ");
                } while (!int.TryParse(Console.ReadLine(), out idCuisinier));

                sql.AjouterCommande(fabrication, fabrication.AddDays(8), idConsommateur,idCuisinier);

                


                    int idCommande=sql.DernierIDcommande(); int idMet; int quantite;

                Console.WriteLine("ajouter combien de plats ?");
                int nb = int.Parse(Console.ReadLine());
                for (int i = 0; i < nb; i++)
                {

                    

                    do
                    {
                        Console.Write("Entrez l'ID du met : ");
                    } while (!int.TryParse(Console.ReadLine(), out idMet));

                    do
                    {
                        Console.Write("Entrez la quantité : ");
                    } while (!int.TryParse(Console.ReadLine(), out quantite) || quantite <= 0);



                    sql.AjouterPlatDansCommande(idCommande, idMet, quantite);

                }
                Console.WriteLine("retour appuyer entrer");
                Console.ReadLine();
                Menu2(sql, ID);

            }
            if (action == 6)
            {
                int idCommande; int idCuisinier; int idConsommateur; float noteClient; string commentaireClient; float noteCuisinier; string commentaireCuisinier;

                do
                {
                    Console.Write(" Entrez l'ID de la commande : ");
                } while (!int.TryParse(Console.ReadLine(), out idCommande));

                /// Demande l'ID du cuisinier
                do
                {
                    Console.Write(" Entrez l'ID du cuisinier : ");
                } while (!int.TryParse(Console.ReadLine(), out idCuisinier));

                /// Demande l'ID du consommateur
                do
                {
                    Console.Write(" Entrez l'ID du consommateur : ");
                } while (!int.TryParse(Console.ReadLine(), out idConsommateur));

                // Demande la note du client
                //do
                //{
                //    Console.Write(" Entrez la note du client (0 à 5) : ");
                //} while (!float.TryParse(Console.ReadLine(), out noteClient) || noteClient < 0 || noteClient > 5);

                //// Demande le commentaire du client
                //Console.Write(" Entrez le commentaire du client : ");
                //commentaireClient = Console.ReadLine();

                do
                {
                    Console.Write(" Entrez la note du cuisinier (0 à 5) : ");
                } while (!float.TryParse(Console.ReadLine(), out noteCuisinier) || noteCuisinier < 0 || noteCuisinier > 5);

                /// Demande le commentaire du cuisinier
                Console.Write(" Entrez le commentaire du cuisinier : ");
                commentaireCuisinier = Console.ReadLine();


                sql.NoterCommande(idCommande, idCuisinier,idConsommateur, 0,null, noteCuisinier, commentaireCuisinier);
                Console.WriteLine("retour appuyer entrer");
                Console.ReadLine();
                Menu2(sql, ID);

            }
            if (action == 7)
            {
                devenirCuisinier(sql, ID);
                Menu2(sql, ID);

            }
            else
            {
                Console.WriteLine("Action invalide.");
            }
        }

        static void Menu10(SQL sql, int ID)
        {
            Console.Clear();

            string display = "Bonjour cuisinier :D quel action souhaitez vous faire?\n\n[1] Voir mes commandes\n[2]Voir mes plats\n[3]Ajouter plat\n[4]devenir consomateur";
            int action = 0;
            do {
                Console.WriteLine(display);
                action = int.Parse(Console.ReadLine()); } while (action != 1 && action != 2 && action != 3 && action != 4 );
           

            if (action == 1)
            {
                sql.AfficherCommandesParCuisinier(ID);
                Console.WriteLine("retour appuyer entrer");
                Console.ReadLine();
                Menu2(sql, ID);
            }

            if (action == 2)
            {

                sql.AfficherTousLesMetsducuisto(sql.idducuisinier(ID));
                Console.WriteLine("retour appuyer entrer");
                Console.ReadLine();
                Menu2(sql, ID);
            }
            if (action == 3)
            {

                string nom; decimal prix; string type; string regime; string origine; int pourCombien; int idCuisinier = sql.idducuisinier(ID);
                Console.Write("🍽 Entrez le nom du met : ");
                nom = Console.ReadLine();

                // Demande du prix du met
                do
                {
                    Console.Write(" Entrez le prix du met : ");
                } while (!decimal.TryParse(Console.ReadLine(), out prix) || prix <= 0);

                // Demande du type du met
                Console.Write(" Entrez le type du met (ex: entrée, plat principal, dessert, etc.) : ");
                type = Console.ReadLine();

                // Demande du régime du met
                Console.Write("🍽 Entrez le régime du met (ex: végétarien, sans gluten, etc.) : ");
                regime = Console.ReadLine();

                // Demande de l'origine du met
                Console.Write(" Entrez l'origine du met (ex: français, italien, etc.) : ");
                origine = Console.ReadLine();

                // Demande du nombre de personnes pour lequel le met est prévu
                do
                {
                    Console.Write(" Entrez le nombre de personnes pour lequel ce met est prévu : ");
                } while (!int.TryParse(Console.ReadLine(), out pourCombien) || pourCombien <= 0);

                // Demande de l'ID du cuisinier
                

                sql.AjouterMet( nom,  prix,  type, regime,  origine,  pourCombien, idCuisinier);
                Console.WriteLine("retour appuyer entrer");
                Console.ReadLine();
                Menu2(sql, ID);
            }
            if (action == 4)
            {
                devenirConsommateur(sql, ID);
                Console.WriteLine("retour appuyer entrer");
                Console.ReadLine();
                Menu2(sql, ID);
            }
            

        }

        static void Menu11(SQL sql, int ID)
        {
            Console.Clear();
            
                

                string display = "Bonjour utilisateur hybride :)\n\n[1] Voir plats disponibles\n[2] Rechercher plat\n[3] Voir mes commandes (consommateur)\n[4] Ajouter élément à commande\n[5] Passer une commande\n[6] Noter commandes\n[7] Voir mes plats (cuisinier)\n[8] Ajouter un plat (cuisinier)";
                int action = 0;
                

                do
                {
                    Console.WriteLine(display);

                    action = int.Parse(Console.ReadLine());

                } while (action < 1 || action > 8);

                if (action == 1)
                {
                    sql.AfficherTousLesMets();
                    Console.WriteLine("retour appuyer entrer");
                    Console.ReadLine();
                    Menu2(sql, ID);
                }

                if (action == 2)
                {
                    string nomplat = "";
                    Console.WriteLine("chercher plat:");
                    nomplat = Console.ReadLine();

                    sql.ChercherEtAfficherPlat(nomplat);
                    Console.WriteLine("retour appuyer entrer");
                    Console.ReadLine();
                    Menu2(sql, ID);
                }

                if (action == 3)
                {
                    sql.AfficherCommandesParCompte(ID);
                    Console.WriteLine("retour appuyer entrer");
                    Console.ReadLine();
                    Menu2(sql, ID);
                }

                if (action == 4)
                {
                    int idCommande, idMet, quantite;
                    do
                    {
                        Console.Write("Entrez l'ID de la commande : ");
                    } while (!int.TryParse(Console.ReadLine(), out idCommande));

                    do
                    {
                        Console.Write("Entrez l'ID du met : ");
                    } while (!int.TryParse(Console.ReadLine(), out idMet));

                    do
                    {
                        Console.Write("Entrez la quantité : ");
                    } while (!int.TryParse(Console.ReadLine(), out quantite) || quantite <= 0);

                    sql.AjouterPlatDansCommande(idCommande, idMet, quantite);
                    Console.WriteLine("retour appuyer entrer");
                    Console.ReadLine();
                    Menu2(sql, ID);
                }

                if (action == 5)
                {
                    DateTime fabrication; int idConsommateur, idCuisinier;
                    do
                    {
                        Console.Write(" Entrez la date de fabrication (yyyy-MM-dd) : ");
                    } while (!DateTime.TryParse(Console.ReadLine(), out fabrication));

                    do
                    {
                        Console.Write(" Entrez l'ID du consommateur : ");
                    } while (!int.TryParse(Console.ReadLine(), out idConsommateur));

                    do
                    {
                        Console.Write(" Entrez l'ID du cuisinier : ");
                    } while (!int.TryParse(Console.ReadLine(), out idCuisinier));

                    sql.AjouterCommande(fabrication, fabrication.AddDays(8), idConsommateur, idCuisinier);

                    int idCommande = sql.DernierIDcommande(); int idMet, quantite;

                    Console.WriteLine("ajouter combien de plats ?");
                    int nb = int.Parse(Console.ReadLine());
                    for (int i = 0; i < nb; i++)
                    {
                        

                        do
                        {
                            Console.Write("Entrez l'ID du met : ");
                        } while (!int.TryParse(Console.ReadLine(), out idMet));

                        do
                        {
                            Console.Write("Entrez la quantité : ");
                        } while (!int.TryParse(Console.ReadLine(), out quantite) || quantite <= 0);

                        sql.AjouterPlatDansCommande(idCommande, idMet, quantite);
                    }
                    Console.WriteLine("retour appuyer entrer");
                    Console.ReadLine();
                    Menu2(sql, ID);
                }

                if (action == 6)
                {
                    int idCommande, idCuisinier, idConsommateur;
                    float noteCuisinier;
                    string commentaireCuisinier;

                    do
                    {
                        Console.Write(" Entrez l'ID de la commande : ");
                    } while (!int.TryParse(Console.ReadLine(), out idCommande));

                    do
                    {
                        Console.Write(" Entrez l'ID du cuisinier : ");
                    } while (!int.TryParse(Console.ReadLine(), out idCuisinier));

                    do
                    {
                        Console.Write(" Entrez l'ID du consommateur : ");
                    } while (!int.TryParse(Console.ReadLine(), out idConsommateur));

                    do
                    {
                        Console.Write(" Entrez la note du cuisinier (0 à 5) : ");
                    } while (!float.TryParse(Console.ReadLine(), out noteCuisinier) || noteCuisinier < 0 || noteCuisinier > 5);

                    Console.Write(" Entrez le commentaire du cuisinier : ");
                    commentaireCuisinier = Console.ReadLine();

                    sql.NoterCommande(idCommande, idCuisinier, idConsommateur, 0, null, noteCuisinier, commentaireCuisinier);
                    Console.WriteLine("retour appuyer entrer");
                    Console.ReadLine();
                    Menu2(sql, ID);
                }

                if (action == 7)
                {
                    sql.AfficherTousLesMetsducuisto(sql.idducuisinier(ID));
                    Console.WriteLine("retour appuyer entrer");
                    Console.ReadLine();
                    Menu2(sql, ID);
                }

                if (action == 8)
                {
                    string nom; decimal prix; string type; string regime; string origine; int pourCombien;
                    int idCuisinier = sql.idducuisinier(ID);

                    Console.Write("🍽 Entrez le nom du met : ");
                    nom = Console.ReadLine();

                    do
                    {
                        Console.Write(" Entrez le prix du met : ");
                    } while (!decimal.TryParse(Console.ReadLine(), out prix) || prix <= 0);

                    Console.Write(" Entrez le type du met (ex: entrée, plat principal, dessert, etc.) : ");
                    type = Console.ReadLine();

                    Console.Write("🍽 Entrez le régime du met (ex: végétarien, sans gluten, etc.) : ");
                    regime = Console.ReadLine();

                    Console.Write(" Entrez l'origine du met (ex: français, italien, etc.) : ");
                    origine = Console.ReadLine();

                    do
                    {
                        Console.Write(" Entrez le nombre de personnes pour lequel ce met est prévu : ");
                    } while (!int.TryParse(Console.ReadLine(), out pourCombien) || pourCombien <= 0);

                    sql.AjouterMet(nom, prix, type, regime, origine, pourCombien, idCuisinier);
                    Console.WriteLine("retour appuyer entrer");
                    Console.ReadLine();
                    Menu2(sql, ID);
                }
            
        }



        static void devenirCuisinier(SQL sql, int ID)
        {
            Console.Clear();
            string nomCuisinier = "";
            Console.WriteLine("Nom de cuisinier :");
            nomCuisinier = Console.ReadLine();
            sql.AjouterCuisinier(nomCuisinier, ID);
            Menu2(sql, ID);

        }

        static void devenirConsommateur(SQL sql, int ID)
        {
            Console.Clear();
            sql.AjouterConsommateur(ID);
            Menu2(sql, ID);

        }
    }
}