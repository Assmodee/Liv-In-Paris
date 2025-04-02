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
       
        private static string connectionString = "Server=localhost;Database=liv_in_paris;User ID=root;Password=ton_mot_de_passe;";

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
        public void AjouterClient(string nom, string prenom, string email, string adresse)
        {
            string query = "INSERT INTO Clients (Nom, Prenom, Email, Adresse) VALUES (@Nom, @Prenom, @Email, @Adresse)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Nom", nom);
                cmd.Parameters.AddWithValue("@Prenom", prenom);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Adresse", adresse);

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
        public void ModifierClient(int id, string nom, string prenom, string email, string adresse)
        {
            string query = "UPDATE Clients SET Nom = @Nom, Prenom = @Prenom, Email = @Email, Adresse = @Adresse WHERE ID_Client = @ID";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Nom", nom);
                cmd.Parameters.AddWithValue("@Prenom", prenom);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Adresse", adresse);

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
            string query = "DELETE FROM Clients WHERE ID_Client = @ID";
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
        public void AfficherClients(string critere = "Nom")
        {
            string query = $"SELECT * FROM Clients ORDER BY {critere}";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("Liste des clients :");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Nom"]} {reader["Prenom"]} ({reader["Email"]}) - {reader["Adresse"]}");
                }
            }
        }

       
    }

    #endregion

    #region compte
    #endregion

    #region entreprise
    #endregion

    #region consomateur
    #endregion

    #region cuisinier
    #endregion

    #region ingredient
    #endregion

    #region mets
    #endregion

    #region compose
    #endregion

    #region stat
    #endregion

    #region idée a trouver_
    #endregion
}








