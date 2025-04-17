using System;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Text;
using Server;
using Server.Commands;
using Server.Commands.Generic;

namespace Server.Customs.Invasion_System
{
    public class InvasionRegionConverter
    {
        // Converts every region from the XML file that has a "name" attribute.
        // Optionally outputs the facet name if the region is nested within a <Facet> element.
        public static string ConvertXmlRegions(string xmlFilePath)
        {
            StringBuilder output = new StringBuilder();

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFilePath);

            // Select all <region> nodes that have a "name" attribute.
            XmlNodeList regionNodes = doc.SelectNodes("//region[@name]");

            foreach (XmlNode regionNode in regionNodes)
            {
                string regionName = regionNode.Attributes["name"].Value;

                // Determine the facet name if available.
                string facetName = "Unknown";
                XmlNode facetNode = regionNode.SelectSingleNode("ancestor::Facet");
                if (facetNode != null && facetNode.Attributes["name"] != null)
                    facetName = facetNode.Attributes["name"].Value;

                int minX = int.MaxValue, minY = int.MaxValue;
                int maxX = int.MinValue, maxY = int.MinValue;
                bool foundRect = false;

                // Process all <rect> elements inside this region (including nested ones)
                XmlNodeList rectNodes = regionNode.SelectNodes(".//rect");
                foreach (XmlNode rectNode in rectNodes)
                {
                    // Ensure that all required attributes exist
                    if (rectNode.Attributes["x"] == null ||
                        rectNode.Attributes["y"] == null ||
                        rectNode.Attributes["width"] == null ||
                        rectNode.Attributes["height"] == null)
                    {
                        continue; // Skip this <rect> if any attribute is missing.
                    }

                    int x = int.Parse(rectNode.Attributes["x"].Value, CultureInfo.InvariantCulture);
                    int y = int.Parse(rectNode.Attributes["y"].Value, CultureInfo.InvariantCulture);
                    int width = int.Parse(rectNode.Attributes["width"].Value, CultureInfo.InvariantCulture);
                    int height = int.Parse(rectNode.Attributes["height"].Value, CultureInfo.InvariantCulture);

                    minX = Math.Min(minX, x);
                    minY = Math.Min(minY, y);
                    maxX = Math.Max(maxX, x + width);
                    maxY = Math.Max(maxY, y + height);
                    foundRect = true;
                }

                // If no rects were found, skip this region.
                if (!foundRect)
                    continue;

                // Determine a spawn point – default to the center of the bounding box
                int spawnX = (minX + maxX) / 2;
                int spawnY = (minY + maxY) / 2;
                int spawnZ = 0;

                // If a <go> element exists, use its coordinates instead.
                XmlNode goNode = regionNode.SelectSingleNode("go");
                if (goNode != null &&
                    goNode.Attributes["x"] != null &&
                    goNode.Attributes["y"] != null &&
                    goNode.Attributes["z"] != null)
                {
                    spawnX = int.Parse(goNode.Attributes["x"].Value, CultureInfo.InvariantCulture);
                    spawnY = int.Parse(goNode.Attributes["y"].Value, CultureInfo.InvariantCulture);
                    spawnZ = int.Parse(goNode.Attributes["z"].Value, CultureInfo.InvariantCulture);
                }

                // Append the converted region output.
                output.AppendLine(String.Format("Facet: \"{0}\", Region: \"{1}\"", facetName, regionName));
                output.AppendLine(String.Format("Top = new Point3D({0}, {1}, {2});", minX, minY, spawnZ));
                output.AppendLine(String.Format("Bottom = new Point3D({0}, {1}, {2});", maxX, maxY, spawnZ));
                output.AppendLine(String.Format("SpawnPoint = new Point3D({0}, {1}, {2});", spawnX, spawnY, spawnZ));
                output.AppendLine("// Additional properties (MinSpawnZ/MaxSpawnZ, Map, etc.) can be set as needed.");
                output.AppendLine(new string('-', 50));
                output.AppendLine();
            }

            return output.ToString();
        }
    }

    // This class registers an admin command that calls the conversion routine and writes the output to a file.
    public class ConvertInvasionRegionsCommand
    {
        public static void Initialize()
        {
            // Registers the command "ConvertRegions" accessible only to GameMasters.
            CommandSystem.Register("ConvertRegions", AccessLevel.GameMaster, new CommandEventHandler(ConvertRegions_OnCommand));
        }

        [Usage("ConvertRegions")]
        [Description("Converts every XML region into invasion system data and writes the output to a file.")]
        public static void ConvertRegions_OnCommand(CommandEventArgs e)
        {
            try
            {
                // Specify the file paths. Adjust these as necessary for your server.
                string xmlFilePath = Path.Combine(Core.BaseDirectory, "ServerRegions.xml");
                string outputFilePath = Path.Combine(Core.BaseDirectory, "InvasionConvertedRegions.txt");

                // Call the conversion method.
                string convertedData = InvasionRegionConverter.ConvertXmlRegions(xmlFilePath);

                // Write the converted data to a file.
                File.WriteAllText(outputFilePath, convertedData);

                e.Mobile.SendMessage("Region conversion complete. Output written to: " + outputFilePath);
            }
            catch (Exception ex)
            {
                e.Mobile.SendMessage("An error occurred during conversion: " + ex.Message);
            }
        }
    }
}
