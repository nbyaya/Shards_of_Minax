using System;
using System.IO;
using Server;

namespace Bittiez.CustomSystems
{
    public static class TensionManager
    {
        // The file path where the tension value is saved.
        private static readonly string FilePath = "tension.dat";

        // Backing field for the tension tally.
        private static int m_Tension;

        public static int Tension
        {
            get { return m_Tension; }
            set
            {
                m_Tension = value;
                Save();
            }
        }

        // Static constructor loads the tension when the script is first run.
        static TensionManager()
        {
            Load();
        }

        public static void IncreaseTension(int amount)
        {
            m_Tension += amount;
            Save();
        }

        public static void SetTension(int amount)
        {
            m_Tension = amount;
            Save();
        }

        // Example persistence methods using a simple file. You may wish to use your own persistence mechanism.
        public static void Save()
        {
            try
            {
                // Write tension value to file
                File.WriteAllText(FilePath, m_Tension.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TensionManager] Error saving tension: {ex.Message}");
            }
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string text = File.ReadAllText(FilePath);
                    int.TryParse(text, out m_Tension);
                }
                else
                {
                    m_Tension = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TensionManager] Error loading tension: {ex.Message}");
                m_Tension = 0;
            }
        }
    }
}
