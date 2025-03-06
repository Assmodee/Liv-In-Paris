using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Liv_In_Paris
{
    class Drawing
    {
        /// <summary>
        /// Dessine un graphe non orienté à partir de sa matrice d'adjacence et de ses degrés.
        /// </summary>
        /// <param name="adjacencyMatrix"></param>
        /// <param name="degrees"></param>
        /// <param name="filename"></param>
        public static void DrawGraph(List<List<int>> adjacencyMatrix, int[] degrees, string filename)
        {
            /// premier initialisation pour les elements de base 
            int nodeCount = adjacencyMatrix.Count; /// Nombre de nœuds
            int width = 1300, height = 1300;  /// Taille du canvas
            int minRadius = 30, maxRadius = 60; /// Taille des nœuds
            int borderPadding = 50;  /// Marge autour du graphe

                                     /// Initialisation des positions sur un cercle

                                     /// ces intitialisations sont fait pour un cercle le plus grand possible qui ne de pase pas le marge imposé
            PointF[] positions = new PointF[nodeCount];
            float graphRadius = (width - 2 * borderPadding) / 2.0f;
            float centerX = width / 2, centerY = height / 2;

            /// On place les noeuds sur un cercle
            for (int i = 0; i < nodeCount; i++)
            {
                double angle = i * 2 * Math.PI / nodeCount;
                positions[i] = new PointF(
                    centerX + (float)(graphRadius * Math.Cos(angle)),
                    centerY + (float)(graphRadius * Math.Sin(angle))
                );
            }

            /// Simulation Force-Directed Layout
            int iterations = 2000;  /// nombre d'iterations / le temps de simulation
            float repulsionStrength = 1000000f;  /// Force de répulsion entre les nœuds
            float attractionStrength = 0.0003f;   /// Force d'attraction pour les arêtes
            float maxDisplacement = 100f;        /// Limite de déplacement des nœuds


            /// On fait les iterations pour chaque noeud
            for (int iter = 0; iter < iterations; iter++)
            {
                PointF[] displacements = new PointF[nodeCount];

                // Répulsion entre les nœuds
                for (int i = 0; i < nodeCount; i++)
                {
                    for (int j = 0; j < nodeCount; j++)
                    {
                        /// On ne prend pas en compte le cas ou on a le même noeud
                        if (i == j) continue;
                        /// On calcule la distance entre les noeuds
                        float dx = positions[i].X - positions[j].X;
                        float dy = positions[i].Y - positions[j].Y;
                        /// On calcule la force de répulsion
                        float distanceSquared = dx * dx + dy * dy + 0.1f;
                        /// On calcule le vecteur de déplacement
                        float distance = (float)Math.Sqrt(distanceSquared);
                        /// On calcule la force
                        float force = repulsionStrength / distanceSquared;
                        /// On ajoute le vecteur de déplacement
                        displacements[i].X += (dx / distance) * force;
                        displacements[i].Y += (dy / distance) * force;
                    }
                }

                /// Attraction pour les arêtes -> fonctionnement similaire a la repulsion
                for (int i = 0; i < nodeCount; i++)
                {
                    for (int j = 0; j < nodeCount; j++)
                    {
                        if (adjacencyMatrix[i][j] == 1)
                        {
                            float dx = positions[j].X - positions[i].X;
                            float dy = positions[j].Y - positions[i].Y;
                            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
                            float force = attractionStrength * distance;

                            displacements[i].X += dx * force;
                            displacements[i].Y += dy * force;
                            displacements[j].X -= dx * force;
                            displacements[j].Y -= dy * force;
                        }
                    }
                }

                /// On déplace les noeuds
                for (int i = 0; i < nodeCount; i++)
                {
                    positions[i].X += Math.Max(-maxDisplacement, Math.Min(maxDisplacement, displacements[i].X));
                    positions[i].Y += Math.Max(-maxDisplacement, Math.Min(maxDisplacement, displacements[i].Y));

                    /// Empêcher les nœuds de sortir du canvas
                    positions[i].X = Math.Min(width - borderPadding, Math.Max(borderPadding, positions[i].X));
                    positions[i].Y = Math.Min(height - borderPadding, Math.Max(borderPadding, positions[i].Y));
                }
            }

            /// Création de l'image
            using (Bitmap bitmap = new Bitmap(width, height))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.FromArgb(165, 192, 197));////////////////////////////////

                /// Dessin des arêtes
                Pen edgePen = new Pen(Color.Black, 2);
                for (int i = 0; i < nodeCount; i++)
                {
                    for (int j = i + 1; j < nodeCount; j++)
                    {
                        if (adjacencyMatrix[i][j] == 1)
                        {
                            g.DrawLine(edgePen, positions[i], positions[j]);
                        }
                    }
                }

                /// Dessin des nœuds qui depend du degrée (taille et couleur)
                for (int i = 0; i < nodeCount; i++)
                {
                    float sizeFactor = Math.Max(0, Math.Min(1, degrees[i] / 10f));
                    int nodeRadius = (int)(minRadius + sizeFactor * (maxRadius - minRadius));

                    float x = positions[i].X - nodeRadius / 2;
                    float y = positions[i].Y - nodeRadius / 2;
                    /// On récupère la couleur du noeud(en bas)
                    Color nodeColor = GetNodeColor(degrees[i]);
                    using (Brush brush = new SolidBrush(nodeColor))
                    {
                        g.FillEllipse(brush, x, y, nodeRadius, nodeRadius);
                    }
                    g.DrawEllipse(Pens.Black, x, y, nodeRadius, nodeRadius);

                    /// Texte centré
                    using (Font font = new Font("Arial", 12, FontStyle.Bold))
                    {
                        SizeF textSize = g.MeasureString(i.ToString(), font);
                        g.DrawString(i.ToString(), font, Brushes.White,
                            x + (nodeRadius - textSize.Width) / 2,
                            y + (nodeRadius - textSize.Height) / 2);
                    }
                }


                /// Dessin du logo
                using (Image logo = Image.FromFile("logo.jpeg"))
                {
                    int logoSize = 300;
                    Rectangle logoRect = new Rectangle(width - logoSize - 20, height - logoSize - 20, logoSize, logoSize);
                    g.DrawImage(logo, logoRect);
                }

                using (Image logo = Image.FromFile("1.png"))
                {
                    int logoSize = 500;
                    Rectangle logoRect = new Rectangle(20, 20, logoSize, (int)Math.Floor(logoSize/2.187));
                    g.DrawImage(logo, logoRect);
                }

                /// saufgarse de l'image
                bitmap.Save(filename);
            }
        }

        /// <summary>
        /// methode pour faire les noeuds avec un degrée plus grande d'une couleur plus rouge
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        private static Color GetNodeColor(int degree)
        {
            float ratio = Math.Min(degree / 10f, 1.0f);
            int red = (int)(255 * ratio);
            int blue = (int)(255 * (1 - ratio));
            return Color.FromArgb(red, 0, blue);
        }

        
    }
}
