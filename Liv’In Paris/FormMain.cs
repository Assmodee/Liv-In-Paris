
using System;
using System.Windows.Forms;
using System.Drawing;
using Org.BouncyCastle.Asn1.Crmf;

namespace Liv_In_Paris
{
    public class FormMain : Form
    {
        // Champs pour création de compte
        private TextBox txtMdp, txtNom, txtPrenom, txtEmail, txtTel, txtStation, txtNomEntreprise, txtNomReferent;
        private RadioButton rbUtilisateur, rbEntreprise;
        private Button btnValiderCompte;
        private Label lblMessage;

        private Button btnSeConnecter;
        private Button btnCreerCompte;
        private Button btnAdmin;

        private int utilisateurID;

        private SQL sql; // Garde une instance partagée

        public FormMain()
        {
            sql = new SQL();

            Text = "Menu principal";
            Width = 300;
            Height = 800;

            btnSeConnecter = new Button() { Text = "Se connecter", Top = 30, Left = 50, Width = 180 };
            btnCreerCompte = new Button() { Text = "Créer un compte", Top = 80, Left = 50, Width = 180 };
            btnAdmin = new Button() { Text = "Accès admin", Top = 130, Left = 50, Width = 180 };

            // Ajout des champs pour création de compte (invisible par défaut)
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



            btnSeConnecter.Click += (sender, e) =>
            {
                var loginForm = new ConnexionForm(sql);
                loginForm.Owner = this; // Permet d'appeler AfficherMenuUtilisateur depuis FormMain
                loginForm.ShowDialog();
            };

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


            btnAdmin.Click += (sender, e) =>
            {
                MessageBox.Show("Admin (à implémenter)");
            };

            Controls.Add(btnSeConnecter);
            Controls.Add(btnCreerCompte);
            Controls.Add(btnAdmin);
        }

        // ➕ Classe interne pour le formulaire de connexion
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

        private void ToggleForm(bool utilisateur)
        {
            // Champs utilisateur
            txtNom.Visible = utilisateur;
            txtPrenom.Visible = utilisateur;
            txtEmail.Visible = utilisateur;
            txtTel.Visible = utilisateur;
            txtStation.Visible = true;

            // Champs entreprise
            txtNomEntreprise.Visible = !utilisateur;
            txtNomReferent.Visible = !utilisateur;
        }


        private void ValiderCreationCompte(object? sender, EventArgs e)
        {
            string mdp = txtMdp.Text;
            bool estUtilisateur = rbUtilisateur.Checked;

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

            lblMessage.Text = $"✅ Compte créé avec succès !\n🆔 ID attribué : {id}";
            lblMessage.Visible = true;
        }


        public void AfficherMenuUtilisateur(int id)
        {
            utilisateurID = id;
            Controls.Clear(); // Efface tous les anciens contrôles (login, création...)

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
                    sql.AjouterConsommateur(id);
                    AfficherMenuUtilisateur(id);
                };

                Controls.AddRange(new Control[] { btnCuisinier, btnClient });
            }
            else if (estCuisinier && !estConsommateur)
            {
                // Menu10
                AjouterBouton("Voir mes commandes", 60);
                AjouterBouton("Voir mes plats", 100);
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
                AjouterBouton("Plats disponibles", 60);
                AjouterBouton("Rechercher plat", 100);
                AjouterBouton("Voir mes commandes", 140);
                AjouterBouton("Ajouter élément à commande", 180);
                AjouterBouton("Passer une commande", 220);
                AjouterBouton("Noter une commande", 260);
                AjouterBouton("Devenir cuisinier", 300, () =>
                {
                    sql.AjouterCuisinier("cuisinier_" + id, id);
                    AfficherMenuUtilisateur(id);
                });
            }
            else
            {
                // Menu11
                AjouterBouton("Voir plats", 60);
                AjouterBouton("Rechercher plat", 100);
                AjouterBouton("Mes commandes (conso)", 140);
                AjouterBouton("Ajouter élément", 180);
                AjouterBouton("Passer commande", 220);
                AjouterBouton("Noter commande", 260);
                AjouterBouton("Voir plats (cuisinier)", 300);
                AjouterBouton("Ajouter plat (cuisinier)", 340, AfficherFormAjoutPlat);
            }
        }

        private void AjouterBouton(string texte, int top, Action? onClick = null)
        {
            Button b = new Button() { Text = texte, Top = top, Left = 50, Width = 250 };
            if (onClick != null)
                b.Click += (s, e) => onClick();
            else
                b.Click += (s, e) => MessageBox.Show($"Action '{texte}' non encore implémentée.");

            Controls.Add(b);
        }

        private void AfficherFormAjoutPlat()
        {
            Controls.Clear();

            Label lblTitre = new Label() { Text = "➕ Ajouter un plat", Top = 20, Left = 50, Width = 200, Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold) };
            Controls.Add(lblTitre);

            int top = 60;
            TextBox txtNom = Champ("Nom du plat", ref top);
            TextBox txtPrix = Champ("Prix (€)", ref top);
            TextBox txtType = Champ("Type (entrée, plat...)", ref top);
            TextBox txtRegime = Champ("Régime (végé, halal...)", ref top);
            TextBox txtOrigine = Champ("Origine", ref top);
            TextBox txtPour = Champ("Pour combien de personnes", ref top);

            Button btnAjouter = new Button() { Text = "✅ Ajouter", Top = top + 10, Left = 50, Width = 200 };
            Controls.Add(btnAjouter);

            btnAjouter.Click += (s, e) =>
            {
                // Vérification des champs
                if (!decimal.TryParse(txtPrix.Text, out decimal prix) || prix <= 0)
                {
                    MessageBox.Show("Prix invalide");
                    return;
                }

                if (!int.TryParse(txtPour.Text, out int pourCombien) || pourCombien <= 0)
                {
                    MessageBox.Show("Nombre de personnes invalide");
                    return;
                }

                int idCuisinier = sql.idducuisinier(utilisateurID);

                sql.AjouterMet(txtNom.Text, prix, txtType.Text, txtRegime.Text, txtOrigine.Text, pourCombien, idCuisinier);

                MessageBox.Show("🍽 Plat ajouté avec succès !");
                AfficherMenuUtilisateur(utilisateurID); // Retour au menu
            };
        }
        private TextBox Champ(string label, ref int top)
        {
            Label lbl = new Label() { Text = label + " :", Top = top, Left = 50, Width = 200 };
            TextBox txt = new TextBox() { Top = top + 20, Left = 50, Width = 250 };

            Controls.Add(lbl);
            Controls.Add(txt);

            top += 60;
            return txt;
        }



    }
}
