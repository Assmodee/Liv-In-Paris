using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    internal class SQL
    {

        private static string connectionString = "Server=localhost;Database=liv_in_paris;User ID=root;Password=C@rioc@78600;";

        private MySqlConnection conn;

        // Constructeur : ouvre la connexion automatiquement
        public SQL()
        {
            conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                Console.WriteLine("Connexion réussie à la base de données !");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur de connexion : " + ex.Message);
            }
        }

        // Ferme la connexion proprement
        public void Close()
        {
            if (conn != null)
            {
                conn.Close();
                Console.WriteLine("Connexion fermée.");
            }
        }

        #region client

        //  Ajouter un client
        public void AjouterClient(int ID,string nom, string prenom, string email, string Tel,string Metro_le_plus_proche)
        {
            string query = "INSERT INTO Clients (ID,Nom, Prenom, Email,Tel,Metro_le_plus_proche) VALUES (@ID,@Nom, @Prenom, @Email, @Tel,@Metro_le_plus_proche)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@Nom", nom);
                cmd.Parameters.AddWithValue("@Prenom", prenom);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Tel",Tel);
                cmd.Parameters.AddWithValue("@Metro_le_plus_proche", Metro_le_plus_proche);

                try
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Client ajouté avec succès !");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Erreur lors de l'ajout du client : " + ex.Message);
                }
            }
        }

        //  Modifier un client
        public void ModifierClient(int id, string nom, string prenom, string email, string Tel , string Metro_le_plus_proche)
        {
            string query = "UPDATE Clients SET Nom = @Nom, Prenom = @Prenom, Email = @Email, Tel = @Tel , Metro_le_plus_proche = @Metro_le_plus_proche WHERE ID = @ID";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Nom", nom);
                cmd.Parameters.AddWithValue("@Prenom", prenom);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Tel",Tel);
                cmd.Parameters.AddWithValue("@Metro_le_plus_proche", Metro_le_plus_proche);


                try
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Client modifié avec succès !");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Erreur lors de la modification : " + ex.Message);
                }
            }
        }

        //  Supprimer un client
        public void SupprimerClient(int id)
        {
            string query = "DELETE FROM Clients WHERE ID = @ID";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                try
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Client supprimé avec succès !");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Erreur lors de la suppression : " + ex.Message);
                }
            }
        }

        //  Afficher tous les clients
        public void AfficherClients(string critere = "ID")
        {
            string query = $"SELECT * FROM Clients ORDER BY {critere}";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
               // Console.WriteLine("Liste des clients :");
                while (reader.Read())
                {
                    Console.WriteLine($" {reader["ID"]}  {reader["Nom"]} {reader["Prenom"]} ({reader["Email"]})");
                }
            }
        }




        #endregion

        #region compte

        public void AjouterCompte(string mdp, bool estUtilisateur)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Compte (Mdp, est_utilisateur) VALUES (@mdp, @estUtilisateur)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@mdp", mdp);
                    cmd.Parameters.AddWithValue("@estUtilisateur", estUtilisateur);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ModifierCompte(int id, string mdp, bool? estUtilisateur)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Compte SET Mdp = @mdp, est_utilisateur = @estUtilisateur WHERE ID = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@mdp", mdp);
                    cmd.Parameters.AddWithValue("@estUtilisateur", estUtilisateur);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SupprimerCompte(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Compte WHERE ID = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void BannirCompte(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Compte SET Ban = TRUE WHERE ID = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> AfficherComptes(string tri = "ID")
        {
            List<string> comptes = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = $"SELECT * FROM Compte ORDER BY {tri}";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comptes.Add($"ID: {reader["ID"]}, Mdp: {reader["Mdp"]}, Utilisateur: {reader["est_utilisateur"]}, Banni: {reader["Ban"]}");
                    }
                }
            }
            return comptes;
        }

        #endregion

        #region entreprise

        public void AjouterEntreprise(string nomEntreprise, string nomReferent, int ID, string Metro_le_plus_proche)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Entreprise (nom_entreprise, nom_referent, ID, Metro_le_plus_proche) VALUES (@nom, @referent, @ID, @Metro_le_plus_proche)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nom", nomEntreprise);
                    cmd.Parameters.AddWithValue("@referent", nomReferent);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@Metro_le_plus_proche", Metro_le_plus_proche);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Modifier une entreprise
        public void ModifierEntreprise(string nomEntreprise, string nouveauNomReferent, string nouveauMetro)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Entreprise SET nom_referent = @referent, Metro_le_plus_proche = @metro WHERE nom_entreprise = @nom";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nom", nomEntreprise);
                    cmd.Parameters.AddWithValue("@referent", nouveauNomReferent);
                    cmd.Parameters.AddWithValue("@metro", nouveauMetro);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Supprimer une entreprise
        public void SupprimerEntreprise(string nomEntreprise)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Entreprise WHERE nom_entreprise = @nom";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nom", nomEntreprise);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Afficher toutes les entreprises triées par un critère
        public void AfficherEntreprises(string critere)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Entreprise ORDER BY " + critere;
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Entreprise: {reader["nom_entreprise"]}, Référent: {reader["nom_referent"]}, Métro: {reader["Metro_le_plus_proche"]}");
                    }
                }
            }
        }

        #endregion

        #region consomateur

        public void AjouterConsommateur(int idCompte)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Consommateur (ID) VALUES (@ID);";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", idCompte);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ModifierConsommateur(int idConsommateur, float nouvelleNote, int nouvellesNotes)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Consommateur SET Note = @Note, nombre_notes = @NombreNotes WHERE id_consommateur = @ID;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Note", nouvelleNote);
                    cmd.Parameters.AddWithValue("@NombreNotes", nouvellesNotes);
                    cmd.Parameters.AddWithValue("@ID", idConsommateur);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SupprimerConsommateur(int idConsommateur)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Consommateur WHERE id_consommateur = @ID;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", idConsommateur);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AfficherConsommateurs(string critere)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = $"SELECT * FROM Consommateur ORDER BY {critere};";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["id_consommateur"]}, Note: {reader["Note"]}, Nombre de notes: {reader["nombre_notes"]}");
                    }
                }
            }
        }


        #endregion

        #region cuisinier

        public void AjouterCuisinier(string nomCuisinier, int compteID)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Cuisinier (nom_cuisinier, ID) VALUES (@nom, @idCompte);";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nom", nomCuisinier);
                    cmd.Parameters.AddWithValue("@idCompte", compteID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ModifierCuisinier(int idCuisinier, string nouveauNom, float nouvelleNote, int nouveauNombreNotes)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Cuisinier SET nom_cuisinier = @nom, Note = @note, nombre_notes = @nombreNotes WHERE id_cuisinier = @id;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nom", nouveauNom);
                    cmd.Parameters.AddWithValue("@note", nouvelleNote);
                    cmd.Parameters.AddWithValue("@nombreNotes", nouveauNombreNotes);
                    cmd.Parameters.AddWithValue("@id", idCuisinier);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SupprimerCuisinier(int idCuisinier)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Cuisinier WHERE id_cuisinier = @id;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idCuisinier);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> AfficherCuisiniers(string triPar = "nom_cuisinier")
        {
            List<string> cuisiniers = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = $"SELECT * FROM Cuisinier ORDER BY {triPar};";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string info = $"ID: {reader["id_cuisinier"]}, Nom: {reader["nom_cuisinier"]}, Note: {reader["Note"]}, Nombre de notes: {reader["nombre_notes"]}";
                        cuisiniers.Add(info);
                    }
                }
            }
            return cuisiniers;
        }

        #endregion

        #region ingredient

        public void AjouterIngredient(string nomIngredient)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO ingredient (ingredient) VALUES (@ingredient);";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ingredient", nomIngredient);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SupprimerIngredient(string nomIngredient)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM ingredient WHERE ingredient = @ingredient;";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ingredient", nomIngredient);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        #endregion

        #region mets

        public void AjouterMet(string nom, decimal prix, string type, string regime, string origine, int pourCombien, int idCuisinier)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO mets (Nom_plat, prix, type_de_plat, Régime_Alimentaire, origine_plat, pour_combien, id_cuisinier) VALUES (@nom, @prix, @type, @regime, @origine, @pourCombien, @idCuisinier);";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nom", nom);
                    cmd.Parameters.AddWithValue("@prix", prix);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@regime", regime);
                    cmd.Parameters.AddWithValue("@origine", origine);
                    cmd.Parameters.AddWithValue("@pourCombien", pourCombien);
                    cmd.Parameters.AddWithValue("@idCuisinier", idCuisinier);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        #endregion

        #region compose

        public void AssocierIngredientAMet(int idMet, string nomIngredient)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO compose (Id_met, ingredient) VALUES (@idMet, @ingredient);";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idMet", idMet);
                    cmd.Parameters.AddWithValue("@ingredient", nomIngredient);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> AfficherIngredientsParPlat(int idMet)
        {
            List<string> ingredients = new List<string>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ingredient FROM compose WHERE Id_met = @idMet;";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idMet", idMet);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ingredients.Add(reader.GetString("ingredient"));
                        }
                    }
                }
            }
            return ingredients;
        }

        #endregion

        #region relatif commande 

        public void AjouterCommande(int prix, int quantite, DateTime fabrication, DateTime peremption, int idConsommateur, int idCuisinier)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Commandes (Prix, Quantite, Date_Fabrication, Date_Peremption, id_consommateur, id_cuisinier) VALUES (@prix, @quantite, @fabrication, @peremption, @idConsommateur, @idCuisinier)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@prix", prix);
                    cmd.Parameters.AddWithValue("@quantite", quantite);
                    cmd.Parameters.AddWithValue("@fabrication", fabrication);
                    cmd.Parameters.AddWithValue("@peremption", peremption);
                    cmd.Parameters.AddWithValue("@idConsommateur", idConsommateur);
                    cmd.Parameters.AddWithValue("@idCuisinier", idCuisinier);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ModifierCommande(int idCommande, int prix, int quantite)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Commandes SET Prix=@prix, Quantite=@quantite WHERE id_commande=@idCommande";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@prix", prix);
                    cmd.Parameters.AddWithValue("@quantite", quantite);
                    cmd.Parameters.AddWithValue("@idCommande", idCommande);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SupprimerCommande(int idCommande)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Commandes WHERE id_commande=@idCommande";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idCommande", idCommande);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> AfficherCommandes()
        {
            List<string> commandes = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Commandes";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        commandes.Add($"Commande ID: {reader["id_commande"]}, Prix: {reader["Prix"]}, Quantité: {reader["Quantite"]}");
                    }
                }
            }
            return commandes;
        }

        public void AjouterPlatDansCommande(int idCommande, int idMet, int quantite)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO compose_commande (id_commande, Id_met, Quantite) VALUES (@idCommande, @idMet, @quantite)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idCommande", idCommande);
                    cmd.Parameters.AddWithValue("@idMet", idMet);
                    cmd.Parameters.AddWithValue("@quantite", quantite);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void NoterCommande(int idCommande, int idCuisinier, int idConsommateur, float noteClient, string commentaireClient, float noteCuisinier, string commentaireCuisinier)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO rating (id_commande, id_cuisinier, id_consommateur, note_client, commentaire_client, note_cuisinier, commentaire_cuisinier) VALUES (@idCommande, @idCuisinier, @idConsommateur, @noteClient, @commentaireClient, @noteCuisinier, @commentaireCuisinier)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idCommande", idCommande);
                    cmd.Parameters.AddWithValue("@idCuisinier", idCuisinier);
                    cmd.Parameters.AddWithValue("@idConsommateur", idConsommateur);
                    cmd.Parameters.AddWithValue("@noteClient", noteClient);
                    cmd.Parameters.AddWithValue("@commentaireClient", commentaireClient);
                    cmd.Parameters.AddWithValue("@noteCuisinier", noteCuisinier);
                    cmd.Parameters.AddWithValue("@commentaireCuisinier", commentaireCuisinier);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region stat
        public void AfficherLivraisonsParCuisinier()
        {
            string query = @"
        SELECT c.nom_cuisinier, COUNT(co.id_commande) AS nb_livraisons
        FROM cuisinier c
        JOIN Commandes co ON c.id_cuisinier = co.id_cuisinier
        GROUP BY c.id_cuisinier
        ORDER BY nb_livraisons DESC;
    ";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nomCuisinier = reader["nom_cuisinier"].ToString();
                        int nbLivraisons = int.Parse(reader["nb_livraisons"].ToString());
                        Console.WriteLine($"{nomCuisinier} a effectué {nbLivraisons} livraisons.");
                    }
                }
            }
        }

        public void AfficherCommandesParPeriode(DateTime dateDebut, DateTime dateFin)
        {
            string query = @"
        SELECT * 
        FROM Commandes
        WHERE Date_Fabrication BETWEEN @DateDebut AND @DateFin
        ORDER BY Date_Fabrication DESC;
    ";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DateDebut", dateDebut);
                    cmd.Parameters.AddWithValue("@DateFin", dateFin);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Commande ID: {reader["id_commande"]}, Date: {reader["Date_Fabrication"]}, Prix: {reader["Prix"]}");
                        }
                    }
                }
            }
        }

        public void AfficherMoyennePrixCommandes()
        {
            string query = "SELECT AVG(Prix) AS moyenne_prix FROM Commandes";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        double moyennePrix = Convert.ToDouble(reader["moyenne_prix"]);
                        Console.WriteLine($"La moyenne des prix des commandes est : {moyennePrix}€");
                    }
                }
            }
        }

        public void AfficherMoyenneAchatsClients()
        {
            string query = @"
        SELECT AVG(total_achats) AS moyenne_achats
        FROM (
            SELECT SUM(Prix) AS total_achats
            FROM Commandes
            GROUP BY id_consommateur
        ) AS achats_clients;
    ";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        double moyenneAchats = Convert.ToDouble(reader["moyenne_achats"]);
                        Console.WriteLine($"La moyenne des achats des clients est : {moyenneAchats}€");
                    }
                }
            }
        }

        public void AfficherCommandesConsomateurParOrigineEtPeriode(int id_consommateur, string originePlat, DateTime dateDebut, DateTime dateFin)
        {
            string query = @"
                            SELECT conso.id_consommateur, com.id_commande, com.Date_Fabrication, m.origine_plat, m.Nom_plat
                            FROM Commandes com
                            JOIN Consommateur conso ON com.id_consommateur = conso.id_consommateur
                  
                            JOIN compose_commande cc ON com.id_commande = cc.id_commande
                            JOIN mets m ON cc.Id_met = m.Id_met
                            WHERE conso.id_consommateur = @id_consommateur
                            AND m.origine_plat = @originePlat
                            AND com.Date_Fabrication BETWEEN @DateDebut AND @DateFin
                            ORDER BY com.Date_Fabrication DESC;
                             ";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id_consommateur", id_consommateur);
                    cmd.Parameters.AddWithValue("@originePlat", originePlat);
                    cmd.Parameters.AddWithValue("@DateDebut", dateDebut);
                    cmd.Parameters.AddWithValue("@DateFin", dateFin);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Commande ID: {reader["id_commande"]}, Plat: {reader["Nom_plat"]}, Origine: {reader["origine_plat"]}, Date: {reader["Date_Fabrication"]}");
                        }
                    }
                }
            }
        }



        #endregion

        #region idée a trouver_
        #endregion
    }
}








