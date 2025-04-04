﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    class Tests<T>
    {
        Graph<T> testGraph;


        public Tests(Graph<T> testGraph)
        {
            this.testGraph = testGraph;
        }


        public void Test_SQL()
        {
            SQL sql = new SQL();

            try
            {


                /// Ajouter un compte
                Console.WriteLine("Ajout d'un compte...");
                sql.AjouterCompte("securepassword", true);

                /// Ajouter un client
                Console.WriteLine("Ajout d'un client...");
                sql.AjouterClient(sql.DernierID(), "Doe", "John", "john.doe@example.com", "0768243263", "chatelet");

                /// Afficher tous les clients
                Console.WriteLine("Liste des clients :");
                sql.AfficherClients();

                /// Modifier un client
                Console.WriteLine("Modification d'un client...");
                sql.ModifierClient(sql.DernierID(), "Smith", "Jane", "jane.smith@example.com", "0768243263", "chatelet");

                /// Afficher tous les clients après modification
                Console.WriteLine("Liste des clients après modification :");
                sql.AfficherClients();

                /// Supprimer un client
                Console.WriteLine("Suppression d'un client...");
                sql.SupprimerClient(sql.DernierID());

                /// Afficher tous les clients après suppression
                Console.WriteLine("Liste des clients après suppression :");
                sql.AfficherClients();

                /// Ajouter une entreprise
                Console.WriteLine("Ajout d'une entreprise...");
                sql.AjouterEntreprise("Entreprise Test", "Referent Test", sql.DernierID(), "Station B");

                /// Afficher toutes les entreprises
                Console.WriteLine("Liste des entreprises :");
                sql.AfficherEntreprises("nom_entreprise");

                ///on supp tout pour repartir sur une meilleur base pour la suite

                sql.SupprimerEntreprise("Entreprise Test");

                /// on pose les base pour les prochains test :
                sql.AjouterClient(sql.DernierID(), "alex", "fath", "alex@example.com", "0768243263", "chatelet");
                /// Ajouter un compte

                Console.WriteLine("Ajout d'un compte...");
                sql.AjouterCompte("azerty", true);


                sql.AjouterClient(sql.DernierID(), "Matthieu", "fecamp", "matt@example.com", "0768243263", "la defense");




                /// Ajouter un consommateur
                Console.WriteLine("Ajout d'un consommateur...");
                sql.AjouterConsommateur(sql.DernierID() - 1);

                /// Afficher tous les consommateurs
                Console.WriteLine("Liste des consommateurs :");
                sql.AfficherConsommateurs("id_consommateur");

                /// Ajouter un cuisinier
                Console.WriteLine("Ajout d'un cuisinier...");
                sql.AjouterCuisinier("Chef A", sql.DernierID());

                /// Afficher tous les cuisiniers
                Console.WriteLine("Liste des cuisiniers :");
                var cuisiniers = sql.AfficherCuisiniers();
                foreach (var cuisinier in cuisiniers)
                {
                    Console.WriteLine(cuisinier);
                }

                /// Ajouter un ingrédient
                Console.WriteLine("Ajout d'un ingrédient...");
                sql.AjouterIngredient("Tomate");

                /// Ajouter un plat
                Console.WriteLine("Ajout d'un plat...");
                sql.AjouterMet("Pizza", 10, "Plat principal", "Végétarien", "Italienne", 1, 1);

                /// Associer un ingrédient à un plat
                Console.WriteLine("Association d'un ingrédient à un plat...");
                sql.AssocierIngredientAMet(1, "Tomate");

                /// Afficher les ingrédients d'un plat
                Console.WriteLine("Ingrédients du plat :");
                var ingredients = sql.AfficherIngredientsParPlat(1);
                foreach (var ingredient in ingredients)
                {
                    Console.WriteLine(ingredient);
                }

                /// Ajouter une commande
                Console.WriteLine("Ajout d'une commande...");
                sql.AjouterCommande(DateTime.Now, DateTime.Now.AddDays(5), 1, 1);

                /// Afficher toutes les commandes
                Console.WriteLine("Liste des commandes :");
                var commandes = sql.AfficherCommandes();
                foreach (var commande in commandes)
                {
                    Console.WriteLine(commande);
                }

                /// Ajouter un plat dans une commande
                Console.WriteLine("Ajout d'un plat dans une commande...");
                sql.AjouterPlatDansCommande(sql.DernierId_commande(), 1, 2);

                /// Afficher toutes les commandes
                Console.WriteLine("Liste des commandes :");

                foreach (var commande in commandes)
                {
                    Console.WriteLine(commande);
                }

                /// Noter une commande
                Console.WriteLine("Noter une commande...");
                sql.NoterCommande(sql.DernierId_commande(), 1, 1, 2, "manque dingredient cest un peu simple non ?", 4, "Merci");

                /// Afficher des statistiques
                Console.WriteLine("Statistiques :");
                sql.AfficherLivraisonsParCuisinier();
                sql.AfficherCommandesParPeriode(DateTime.Now.AddDays(-7), DateTime.Now);
                sql.AfficherMoyennePrixCommandes();

                sql.AfficherCommandesConsomateurParOrigineEtPeriode(1, "Italienne", DateTime.Now.AddDays(-7), DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite : " + ex.Message);
            }
            finally
            {
                /// Fermer la connexion
                sql.Close();
            }
        }


        public string minutesToTime(double minutes)
        {
            double time = minutes;
            int hours = (int)time / 60;
            time -= hours * 60;
            int min = (int)time;
            time = (int)((time - (int)time) * 100);
            min += (int)time / 60;
            time -= (int)(time / 60) * 60;
            int seconds = (int)time;
            return "" + hours + "h " + min + "m " + seconds + "s;";
        }


        public void displayPathInfo(List<int> path)
        {
            double totalWeight = 0;
            for (int i = 0; i < path.Count; i++)
            {
                Console.Write(testGraph.NodesList[path[i]].toString() + " "); /// Affiche la prochaine station
                if (i != path.Count - 1)
                {
                    for (int j = 0; j < testGraph.IncidenceMatrix[path[i]].Count; j++)
                    {
                        if (testGraph.IncidenceMatrix[path[i]][j] == path[i + 1])
                        {
                            totalWeight += testGraph.Weights[path[i]][j]; /// Récupère le poids associé à la connexion
                        }
                    }
                }
            }
            Console.WriteLine("\n\nTotal : " + minutesToTime(totalWeight)); /// AFfiche le poids total au format date
        }


        public void Separator()
        {
            Console.WriteLine();
            for (int i = 0; i < 209; i++)
            {
                Console.Write("=");
            }
            Console.WriteLine();
        }


        public void TestFunction()
        {
            Stopwatch sw = new Stopwatch();

            Console.WriteLine("Tests Graphe ...\nDFS test : ");
            List<int> DFSNodes = new List<int>(); /// La fonction DFS remplit une liste de noeuds
            testGraph.DFS(DFSNodes);
            foreach (int i in DFSNodes)
            {
                Console.Write(testGraph.NodesList[i].toString() + " | "); /// Affiche la liste des noeuds par DFS
            }
            Console.WriteLine("\n" + DFSNodes.Count + " Stations");

            Separator();

            Console.WriteLine("\nBFS test : ");
            List<int> BFSNodes = new List<int>();
            testGraph.BFS(BFSNodes); /// Remplit la liste par BFS
            foreach (int i in BFSNodes)
            {
                Console.Write(testGraph.NodesList[i].toString() + " | "); /// Affiche la liste des noeuds par BFS
            }
            Console.WriteLine("\n" + BFSNodes.Count + " Stations");

            Separator();
            Separator();

            List<int> dijkstraPath, bellmanFordPath, floydWarshallPath;
            Console.WriteLine("\nPlus court chemin de République à Saint-Mandé : (Dijkstra)\n");
            sw.Start();
            dijkstraPath = testGraph.Dijkstra(67, 23); /// Applique l'algorithme de Dijkstra
            sw.Stop();
            displayPathInfo(dijkstraPath);
            Console.WriteLine(sw.Elapsed);
            sw.Reset();

            Separator();

            Console.WriteLine("\nPlus court chemin de Porte de La Défense à Châteu de Vincennes : (Dijkstra)\n");
            sw.Start();
            dijkstraPath = testGraph.Dijkstra(1, 25);
            sw.Stop();
            displayPathInfo(dijkstraPath);
            Console.WriteLine(sw.Elapsed);
            sw.Reset();

            Separator();

            Console.WriteLine("\nPlus court chemin de République à Saint-Mandé : (Bellman-Ford)\n");
            sw.Start();
            bellmanFordPath = testGraph.BellmanFord(67, 23); /// Applique Bellman-Ford
            sw.Stop();
            displayPathInfo(bellmanFordPath);
            Console.WriteLine(sw.Elapsed);
            sw.Reset();

            Separator();

            Console.WriteLine("\nPlus court chemin de La Défense à Château de Vincennes : (Bellman-Ford)\n");
            sw.Start();
            bellmanFordPath = testGraph.BellmanFord(1, 25);
            sw.Stop();
            displayPathInfo(bellmanFordPath);
            Console.WriteLine(sw.Elapsed);
            sw.Reset();

            Separator();

            Console.WriteLine("\nPlus court chemin de République à Saint-Mandé : (Floyd-Warshall)\n");
            sw.Start();
            floydWarshallPath = testGraph.FloydWarshall(67, 23); /// Applique Floyd-Warshall
            sw.Stop();
            displayPathInfo(floydWarshallPath);
            Console.WriteLine(sw.Elapsed);
            sw.Reset();

            Separator();

            Console.WriteLine("\nPlus court chemin de La Défense à Château de Vincennes : (Floyd-Warshall)\n");
            sw.Start();
            floydWarshallPath = testGraph.FloydWarshall(1, 25);
            sw.Stop();
            displayPathInfo(floydWarshallPath);
            Console.WriteLine(sw.Elapsed);
            sw.Reset();

            Separator();
        }



        


    }
}
