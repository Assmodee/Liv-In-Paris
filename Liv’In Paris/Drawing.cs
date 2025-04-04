using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Liv_In_Paris
{
    class Drawing
    {
        /// <summary>
        /// Dessine un graphe orienté basé sur les coordonnées latitude/longitude des stations.
        /// </summary>
        public static void DrawGraphFromCoordinates(List<Node<string>> nodes, List<List<int>> connexions, string filename)
        {
            /// Taille de l'image finale
            int width = 6000, height = 6000;

            /// Marge autour du graphe
            int borderPadding = 150;

            /// Rayon des cercles représentant les noeuds (augmenté)
            float nodeRadius = 24f;

            /// Police et pinceau pour afficher les noms des stations
            Font labelFont = new Font("Arial", 6);
            Brush labelBrush = Brushes.Black;

            /// Initialisation des bornes min/max des coordonnées géographiques
            double minLat = double.MaxValue, maxLat = double.MinValue;
            double minLon = double.MaxValue, maxLon = double.MinValue;

            /// Parcours pour détecter les limites géographiques du graphe
            foreach (var node in nodes)
            {
                if (node.Id == 0) continue; /// Sauter un éventuel noeud nul ou spécial
                minLat = Math.Min(minLat, node.Latitude);
                maxLat = Math.Max(maxLat, node.Latitude);
                minLon = Math.Min(minLon, node.Longitude);
                maxLon = Math.Max(maxLon, node.Longitude);
            }

            /// Création de l'image et initialisation du contexte graphique
            using (Bitmap bitmap = new Bitmap(width, height))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.HighQuality; /// Antialiasing pour les formes
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; /// Texte plus net

                /// Fond légèrement gris 
                g.Clear(Color.FromArgb(240, 240, 240));

                /// Définition du stylo pour les arêtes avec flèche bleue
                Pen edgePen = new Pen(Color.Gray, 3); /// Ligne plus épaisse
                AdjustableArrowCap arrowCap = new AdjustableArrowCap(5, 10); /// Plus grande flèche
                edgePen.CustomEndCap = arrowCap; /// Ajout de la flèche en bout

                /// Dessin des arêtes (connexions entre noeuds)
                foreach (var node in nodes)
                {
                    if (node.Id == 0) continue;

                    /// Conversion des coordonnées géographiques en pixels (point de départ)
                    float x1 = MapCoord(node.Longitude, minLon, maxLon, borderPadding, width - borderPadding);
                    float y1 = MapCoord(node.Latitude, maxLat, minLat, borderPadding, height - borderPadding);

                    /// Pour chaque voisin du noeud actuel
                    foreach (int toId in connexions[node.Id])
                    {
                        var dest = nodes[toId];

                        /// Conversion des coordonnées géographiques en pixels (point d'arrivée)
                        float x2 = MapCoord(dest.Longitude, minLon, maxLon, borderPadding, width - borderPadding);
                        float y2 = MapCoord(dest.Latitude, maxLat, minLat, borderPadding, height - borderPadding);

                        /// Calcul du vecteur entre les deux points
                        float dx = x2 - x1, dy = y2 - y1;
                        float distance = (float)Math.Sqrt(dx * dx + dy * dy);
                        float shorten = nodeRadius / 1.5f; /// Distance à réduire pour éviter que la flèche touche le cercle

                        /// Réduction des extrémités pour ne pas coller aux cercles
                        if (distance > shorten * 2)
                        {
                            x1 += dx / distance * shorten;
                            y1 += dy / distance * shorten;
                            x2 -= dx / distance * shorten;
                            y2 -= dy / distance * shorten;
                        }

                        /// Dessin de la ligne avec flèche
                        g.DrawLine(edgePen, x1, y1, x2, y2);
                    }
                }

                /// Dessin des noeuds (cercles rouges) et des labels
                foreach (var node in nodes)
                {
                    if (node.Id == 0) continue;

                    /// Position du noeud sur l'image
                    float x = MapCoord(node.Longitude, minLon, maxLon, borderPadding, width - borderPadding);
                    float y = MapCoord(node.Latitude, maxLat, minLat, borderPadding, height - borderPadding);

                    /// Dessin du cercle rouge
                    g.FillEllipse(Brushes.Red, x - nodeRadius / 2, y - nodeRadius / 2, nodeRadius, nodeRadius);
                    g.DrawEllipse(Pens.Black, x - nodeRadius / 2, y - nodeRadius / 2, nodeRadius, nodeRadius);

                    /// Construction du texte à afficher
                    string label = $"{node.Id}: {node.Value}";

                    /// Calcul de la taille du texte pour bien le centrer
                    SizeF labelSize = g.MeasureString(label, labelFont);

                    /// Dessin du texte en-dessous du cercle, centré horizontalement
                    g.DrawString(label, labelFont, labelBrush, x - labelSize.Width / 2, y + nodeRadius / 2 + 2);
                }

                /// Sauvegarde du résultat dans un fichier image
                bitmap.Save(filename);
            }
        }

        /// <summary>
        /// Convertit une coordonnée géographique en coordonnée sur l'image
        /// </summary>
        private static float MapCoord(double value, double min, double max, float outMin, float outMax)
        {
            return (float)((value - min) / (max - min) * (outMax - outMin) + outMin);
        }
    }
}
