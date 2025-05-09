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

        
        private Panel panelMenuPrincipal;


        private SQL sql; /// Garde une instance partagée

        /// constructeur principal de l'interface
        public FormMain()
        {
            sql = new SQL();

            /// Configuration de la fenêtre principale
            Text = "Menu principal";
            Width = 800;
            Height = 800;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(255, 165, 196, 201);

            /// Initialisation des champs de formulaire (hors affichage)
            txtMdp = new TextBox();
            txtNom = new TextBox();
            txtPrenom = new TextBox();
            txtEmail = new TextBox();
            txtTel = new TextBox();
            txtStation = new TextBox();
            txtNomEntreprise = new TextBox();
            txtNomReferent = new TextBox();
            rbUtilisateur = new RadioButton();
            rbEntreprise = new RadioButton();
            btnValiderCompte = new Button();
            lblMessage = new Label();

            /// Initialisation des boutons principaux (hors affichage)
            btnSeConnecter = new Button();
            btnCreerCompte = new Button();
            btnAdmin = new Button();

            // Appel direct au menu principal complet
            AfficherMenuPrincipal();
        }

        private void AfficherMenuPrincipal()
        {
            Controls.Clear();

            /// Logo Cookhub tm
            PictureBox logo = new PictureBox();
            logo.Image = Image.FromFile(@"logo.jpeg");
            logo.Size = new Size(200, 200);
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.Left = (ClientSize.Width - logo.Width) / 2;
            logo.Top = 20;
            Controls.Add(logo);

            int topOffset = logo.Bottom + 20;
            int centerX = (ClientSize.Width - 180) / 2;

            /// Boutons principaux (réinitialisation complète)
            btnSeConnecter.Text = "Se connecter";
            btnSeConnecter.Width = 180;
            btnSeConnecter.Top = topOffset;
            btnSeConnecter.Left = centerX;

            btnCreerCompte.Text = "Créer un compte";
            btnCreerCompte.Width = 180;
            btnCreerCompte.Top = topOffset + 50;
            btnCreerCompte.Left = centerX;

            btnAdmin.Text = "Accès admin";
            btnAdmin.Width = 180;
            btnAdmin.Top = topOffset + 100;
            btnAdmin.Left = centerX;

            btnSeConnecter.Click += (sender, e) =>
            {
                var loginForm = new ConnexionForm(sql);
                loginForm.Owner = this;
                loginForm.ShowDialog();
            };

            btnCreerCompte.Click += (sender, e) =>
            {
                AfficherFormulaireCreationCompte();
            };

            btnAdmin.Click += (sender, e) =>
            {
                AfficherInterfaceAdmin();
            };

            Controls.AddRange(new Control[] { btnSeConnecter, btnCreerCompte, btnAdmin });
        }


        private void AfficherFormulaireCreationCompte()
        {
            Controls.Clear();

            int centerX = (ClientSize.Width - 180) / 2;
            int top = 20;

            /// Champs visibles pour le mot de passe et le choix de type
            txtMdp = new TextBox() { PlaceholderText = "Mot de passe", Top = top, Left = centerX, Width = 180 };
            rbUtilisateur = new RadioButton() { Text = "Utilisateur", Top = top + 40, Left = centerX };
            rbEntreprise = new RadioButton() { Text = "Entreprise", Top = top + 70, Left = centerX };

            /// Champs utilisateurs
            txtNom = new TextBox() { PlaceholderText = "Nom", Top = top + 100, Left = centerX, Width = 180 };
            txtPrenom = new TextBox() { PlaceholderText = "Prénom", Top = top + 130, Left = centerX, Width = 180 };
            txtEmail = new TextBox() { PlaceholderText = "Email", Top = top + 160, Left = centerX, Width = 180 };
            txtTel = new TextBox() { PlaceholderText = "Téléphone", Top = top + 190, Left = centerX, Width = 180 };
            txtStation = new TextBox() { PlaceholderText = "Station proche", Top = top + 220, Left = centerX, Width = 180 };

            /// Champs entreprise (mêmes positions que utilisateur)
            txtNomEntreprise = new TextBox() { PlaceholderText = "Nom entreprise", Top = top + 100, Left = centerX, Width = 180 };
            txtNomReferent = new TextBox() { PlaceholderText = "Nom référent", Top = top + 130, Left = centerX, Width = 180 };

            /// Bouton valider + message
            btnValiderCompte = new Button() { Text = "Valider", Top = top + 270, Left = (ClientSize.Width - 120) / 2, Width = 120 };
            btnValiderCompte.Click += ValiderCreationCompte;

            lblMessage = new Label() { Width = 300, Top = top + 310, Left = (ClientSize.Width - 300) / 2 };

            /// Ajout au formulaire
            Controls.AddRange(new Control[]
            {
        txtMdp, rbUtilisateur, rbEntreprise,
        txtNom, txtPrenom, txtEmail, txtTel, txtStation,
        txtNomEntreprise, txtNomReferent,
        btnValiderCompte, lblMessage
            });

            /// Logique de bascule entre utilisateur et entreprise
            rbUtilisateur.CheckedChanged += (s, e) => ToggleForm(true);
            rbEntreprise.CheckedChanged += (s, e) => ToggleForm(false);
            rbUtilisateur.Checked = true;
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

            lblMessage.Visible = true;
            MessageBox.Show($" Compte créé avec succès !\n Votre ID : {id}", "Compte créé", MessageBoxButtons.OK, MessageBoxIcon.Information);

            AfficherMenuPrincipal();
        }

        /// Affiche le menu utilisateur avec des options selon ses rôles (cuisinier, consommateur)
        public void AfficherMenuUtilisateur(int id)
        {
            /// Stocke l'ID de l'utilisateur connecté
            utilisateurID = id;

            /// Efface tous les éléments de l'interface précédents
            Controls.Clear();

            /// Vérifie les rôles de l'utilisateur
            bool estCuisinier = sql.rolecuisinier(id);
            bool estConsommateur = sql.roleconsommateur(id);

            /// Calcule la largeur et la position centrale des boutons
            int buttonWidth = 250;
            int centerX = (ClientSize.Width - buttonWidth) / 2;

            /// Ajoute un titre centré en haut
            Label titre = new Label()
            {
                Text = $"Bienvenue, utilisateur #{id}",
                Top = 20,
                Left = (ClientSize.Width - 300) / 2,
                Width = 300,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            Controls.Add(titre);

            /// Position verticale de départ pour les boutons
            int top = 60;

            /// Fonction interne pour ajouter des boutons centrés verticalement
            void AjouterBoutonCentré(string texte, Action onClick)
            {
                Button btn = new Button()
                {
                    Text = texte,
                    Top = top,
                    Left = centerX,
                    Width = buttonWidth
                };
                btn.Click += (s, e) => onClick();
                Controls.Add(btn);
                top += 40;
            }

            /// Cas 1 : l'utilisateur n'est ni cuisinier ni consommateur
            if (!estCuisinier && !estConsommateur)
            {
                AjouterBoutonCentré("Devenir cuisinier", () =>
                {
                    sql.AjouterCuisinier("cuisinier_" + id, id);
                    AfficherMenuUtilisateur(id);
                });

                AjouterBoutonCentré("Devenir consommateur", () =>
                {
                    sql.AjouterConsommateur(id);
                    AfficherMenuUtilisateur(id);
                });
            }
            /// Cas 2 : utilisateur est cuisinier seulement
            else if (estCuisinier && !estConsommateur)
            {
                AjouterBoutonCentré("Voir mes commandes", () => AfficherCommandes(id));
                AjouterBoutonCentré("Voir mes plats", AfficherPlatsCuisinier);
                AjouterBoutonCentré("Ajouter plat", AfficherFormAjoutPlat);
                AjouterBoutonCentré("Devenir consommateur", () =>
                {
                    sql.AjouterConsommateur(id);
                    AfficherMenuUtilisateur(id);
                });
            }
            /// Cas 3 : utilisateur est consommateur seulement
            else if (!estCuisinier && estConsommateur)
            {
                AjouterBoutonCentré("Plats disponibles", AfficherPlatsDisponibles);
                AjouterBoutonCentré("Rechercher plat", RechercherPlat);
                AjouterBoutonCentré("Voir mes commandes", () => AfficherCommandes(id));
                AjouterBoutonCentré("Ajouter élément à commande", AjouterElementCommande);
                AjouterBoutonCentré("Passer une commande", PasserCommande);
                AjouterBoutonCentré("Noter une commande", NoterCommande);
                AjouterBoutonCentré("Devenir cuisinier", () =>
                {
                    sql.AjouterCuisinier("cuisinier_" + id, id);
                    AfficherMenuUtilisateur(id);
                });
            }
            /// Cas 4 : utilisateur est à la fois cuisinier et consommateur
            else
            {
                AjouterBoutonCentré("Voir plats", AfficherPlatsDisponibles);
                AjouterBoutonCentré("Rechercher plat", RechercherPlat);
                AjouterBoutonCentré("Mes commandes (conso)", () => AfficherCommandes(id));
                AjouterBoutonCentré("Ajouter élément", AjouterElementCommande);
                AjouterBoutonCentré("Passer commande", PasserCommande);
                AjouterBoutonCentré("Noter commande", NoterCommande);
                AjouterBoutonCentré("Voir plats (cuisinier)", AfficherPlatsCuisinier);
                AjouterBoutonCentré("Ajouter plat (cuisinier)", AfficherFormAjoutPlat);
            }

            /// Ajoute un bouton retour en bas de page
            top += 20;
            Button btnRetour = new Button()
            {
                Text = "Retour au menu principal",
                Top = top,
                Left = centerX,
                Width = buttonWidth
            };
            btnRetour.Click += (s, e) => AfficherMenuPrincipal(); // Retour au menu d’accueil
            Controls.Add(btnRetour);
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

            ListBox listBox = new ListBox() { Top = 60, Left = 50, Width = 250, Height = 300 };
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
            /// Afficher les cuisiniers disponibles dans la console
            var cuisiniers = sql.ObtenirTousLesCuisiniers(); /// à implémenter dans ta classe SQL

            if (cuisiniers.Count == 0)
            {
                MessageBox.Show("Aucun cuisinier disponible pour le moment.");
                return;
            }

            Console.WriteLine("Liste des cuisiniers disponibles :");
            foreach (var (id, pseudo) in cuisiniers)
            {
                Console.WriteLine($"ID : {id}, Pseudo : {pseudo}");
            }

            string inputId = Prompt.ShowDialog("Entrez l'ID du cuisinier :", "Passer une commande");

            if (!int.TryParse(inputId, out int idCuisinier))
            {
                MessageBox.Show("ID invalide.");
                return;
            }

            if (!sql.rolecuisinier(idCuisinier)) /// Attention ici ! c'était inversé dans ton code
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
            int idConsommateur = sql.idduconsomateur(utilisateurID);
            int idCompteCuisinier = sql.idducuisinier(utilisateurID); 
            int idCuisinier = sql.idducuisinier(utilisateurID);


            if (idConsommateur == idCompteCuisinier)
            {
                MessageBox.Show("Tu ne peux pas noter ta propre commande !");
                return;
            }

            if (idCuisinier <=0)
            {
                MessageBox.Show("Erreur : ID du cuisinier introuvable.");
                return;
            }

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

            sql.NoterCommande(idCommande, idCuisinier, idConsommateur, noteClient, commentaireClient, noteCuisinier, commentaireCuisinier);

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
            var plats = sql.AfficherTousLesMetsducuisto(idCuisinier);

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

            Label lblTitre = new Label()
            {
                Text = "Statistiques",
                Top = 20,
                Left = 50,
                Width = 600,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            Controls.Add(lblTitre);

            // Zone d'affichage des résultats
            TextBox txtResultats = new TextBox()
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Top = 60,
                Left = 50,
                Width = 680,
                Height = 300,
                Font = new Font("Consolas", 10)
            };
            Controls.Add(txtResultats);

            /// Capture de la console
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            /// Appels aux méthodes SQL
            sql.AfficherLivraisonsParCuisinier();
            sql.AfficherMoyennePrixCommandes();

            /// Restauration de la console standard
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

            /// Affichage dans la TextBox
            txtResultats.Text = stringWriter.ToString();

            /// Bouton retour
            Button btnRetour = new Button()
            {
                Text = "Retour",
                Top = 380,
                Left = 50,
                Width = 200
            };
            btnRetour.Click += (s, e) => AfficherInterfaceAdmin();
            Controls.Add(btnRetour);
        }


        private void AfficherInterfaceAdmin()
        {
            Controls.Clear();

            Label lblTitre = new Label()
            {
                Text = "Console SQL (admin)",
                Top = 20,
                Left = (ClientSize.Width - 300) / 2,
                Width = 300,
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(lblTitre);

            TextBox txtRequete = new TextBox()
            {
                Multiline = true,
                Width = 600,
                Height = 200,
                Top = 60,
                Left = (ClientSize.Width - 600) / 2,
                Font = new Font("Consolas", 10),
                ScrollBars = ScrollBars.Vertical
            };
            Controls.Add(txtRequete);

            Label lblResultat = new Label()
            {
                Text = "",
                Top = 270,
                Left = (ClientSize.Width - 600) / 2,
                Width = 600,
                Height = 60,
                ForeColor = Color.DarkGreen
            };
            Controls.Add(lblResultat);

            Button btnExecuter = new Button()
            {
                Text = "Exécuter requête",
                Top = 340,
                Left = (ClientSize.Width - 310) / 2,
                Width = 150
            };
            btnExecuter.Click += (s, e) =>
            {
                try
                {
                    SQL.ExecuterRequeteLibre(txtRequete.Text);
                    lblResultat.ForeColor = Color.DarkGreen;
                    lblResultat.Text = "Requête exécutée avec succès.";
                }
                catch (Exception ex)
                {
                    lblResultat.ForeColor = Color.DarkRed;
                    lblResultat.Text = "Erreur : " + ex.Message;
                }
            };
            Controls.Add(btnExecuter);

            Button btnStatistiques = new Button()
            {
                Text = "Statistiques",
                Top = 340,
                Left = (ClientSize.Width + 10) / 2,
                Width = 150
            };
            btnStatistiques.Click += (s, e) => AfficherStatistiques();
            Controls.Add(btnStatistiques);

            Button btnRetour = new Button()
            {
                Text = "Retour",
                Top = 400,
                Left = (ClientSize.Width - 150) / 2,
                Width = 150
            };
            btnRetour.Click += (s, e) => AfficherMenuPrincipal();

            Controls.Add(btnRetour);
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
            TextBox txtInput = new TextBox() { Left = 10, Top = 40, Width = 261 };
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
