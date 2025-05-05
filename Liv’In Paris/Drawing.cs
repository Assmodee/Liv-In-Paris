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
        public static void DrawGraphFromCoordinates(
    List<Node<string>> nodes,
    List<List<int>> connexions,
    string filename,
    Dictionary<int, List<int>> colorMap)
        {
            /// <summary>
            /// Dessine un graphe orienté basé sur les coordonnées latitude/longitude des stations,
            /// avec coloration des nœuds selon les groupes générés par l'algorithme de Welsh-Powell.
            /// </summary>

            int width = 6000, height = 6000;
            int borderPadding = 150;
            float nodeRadius = 24f;
            Font labelFont = new Font("Arial", 6);
            Brush labelBrush = Brushes.Black;

            /// Initialisation des bornes min/max des coordonnées géographiques
            double minLat = double.MaxValue, maxLat = double.MinValue;
            double minLon = double.MaxValue, maxLon = double.MinValue;

            /// Parcours pour détecter les limites géographiques du graphe
            foreach (var node in nodes)
            {
                if (node.Id == 0) continue;
                minLat = Math.Min(minLat, node.Latitude);
                maxLat = Math.Max(maxLat, node.Latitude);
                minLon = Math.Min(minLon, node.Longitude);
                maxLon = Math.Max(maxLon, node.Longitude);
            }

            /// Définition d'une palette de couleurs
            Color[] palette = new Color[] {
        Color.Red, Color.Blue, Color.Green, Color.Orange,
        Color.Purple, Color.Brown, Color.Magenta, Color.Cyan,
        Color.Yellow, Color.Teal, Color.Gray, Color.Pink
    };

            /// Construction du dictionnaire nodeId -> couleur
            Dictionary<int, Color> nodeColors = new Dictionary<int, Color>();
            foreach (var kvp in colorMap)
            {
                int colorIndex = kvp.Key % palette.Length;
                foreach (int nodeId in kvp.Value)
                {
                    nodeColors[nodeId] = palette[colorIndex];
                }
            }

            using (Bitmap bitmap = new Bitmap(width, height))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                /// Fond légèrement gris
                g.Clear(Color.FromArgb(240, 240, 240));

                /// Définition du stylo pour les arêtes avec flèche grise
                Pen edgePen = new Pen(Color.Gray, 3);
                AdjustableArrowCap arrowCap = new AdjustableArrowCap(5, 10);
                edgePen.CustomEndCap = arrowCap;

                /// Dessin des arêtes
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

                        float dx = x2 - x1, dy = y2 - y1;
                        float distance = (float)Math.Sqrt(dx * dx + dy * dy);
                        float shorten = nodeRadius / 1.5f;

                        if (distance > shorten * 2)
                        {
                            x1 += dx / distance * shorten;
                            y1 += dy / distance * shorten;
                            x2 -= dx / distance * shorten;
                            y2 -= dy / distance * shorten;
                        }

                        g.DrawLine(edgePen, x1, y1, x2, y2);
                    }
                }

                /// Dessin des nœuds (cercles colorés) et des labels
                foreach (var node in nodes)
                {
                    if (node.Id == 0) continue;

                    float x = MapCoord(node.Longitude, minLon, maxLon, borderPadding, width - borderPadding);
                    float y = MapCoord(node.Latitude, maxLat, minLat, borderPadding, height - borderPadding);

                    /// Couleur assignée selon Welsh-Powell
                    Color color = nodeColors.ContainsKey(node.Id) ? nodeColors[node.Id] : Color.Red;

                    using (Brush nodeBrush = new SolidBrush(color))
                    {
                        g.FillEllipse(nodeBrush, x - nodeRadius / 2, y - nodeRadius / 2, nodeRadius, nodeRadius);
                    }

                    g.DrawEllipse(Pens.Black, x - nodeRadius / 2, y - nodeRadius / 2, nodeRadius, nodeRadius);

                    string label = $"{node.Id}: {node.Value}";
                    SizeF labelSize = g.MeasureString(label, labelFont);
                    g.DrawString(label, labelFont, labelBrush, x - labelSize.Width / 2, y + nodeRadius / 2 + 2);
                }

                /// Sauvegarde dans un fichier image
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
