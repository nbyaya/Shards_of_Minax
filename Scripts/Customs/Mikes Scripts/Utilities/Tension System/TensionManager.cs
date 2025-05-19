using System;
using System.IO;
using Server;
using System.Globalization; // 需要这个命名空间来处理浮点数的文化差异

namespace Bittiez.CustomSystems
{
    public static class TensionManager
    {
        // The file path where the tension value is saved.
        private static readonly string FilePath = "tension.dat";

        // Backing field for the tension tally.
        private static double m_Tension;

        public static double Tension
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

        public static void IncreaseTension(double amount)
        {
            m_Tension += amount;
            Save();
        }

        public static void SetTension(double amount)
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
                File.WriteAllText(FilePath, m_Tension.ToString("F3"));
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
                    double parsedTension;
					// 使用 double.TryParse 尝试解析浮点数，并指定 InvariantCulture
					if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out parsedTension))
					{
						m_Tension = parsedTension;
					}
					else
					{
					
					    // 如果解析失败，将张力设置为 0
						Console.WriteLine($"[TensionManager] Could not parse tension value '{text}' from file. Setting to 0.");
						m_Tension = 0;
					}					
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
