using System;
using System.Windows.Forms;
using System.Drawing;

namespace Liv_In_Paris
{
    public class FormMain : Form
    {
        /// <summary>
        /// champs utilisés pour la création de compte
        /// </summary>
        private TextBox txtMdp, txtNom, txtPrenom, txtEmail, txtTel, txtStation, txtNomEntreprise, txtNomReferent;
        private RadioButton rbUtilisateur, rbEntreprise;
        private Button btnValiderCompte;
        private Label lblMessage;

        /// <summary>
        /// buttons principales
        /// </summary>
        private Button btnSeConnecter;
        private Button btnCreerCompte;
        private Button btnAdmin;
        /// <summary>
        ///  identifiant de l'utilisateur connecté actuellement
        /// </summary>
        private int utilisateurID;
        
        /// <summary>
        /// sert a gerer la mise en page de l'affichage des plats
        /// </summary>
        private int currentPage = 0; 
        private const int itemsPerPage = 10; /// Nombre d'éléments par page

        private SQL sql; /// Garde une instance partagée

        /// constructeur principal de l'interface
        public FormMain()
        {
            sql = new SQL();

            Text = "Menu principal";
            Width = 800;
            Height = 800;


            /// Création et configuration des boutons principales
            btnSeConnecter = new Button() { Text = "Se connecter", Top = 30, Left = 50, Width = 180 };
            btnCreerCompte = new Button() { Text = "Créer un compte", Top = 80, Left = 50, Width = 180 };
            btnAdmin = new Button() { Text = "Accès admin", Top = 130, Left = 50, Width = 180 };

            /// Ajout des champs pour création de compte (invisible par défaut)
            rbUtilisateur = new RadioButton() { Text = "Utilisateur", Top = 180, Left = 50, Visible = false };
            rbEntreprise = new RadioButton() { Text = "Entreprise", Top = 200, Left = 50, Visible = false };

            txtMdp = new TextBox() { PlaceholderText = "Mot de passe", Top = 160, Left = 50, Width = 180, Visible = false };
            txtNom = new TextBox() { PlaceholderText = "Nom", Top = 230, Left = 50, Width = 180, Visible = false };
            txtPrenom = new TextBox() { PlaceholderText = "Prénom", Top = 260, Left = 50, Width = 180, Visible = false };
            txtEmail = new TextBox() { PlaceholderText = "Email", Top = 290, Left = 50, Width = 180, Visible = false };
            txtTel = new TextBox() { PlaceholderText = "Téléphone", Top = 320, Left = 50, Width = 180, Visible = false };
            txtStation = new TextBox() { PlaceholderText = "Station proche", Top = 350, Left = 50, Width = 180, Visible = false };

            txtNomEntreprise = new TextBox() { PlaceholderText = "Nom entreprise", Top = 230, Left = 50, Width = 180, Visible = false };
            txtNomReferent = new TextBox() { PlaceholderText = "Nom référent", Top = 260, Left = 50, Width = 180, Visible = false };

            btnValiderCompte = new Button() { Text = "Valider", Top = 400, Left = 90, Width = 120, Visible = false };
            btnValiderCompte.Click += ValiderCreationCompte;

            lblMessage = new Label() { Text = "", Top = 430, Left = 50, Width = 250, Visible = false };

            Controls.AddRange(new Control[] {
                txtMdp, rbUtilisateur, rbEntreprise, txtNom, txtPrenom, txtEmail, txtTel, txtStation,
                txtNomEntreprise, txtNomReferent, btnValiderCompte, lblMessage
            });
            /// gestion du bouton se connecter
            btnSeConnecter.Click += (sender, e) =>
            {
                var loginForm = new FormMain.ConnexionForm(sql);
                loginForm.Owner = this; // Permet d'appeler AfficherMenuUtilisateur depuis FormMain
                loginForm.ShowDialog();
            };

            ///affichage du formulaire de creation de compte
            btnCreerCompte.Click += (sender, e) =>
            {
                lblMessage.Text = "";
                lblMessage.Visible = false;

                txtMdp.Visible = true;
                rbUtilisateur.Visible = true;
                rbEntreprise.Visible = true;
                btnValiderCompte.Visible = true;

                rbUtilisateur.CheckedChanged += (s, ev) => ToggleForm(true);
                rbEntreprise.CheckedChanged += (s, ev) => ToggleForm(false);
                rbUtilisateur.Checked = true; // Par défaut
            };

            ///bouton admin (non implémenté encore)
            btnAdmin.Click += (sender, e) =>
            {
                MessageBox.Show("Admin (à implémenter)");
            };

            Controls.Add(btnSeConnecter);
            Controls.Add(btnCreerCompte);
            Controls.Add(btnAdmin);
        }

        /// <summary>
        /// C'est un moyen d'affçchage utilisé ici pour le compte classique et d'entreprise avec des
        /// cases à cocher pour choisir le type de compte
        /// </summary>
        /// <param name="utilisateur"></param>
        private void ToggleForm(bool utilisateur)
        {
            /// Champs utilisateur
            txtNom.Visible = utilisateur;
            txtPrenom.Visible = utilisateur;
            txtEmail.Visible = utilisateur;
            txtTel.Visible = utilisateur;
            txtStation.Visible = true;

            /// Champs entreprise
            txtNomEntreprise.Visible = !utilisateur;
            txtNomReferent.Visible = !utilisateur;
        }

        /// <summary>
        /// 
        /// methode appelée lors de la validation du formulaire de création de compte
        /// on enregistre le mot de passe et on insere soit le client soit l'entreprise
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValiderCreationCompte(object? sender, EventArgs e)
        {
            string mdp = txtMdp.Text;
            bool estUtilisateur = rbUtilisateur.Checked;

            // Utilisation directe du mot de passe, sans hachage
            sql.AjouterCompte(mdp, estUtilisateur);
            int id = sql.DernierID();

            if (estUtilisateur)
            {
                string nom = txtNom.Text;
                string prenom = txtPrenom.Text;
                string email = txtEmail.Text;
                string tel = txtTel.Text;
                string station = txtStation.Text.ToLower();

                sql.AjouterClient(id, nom, prenom, email, tel, station);
            }
            else
            {
                string nomEntreprise = txtNomEntreprise.Text;
                string nomReferent = txtNomReferent.Text;
                string station = txtStation.Text.ToLower();

                sql.AjouterEntreprise(nomEntreprise, nomReferent, id, station);
            }

            lblMessage.Text = $" Compte créé avec succès !\n ID attribué : {id}";
            lblMessage.Visible = true;
        }

        public void AfficherMenuUtilisateur(int id)
        {
            utilisateurID = id;
            Controls.Clear(); /// Efface tous les anciens contrôles (login, création...)

            bool estCuisinier = sql.rolecuisinier(id);
            bool estConsommateur = sql.roleconsommateur(id);

            Label titre = new Label()
            {
                Text = $"Bienvenue, utilisateur #{id}",
                Top = 20,
                Left = 50,
                Width = 300,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            Controls.Add(titre);
            /// Ici on traite les 4 types de menu possible (alternant consomateur et cuisinier)

            if (!estCuisinier && !estConsommateur)
            {
                // Menu00
                Button btnCuisinier = new Button() { Text = "Devenir cuisinier", Top = 60, Left = 50, Width = 180 };
                Button btnClient = new Button() { Text = "Devenir consommateur", Top = 100, Left = 50, Width = 180 };

                btnCuisinier.Click += (s, e) =>
                {
                    sql.AjouterCuisinier("cuisinier_" + id, id);
                    AfficherMenuUtilisateur(id);
                };

                btnClient.Click += (s, e) =>
                {
                    Console.WriteLine("id: " + id);
                    Console.WriteLine("rol: " + sql.roleconsommateur(id));

                    sql.AjouterConsommateur(id);

                    Console.WriteLine("rol: " + sql.roleconsommateur(id));


                    AfficherMenuUtilisateur(id);
                };

                Controls.AddRange(new Control[] { btnCuisinier, btnClient });
            }
            else if (estCuisinier && !estConsommateur)
            {
                // Menu10
                AjouterBouton("Voir mes commandes", 60, () => AfficherCommandes(id));
                AjouterBouton("Voir mes plats", 100, AfficherPlatsCuisinier);
                AjouterBouton("Ajouter plat", 140, AfficherFormAjoutPlat);
                AjouterBouton("Devenir consommateur", 180, () =>
                {
                    sql.AjouterConsommateur(id);
                    AfficherMenuUtilisateur(id);
                });
            }
            else if (!estCuisinier && estConsommateur)
            {
                // Menu01
                AjouterBouton("Plats disponibles", 60, AfficherPlatsDisponibles);
                AjouterBouton("Rechercher plat", 100, RechercherPlat);
                AjouterBouton("Voir mes commandes", 140, () => AfficherCommandes(id));
                AjouterBouton("Ajouter élément à commande", 180, AjouterElementCommande);
                AjouterBouton("Passer une commande", 220, PasserCommande);
                AjouterBouton("Noter une commande", 260, NoterCommande);
                AjouterBouton("Devenir cuisinier", 300, () =>
                {
                    sql.AjouterCuisinier("cuisinier_" + id, id);
                    AfficherMenuUtilisateur(id);
                });
            }
            else
            {
                // Menu11
                AjouterBouton("Voir plats", 60, AfficherPlatsDisponibles);
                AjouterBouton("Rechercher plat", 100, RechercherPlat);
                AjouterBouton("Mes commandes (conso)", 140, () => AfficherCommandes(id));
                AjouterBouton("Ajouter élément", 180, AjouterElementCommande);
                AjouterBouton("Passer commande", 220, PasserCommande);
                AjouterBouton("Noter commande", 260, NoterCommande);
                AjouterBouton("Voir plats (cuisinier)", 300, AfficherPlatsCuisinier);
                AjouterBouton("Ajouter plat (cuisinier)", 340, AfficherFormAjoutPlat);
                AjouterBouton("Afficher statistiques", 380, AfficherStatistiques);
            }
        }

        /// <summary>
        /// mettre les boutons un par un prend beaucoup de espace
        /// donc on a fait une fonction dédié
        /// </summary>
        /// <param name="texte"></param>
        /// <param name="top"></param>
        /// <param name="onClick"></param>
        private void AjouterBouton(string texte, int top, Action? onClick = null)
        {
            Button b = new Button() { Text = texte, Top = top, Left = 50, Width = 250 };
            if (onClick != null)
                b.Click += (s, e) => onClick();
            else
                b.Click += (s, e) => MessageBox.Show($"Action '{texte}' non encore implémentée.");

            Controls.Add(b);
        }

        private void AfficherCommandes(int id)
        {
            sql.AfficherCommandesParCompte(id);
            MessageBox.Show("Commandes affichées dans la console.");
        }


        private void AfficherPlatsDisponibles()
        {
            Controls.Clear();

            Label lblTitre = new Label() { Text = "Plats disponibles", Top = 20, Left = 50, Width = 200, Font = new Font("Arial", 12, FontStyle.Bold) };
            Controls.Add(lblTitre);

            ListBox listBox = new ListBox() { Top = 60, Left = 50, Width = 200, Height = 300 };
            Controls.Add(listBox);

            var plats = sql.AfficherTousLesMets();
            var pagePlats = plats.Skip(currentPage * itemsPerPage).Take(itemsPerPage);

            foreach (var plat in pagePlats)
            {
                listBox.Items.Add(plat);
            }

            Button btnPrecedent = new Button() { Text = "Précédent", Top = 380, Left = 50, Width = 90 };
            btnPrecedent.Click += (s, e) =>
            {
                if (currentPage > 0)
                {
                    currentPage--;
                    AfficherPlatsDisponibles();
                }
            };
            Controls.Add(btnPrecedent);

            Button btnSuivant = new Button() { Text = "Suivant", Top = 380, Left = 160, Width = 90 };
            btnSuivant.Click += (s, e) =>
            {
                if ((currentPage + 1) * itemsPerPage < plats.Count)
                {
                    currentPage++;
                    AfficherPlatsDisponibles();
                }
            };
            Controls.Add(btnSuivant);

            Button btnRetour = new Button() { Text = "Retour", Top = 420, Left = 50, Width = 200 };
            btnRetour.Click += (s, e) => AfficherMenuUtilisateur(utilisateurID);
            Controls.Add(btnRetour);
        }

        private void RechercherPlat()
        {
            string nomPlat = Prompt.ShowDialog("Nom du plat à rechercher :", "Rechercher Plat");
            if (string.IsNullOrWhiteSpace(nomPlat))
            {
                MessageBox.Show("Nom de plat invalide ou action annulée.");
                return;
            }

            sql.ChercherEtAfficherPlat(nomPlat);
            MessageBox.Show("Résultat affiché dans la console.", "Rechercher Plat");
        }

        private void PasserCommande()
        {
            string inputId = Prompt.ShowDialog("Entrez l'ID du cuisinier :", "Passer une commande");

            if (!int.TryParse(inputId, out int idCuisinier))
            {
                MessageBox.Show("ID invalide.");
                return;
            }

            // Vérifie que cet ID appartient bien à un cuisinier
            if (sql.rolecuisinier(idCuisinier))
            {
                MessageBox.Show("Cet ID ne correspond pas à un cuisinier.");
                return;
            }

            DateTime fabrication = DateTime.Now;
            DateTime peremption = fabrication.AddDays(7);
            int idConsommateur = sql.idduconsomateur(utilisateurID);

            sql.AjouterCommande(fabrication, peremption, idConsommateur, idCuisinier);
            MessageBox.Show("Commande ajoutée avec succès !");
        }


        private void AjouterElementCommande()
        {
            string idCommandeStr = Prompt.ShowDialog("Entrez l'ID de la commande :", "Ajouter Élément");
            if (!int.TryParse(idCommandeStr, out int idCommande))
            {
                MessageBox.Show("ID de commande invalide.");
                return;
            }

            string idMetStr = Prompt.ShowDialog("Entrez l'ID du plat :", "Ajouter Élément");
            if (!int.TryParse(idMetStr, out int idMet))
            {
                MessageBox.Show("ID du plat invalide.");
                return;
            }

            string quantiteStr = Prompt.ShowDialog("Entrez la quantité :", "Ajouter Élément");
            if (!int.TryParse(quantiteStr, out int quantite) || quantite <= 0)
            {
                MessageBox.Show("Quantité invalide.");
                return;
            }

            sql.AjouterPlatDansCommande(idCommande, idMet, quantite);
            MessageBox.Show("Élément ajouté à la commande avec succès !");
        }

        private void NoterCommande()
        {
            string idCommandeStr = Prompt.ShowDialog("Entrez l'ID de la commande :", "Noter Commande");
            if (!int.TryParse(idCommandeStr, out int idCommande))
            {
                MessageBox.Show("ID de commande invalide.");
                return;
            }

            string noteClientStr = Prompt.ShowDialog("Entrez la note du client (0 à 5) :", "Noter Commande");
            if (!float.TryParse(noteClientStr, out float noteClient) || noteClient < 0 || noteClient > 5)
            {
                MessageBox.Show("Note invalide.");
                return;
            }

            string commentaireClient = Prompt.ShowDialog("Entrez le commentaire du client :", "Noter Commande");

            string noteCuisinierStr = Prompt.ShowDialog("Entrez la note du cuisinier (0 à 5) :", "Noter Commande");
            if (!float.TryParse(noteCuisinierStr, out float noteCuisinier) || noteCuisinier < 0 || noteCuisinier > 5)
            {
                MessageBox.Show("Note invalide.");
                return;
            }

            string commentaireCuisinier = Prompt.ShowDialog("Entrez le commentaire du cuisinier :", "Noter Commande");

            sql.NoterCommande(idCommande, utilisateurID, utilisateurID, noteClient, commentaireClient, noteCuisinier, commentaireCuisinier);
            MessageBox.Show("Commande notée avec succès !");
        }

        private void AfficherPlatsCuisinier()
        {
            Controls.Clear();

            Label lblTitre = new Label()
            {
                Text = "Mes plats (cuisinier)",
                Top = 20,
                Left = 50,
                Width = 300,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            Controls.Add(lblTitre);

            ListBox listBox = new ListBox()
            {
                Top = 60,
                Left = 50,
                Width = 250,
                Height = 300
            };
            Controls.Add(listBox);

            int idCuisinier = sql.idducuisinier(utilisateurID);
            var plats = sql.AfficherTousLesMetsducuisto(idCuisinier); // Cette méthode doit retourner une List<string>

            foreach (var plat in plats)
            {
                listBox.Items.Add(plat);
            }

            Button btnRetour = new Button()
            {
                Text = "Retour",
                Top = 380,
                Left = 50,
                Width = 200
            };
            btnRetour.Click += (s, e) => AfficherMenuUtilisateur(utilisateurID);
            Controls.Add(btnRetour);
        }


        private void AfficherFormAjoutPlat()
        {
            Controls.Clear();

            Label lblTitre = new Label()
            {
                Text = "Ajouter un nouveau plat",
                Top = 20,
                Left = 50,
                Width = 300,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            Controls.Add(lblTitre);

            // Champs nécessaires
            TextBox txtNom = new TextBox() { PlaceholderText = "Nom du plat", Top = 60, Left = 50, Width = 200 };
            TextBox txtPrix = new TextBox() { PlaceholderText = "Prix", Top = 100, Left = 50, Width = 200 };
            TextBox txtType = new TextBox() { PlaceholderText = "Type (entrée, plat...)", Top = 140, Left = 50, Width = 200 };
            TextBox txtRegime = new TextBox() { PlaceholderText = "Régime (végétarien...)", Top = 180, Left = 50, Width = 200 };
            TextBox txtOrigine = new TextBox() { PlaceholderText = "Origine (italien...)", Top = 220, Left = 50, Width = 200 };
            TextBox txtPourCombien = new TextBox() { PlaceholderText = "Portions", Top = 260, Left = 50, Width = 200 };

            Button btnValider = new Button() { Text = "Ajouter", Top = 310, Left = 100, Width = 120 };
            btnValider.Click += (s, e) =>
            {
                if (!decimal.TryParse(txtPrix.Text, out decimal prix) || prix <= 0)
                {
                    MessageBox.Show("Prix invalide.");
                    return;
                }

                if (!int.TryParse(txtPourCombien.Text, out int portions) || portions <= 0)
                {
                    MessageBox.Show("Nombre de portions invalide.");
                    return;
                }

                int idCuisinier = sql.idducuisinier(utilisateurID);
                sql.AjouterMet(txtNom.Text, prix, txtType.Text, txtRegime.Text, txtOrigine.Text, portions, idCuisinier);

                MessageBox.Show("Plat ajouté !");
                AfficherMenuUtilisateur(utilisateurID);
            };

            Controls.AddRange(new Control[] { txtNom, txtPrix, txtType, txtRegime, txtOrigine, txtPourCombien, btnValider });
        }


        private void AfficherStatistiques()
        {
            Controls.Clear();

            Label lblTitre = new Label() { Text = "Statistiques", Top = 20, Left = 50, Width = 200, Font = new Font("Arial", 12, FontStyle.Bold) };
            Controls.Add(lblTitre);

            ListBox listBox = new ListBox() { Top = 60, Left = 50, Width = 200, Height = 300 };
            Controls.Add(listBox);

            sql.AfficherLivraisonsParCuisinier();
            listBox.Items.Add("Statistiques affichées dans la console.");

            Button btnRetour = new Button() { Text = "Retour", Top = 380, Left = 50, Width = 200 };
            btnRetour.Click += (s, e) => AfficherMenuUtilisateur(utilisateurID);
            Controls.Add(btnRetour);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        private class ConnexionForm : Form
        {
            private Label lblID;
            private TextBox txtID;
            private Label lblMdp;
            private TextBox txtMdp;
            private Button btnConnexion;
            private readonly SQL sqlRef;

            public ConnexionForm(SQL sql)
            {
                sqlRef = sql;
                Text = "Connexion";
                Width = 300;
                Height = 200;

                lblID = new Label() { Text = "ID :", Left = 20, Top = 20, Width = 100 };
                txtID = new TextBox() { Left = 120, Top = 20, Width = 120 };

                lblMdp = new Label() { Text = "Mot de passe :", Left = 20, Top = 60, Width = 100 };
                txtMdp = new TextBox() { Left = 120, Top = 60, Width = 120, UseSystemPasswordChar = true };

                btnConnexion = new Button() { Text = "Se connecter", Left = 80, Top = 110, Width = 120 };
                btnConnexion.Click += BtnConnexion_Click;

                Controls.Add(lblID);
                Controls.Add(txtID);
                Controls.Add(lblMdp);
                Controls.Add(txtMdp);
                Controls.Add(btnConnexion);
            }

            private void BtnConnexion_Click(object? sender, EventArgs e)
            {
                if (!int.TryParse(txtID.Text, out int id))
                {
                    MessageBox.Show("ID invalide");
                    return;
                }

                string mdp = txtMdp.Text;

                if (sqlRef.VerifierCompte(id, mdp))
                {
                    MessageBox.Show("Connexion réussie !");
                    ((FormMain)Owner).AfficherMenuUtilisateur(id);
                    Close();
                }
                else
                {
                    MessageBox.Show("Échec de la connexion.");
                }


            }
        }

    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                Text = caption
            };

            Label lblText = new Label() { Left = 10, Top = 10, Text = text, Width = 260 };
            TextBox txtInput = new TextBox() { Left = 10, Top = 40, Width = 260 };
            Button btnOk = new Button() { Text = "OK", Left = 200, Top = 70, Width = 70 };

            string result = null;

            btnOk.Click += (sender, e) => { result = txtInput.Text; prompt.Close(); };

            prompt.Controls.Add(lblText);
            prompt.Controls.Add(txtInput);
            prompt.Controls.Add(btnOk);

            prompt.ShowDialog();

            return string.IsNullOrWhiteSpace(result) ? null : result;
        }
    }
}
