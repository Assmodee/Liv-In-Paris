using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    internal class SQL
    {

        private static string connectionString = "Server=localhost;Database=liv_in_paris;User ID=superbozo;Password=\"On peut tromper une personne mille fois. On peut tromper mille personne une fois. Mais on ne peut pas tromper mille personnes, mille fois\";";

        private MySqlConnection conn;

        /// Constructeur : ouvre la connexion automatiquement
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

        /// Ferme la connexion 
        public void Close()
        {
            if (conn != null)
            {
                conn.Close();
                Console.WriteLine("Connexion fermée.");
            }
        }

        public string pourAlex()
        {
            string result = "";

            
            string query = @"
            SELECT 
                conso.ID AS ConsumerAccount, 
                cuis.ID AS ChefAccount
            FROM Commandes cmd
            JOIN Consommateur conso ON cmd.id_consommateur = conso.id_consommateur
            JOIN Cuisinier cuis ON cmd.id_cuisinier = cuis.id_cuisinier;
        ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // magie noir 
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int consumer = reader.GetInt32("ConsumerAccount");
                        int chef = reader.GetInt32("ChefAccount");

                        
                        result += consumer + "|" + chef + "|" + "true" + "\n";
                    }
                }
            }

            

            return result;
        }

        #region client

        //  Ajouter un client
        public void AjouterClient(int ID, string nom, string prenom, string email, string Tel, string Metro_le_plus_proche)
        {
            string query = "INSERT INTO Clients (ID,Nom, Prenom, Email,Tel,Metro_le_plus_proche) VALUES (@ID,@Nom, @Prenom, @Email, @Tel,@Metro_le_plus_proche)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@Nom", nom);
                cmd.Parameters.AddWithValue("@Prenom", prenom);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Tel", Tel);
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
        public void ModifierClient(int id, string nom, string prenom, string email, string Tel, string Metro_le_plus_proche)
        {
            string query = "UPDATE Clients SET Nom = @Nom, Prenom = @Prenom, Email = @Email, Tel = @Tel , Metro_le_plus_proche = @Metro_le_plus_proche WHERE ID = @ID";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Nom", nom);
                cmd.Parameters.AddWithValue("@Prenom", prenom);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Tel", Tel);
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

        public void AjouterCompte(string Mdp, bool est_utilisateur)
        {
            try
            {
                 using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Compte (Mdp, est_utilisateur) VALUES (@Mdp, @est_utilisateur)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Mdp", Mdp);
                        cmd.Parameters.AddWithValue("@est_utilisateur", est_utilisateur);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur MySQL : " + ex.Message);
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

        public void SupprimerCompte(int id) /// on nutiliseras pas cette fonction on prefere bannir les comptes pour garder les infos et pouvoir detecter les gens qui font 2 comptes 
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

        public static void ValiderCommande(int idCommande)
        {
           

            string query = "UPDATE Commandes SET validé = true WHERE id_commande = @id_commande";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_commande", idCommande);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Commande validée avec succès.");
                        }
                        else
                        {
                            Console.WriteLine("Aucune commande trouvée avec cet ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
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


        public List<string> AfficherTousLesMets()
        {
            List<string> plats = new List<string>();
            string query = "SELECT DISTINCT Id_met, Nom_plat, Prix FROM mets;";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("Id_met");
                        string nom = reader.GetString("Nom_plat");
                        decimal prix = reader.GetDecimal("Prix");

                        plats.Add($"ID: {id}, Nom: {nom}, Prix: {prix}€");
                    }
                }
            }

            return plats;
        }



        public List<string> AfficherTousLesMetsducuisto(int idduCuisinier)
{
    var plats = new List<string>();
    string query = "SELECT DISTINCT Id_met, Nom_plat, Prix FROM mets WHERE id_cuisinier = @idduCuisinier;";

    using (var conn = new MySqlConnection(connectionString))
    {
        conn.Open();
        using (var cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@idduCuisinier", idduCuisinier);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32("Id_met");
                    string nom = reader.GetString("Nom_plat");
                    decimal prix = reader.GetDecimal("Prix");

                    plats.Add($"ID: {id}, Nom: {nom}, Prix: {prix}€");
                }
            }
        }
    }

    return plats;
}



        public void ChercherEtAfficherPlat(string nomPlat)
        {
            string query = "SELECT Id_met, Nom_plat, Prix FROM mets WHERE Nom_plat = @nomPlat;";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nomPlat", nomPlat);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) 
                        {
                            int id = reader.GetInt32("Id_met");
                            string nom = reader.GetString("Nom_plat");
                            decimal prix = reader.GetDecimal("Prix");

                            Console.WriteLine($"Le plat existe !");
                            Console.WriteLine($"ID: {id}, Nom: {nom}, Prix: {prix}€");
                        }
                        else
                        {
                            Console.WriteLine($"Le plat '{nomPlat}' n'existe pas.");
                        }
                    }
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

        public void AjouterCommande(DateTime fabrication, DateTime peremption, int id_consommateur, int id_cuisinier)
        {

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Commandes ( Date_Fabrication, Date_Peremption, id_consommateur, id_cuisinier) VALUES ( @fabrication, @peremption, @id_consommateur, @id_cuisinier)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@fabrication", fabrication);
                    cmd.Parameters.AddWithValue("@peremption", peremption);
                    cmd.Parameters.AddWithValue("@id_consommateur", id_consommateur);
                    cmd.Parameters.AddWithValue("@id_cuisinier", id_cuisinier);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //public void ModifierCommande(int idCommande, int prix, int quantite)
        //{
        //    using (MySqlConnection conn = new MySqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        string query = "UPDATE Commandes SET Prix=@prix, Quantite=@quantite WHERE id_commande=@idCommande";
        //        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@prix", prix);
        //            cmd.Parameters.AddWithValue("@quantite", quantite);
        //            cmd.Parameters.AddWithValue("@idCommande", idCommande);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}

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

        public static void AfficherCommandesValides()
        {
            
            string query = @"
                                SELECT * FROM Commandes
                                WHERE CURDATE() BETWEEN Date_Fabrication AND Date_Peremption;
                            ";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id_commande");
                            DateTime fabrication = reader.GetDateTime("Date_Fabrication");
                            DateTime peremption = reader.GetDateTime("Date_Peremption");
                            int idConsommateur = reader.GetInt32("id_consommateur");
                            int idCuisinier = reader.GetInt32("id_cuisinier");
                            

                            Console.WriteLine($"Commande {id} | Fabriquée le {fabrication:yyyy-MM-dd} | Périme le {peremption:yyyy-MM-dd} | Consommateur: {idConsommateur} | Cuisinier: {idCuisinier} ");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
            }
        }

        public void AjouterPlatDansCommande(int id_commande, int Id_met, int Quantite)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

               
                string checkQuery = "SELECT Quantite FROM compose_commande WHERE id_commande = @id_commande AND Id_met = @Id_met";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@id_commande", id_commande);
                    checkCmd.Parameters.AddWithValue("@Id_met", Id_met);

                    object result = checkCmd.ExecuteScalar();

                    if (result != null)
                    {
                       
                        int quantiteExistante = Convert.ToInt32(result);
                        string updateQuery = "UPDATE compose_commande SET Quantite = @newQuantite WHERE id_commande = @id_commande AND Id_met = @Id_met";
                        using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@newQuantite", quantiteExistante + Quantite);
                            updateCmd.Parameters.AddWithValue("@id_commande", id_commande);
                            updateCmd.Parameters.AddWithValue("@Id_met", Id_met);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                       
                        string insertQuery = "INSERT INTO compose_commande (id_commande, Id_met, Quantite) VALUES (@id_commande, @Id_met, @Quantite)";
                        using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@id_commande", id_commande);
                            insertCmd.Parameters.AddWithValue("@Id_met", Id_met);
                            insertCmd.Parameters.AddWithValue("@Quantite", Quantite);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
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
                        decimal prix;
                        while (reader.Read())
                        {
                            prix = GetPrixCommande(reader.GetInt32(0));

                            Console.WriteLine($"Commande ID: {reader["id_commande"]}, Date: {reader["Date_Fabrication"]}, Prix: {prix}");
                        }
                    }
                }
            }
        }

        public void AfficherMoyennePrixCommandes()
        {
            string query = @"
        SELECT AVG(total) 
        FROM (
            SELECT SUM(m.Prix * cc.Quantite) AS total
            FROM compose_commande cc
            JOIN mets m ON cc.Id_met = m.Id_met
            GROUP BY cc.id_commande
        ) AS sous_requete;";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    var result = cmd.ExecuteScalar();
                    double moyennePrix = (result != null) ? Convert.ToDouble(result) : 0;
                    Console.WriteLine($"Moyenne des prix des commandes : {moyennePrix}€");
                }
            }
        }

       

        public void AfficherCommandesConsomateurParOrigineEtPeriode(int id_consommateur, string originePlat, DateTime dateDebut, DateTime dateFin) // jsps si ca marche bien 
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
                            Console.WriteLine($"Plat: {reader["Nom_plat"]}, Origine: {reader["origine_plat"]}, Date: {reader["Date_Fabrication"]}");
                        }
                    }
                }
            }
        }



        #endregion



        #region fonction autre

        public decimal GetPrixCommande(int commandeId)
        {
            decimal prixTotal = 0m;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                                    SELECT SUM(m.Prix * cc.Quantite) AS prix_total
                                    FROM compose_commande cc
                                    JOIN mets m ON cc.Id_met = m.Id_met
                                    WHERE cc.id_commande = @commandeId;";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@commandeId", commandeId);

                    var result = cmd.ExecuteScalar();
                    prixTotal = (result == DBNull.Value || result == null) ? 0m : Convert.ToDecimal(result);


                }
            }

            return prixTotal;
        }

        public int DernierID()
        {
            int nb = 1;
            

            string query = @"SELECT ID FROM Compte";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) > nb)
                    {
                        nb = reader.GetInt32(0);
                    }
                }
                return nb;
            }
        }

        public int DernierIDcommande()
        {
            int dernierID = 0;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                

                string query = "SELECT MAX(id_commande) FROM Commandes";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                        dernierID = Convert.ToInt32(result);
                }
            }

            return dernierID;
        }




        public int Dernieridcuisto()
        {
            int nb = 0;
            

            string query = @"SELECT id_cuisinier FROM cuisinier";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) > nb)
                    {
                        nb = reader.GetInt32(0);
                    }
                }
                return nb;
            }
        }

        public int Dernieridconsomateur() 
        {
            int nb = 0;
            

            string query = @"SELECT id_consommateur FROM Consommateur";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) > nb)
                    {
                        nb = reader.GetInt32(0);
                    }
                }
                return nb;
            }
        }

        public int DernierId_commande()
        {
            int nb = 0;
           

            string query = @"SELECT id_commande FROM Commandes";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) > nb)
                    {
                        nb = reader.GetInt32(0);
                    }
                }
                return nb;
            }
        }

        public bool rolecuisinier(int ID)
        {
            bool result = false;
            string query = @"Select ID from cuisinier";
            int dernierID = DernierID();
            int dernieridcuisto = Dernieridcuisto();

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                
                while ((reader.Read()) )
                {
                    if ( reader.GetInt32(0) == ID)
                    {
                        result = true;
                    }
                    
                }
                return result;
            }
        }

        public int idducuisinier(int id)
        {
            int result = 0;
            string query = @"SELECT id_cuisinier FROM cuisinier WHERE ID = @id";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // Important !
                    {
                        result = reader.GetInt32(0);
                    }
                }
            }

            return result;
        }


        public bool roleconsommateur(int ID)
        {
            bool result = false;
            string query = @"Select ID from Consommateur";
            int dernierID = DernierID();
            int dernieridcuisto = Dernieridconsomateur();

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                
                while (reader.Read())
                {
                    if (reader.GetInt32(0) == ID)
                    {
                        result = true;
                    }
                    
                }
                return result;
            }
        }


        public bool VerifierCompte(int id, string motDePasse)
        {
            string query = "SELECT COUNT(*) FROM Compte WHERE ID = @id AND Mdp = @motDePasse;";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@motDePasse", motDePasse);

                    
                    int compteExiste = Convert.ToInt32(cmd.ExecuteScalar());
                    return compteExiste > 0;
                }
            }
        }



        public void AfficherCommandesParCompte(int ID)
        {
            string query = @"
SELECT c.id_commande, c.Date_Fabrication, c.Date_Peremption, cu.nom_cuisinier AS NomCuisinier
FROM Commandes c
JOIN cuisinier cu ON c.id_cuisinier = cu.id_cuisinier
JOIN consommateur conso ON c.id_consommateur = conso.id_consommateur
WHERE conso.ID = @ID;";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine($"Aucune commande trouvée pour le compte ID {ID}.");
                            return;
                        }

                        Console.WriteLine($"Commandes du compte ID {ID} :");
                        while (reader.Read())
                        {
                            int idCommande = reader.GetInt32("id_commande");
                            DateTime dateFab = reader.GetDateTime("Date_Fabrication");
                            DateTime datePer = reader.GetDateTime("Date_Peremption");
                            string nomCuisinier = reader.GetString("NomCuisinier");

                            Console.WriteLine($"Commande ID: {idCommande}, Fabrication: {dateFab:yyyy-MM-dd}, Péremption: {datePer:yyyy-MM-dd}, Cuisinier: {nomCuisinier}");
                        }
                    }
                }
            }
        }



        public void AfficherCommandesParCuisinier(int id)
        {
            string query = @"
        SELECT c.id_commande, c.Date_Fabrication, c.Date_Peremption AS NomConsommateur
        FROM Commandes c
        
        JOIN cuisinier cuis ON c.id_cuisinier = cuis.id_cuisinier
        WHERE cuis.ID = @id;";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine($" Aucune commande trouvée pour le cuisinier avec le compte ID {id}.");
                            return;
                        }

                        Console.WriteLine($" Commandes préparées par le cuisinier (Compte ID {id}) :");
                        while (reader.Read())
                        {
                            int idCommande = reader.GetInt32("id_commande");
                            DateTime dateFab = reader.GetDateTime("Date_Fabrication");
                            DateTime datePer = reader.GetDateTime("Date_Peremption");
                            string nomConsommateur = reader.GetString("NomConsommateur");

                            Console.WriteLine($" Commande ID: {idCommande},  Fabrication: {dateFab:yyyy-MM-dd},  Péremption: {datePer:yyyy-MM-dd},  Client: {nomConsommateur}");
                        }
                    }
                }
            }
        }


        public decimal obtenirnoteconsomateur(int idconso)
        {
            decimal result = 0m;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string querry = @"select avg(note_client) 
                                  from RATING 
                                  where id_consommateur = @idconso";

                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(querry, connection))
                    {
                        command.Parameters.AddWithValue("@idconso", idconso);

                        var moyenne = command.ExecuteScalar();
                        if (moyenne != null)
                        {
                            result = Convert.ToDecimal(moyenne);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Une erreur s'est produite : " + ex.Message);
                }
            }


            return result;
        }

        public decimal obtenirnotecuisinier(int idcuisto)
        {
            decimal result = 0m;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string querry = @"select avg(note_cuisinier) 
                                  from RATING 
                                  where id_cuisinier = @idcuisto";

                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(querry, connection))
                    {
                        command.Parameters.AddWithValue("@idcuisto", idcuisto);

                        var moyenne = command.ExecuteScalar();
                        if (moyenne != null)
                        {
                            result = Convert.ToDecimal(moyenne);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Une erreur s'est produite : " + ex.Message);
                }
            }


            return result;
        }


        #endregion


        #region fancy

        public static void ExecuterRequeteLibre(string requete)
        {

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(requete, connection))
                    {
                        int lignesAffectees = command.ExecuteNonQuery();
                        Console.WriteLine($"Requête exécutée avec succès. Lignes affectées : {lignesAffectees}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Une erreur s'est produite : " + ex.Message);
                }
            }
        }

        #endregion

    }
}








