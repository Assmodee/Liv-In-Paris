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
            int width = 3000, height = 3000; // Meilleure résolution
            int borderPadding = 150;
            float nodeRadius = 14f;
            Font labelFont = new Font("Arial", 8);
            Brush labelBrush = Brushes.Black;

            // Déterminer les bornes géographiques
            double minLat = double.MaxValue, maxLat = double.MinValue;
            double minLon = double.MaxValue, maxLon = double.MinValue;

            foreach (var node in nodes)
            {
                if (node.Id == 0) continue;
                minLat = Math.Min(minLat, node.Latitude);
                maxLat = Math.Max(maxLat, node.Latitude);
                minLon = Math.Min(minLon, node.Longitude);
                maxLon = Math.Max(maxLon, node.Longitude);
            }

            // Création de l'image
            using (Bitmap bitmap = new Bitmap(width, height))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.Clear(Color.White);

                // Dessiner les arcs avec flèches bleues
                Pen edgePen = new Pen(Color.Blue, 2);
                AdjustableArrowCap arrowCap = new AdjustableArrowCap(4, 6);
                edgePen.CustomEndCap = arrowCap;

                foreach (var node in nodes)
                {
                    if (node.Id == 0) continue;

                    float x1 = MapCoord(node.Longitude, minLon, maxLon, borderPadding, width - borderPadding);
                    float y1 = MapCoord(node.Latitude, maxLat, minLat, borderPadding, height - borderPadding);

                    foreach (int toId in connexions[node.Id])
                    {
                        var dest = nodes[toId];
                        float x2 = MapCoord(dest.Longitude, minLon, maxLon, borderPadding, width - borderPadding);
                        float y2 = MapCoord(dest.Latitude, maxLat, minLat, borderPadding, height - borderPadding);

                        g.DrawLine(edgePen, x1, y1, x2, y2);
                    }
                }

                // Dessiner les noeuds rouges avec labels
                foreach (var node in nodes)
                {
                    if (node.Id == 0) continue;
                    float x = MapCoord(node.Longitude, minLon, maxLon, borderPadding, width - borderPadding);
                    float y = MapCoord(node.Latitude, maxLat, minLat, borderPadding, height - borderPadding);

                    g.FillEllipse(Brushes.Red, x - nodeRadius / 2, y - nodeRadius / 2, nodeRadius, nodeRadius);
                    g.DrawEllipse(Pens.Black, x - nodeRadius / 2, y - nodeRadius / 2, nodeRadius, nodeRadius);

                    // Afficher ID et nom de station
                    string label = $"{node.Id}: {node.Value}";
                    g.DrawString(label, labelFont, labelBrush, x + 6, y + 6);
                }

                // Sauvegarde du fichier
                bitmap.Save(filename);
            }
        }

        /// <summary>
        /// Convertit une coordonnée géographique (lat/lon) en coordonnée d'écran
        /// </summary>
        private static float MapCoord(double value, double min, double max, float outMin, float outMax)
        {
            return (float)((value - min) / (max - min) * (outMax - outMin) + outMin);
        }
    }
}
