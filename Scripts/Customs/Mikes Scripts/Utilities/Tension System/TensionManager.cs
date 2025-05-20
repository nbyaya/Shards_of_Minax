using System;
using System.IO;
using Server;
using System.Globalization; // Needed for CultureInfo.InvariantCulture

namespace Bittiez.CustomSystems
{
    public static class TensionManager
    {
        // The file path where the tension value is saved.
        private static readonly string FilePath = "logs/tension_settings.dat";

        // Backing field for the tension tally.
        private static double m_Tension;

        public static double Tension
        {
            get { return m_Tension; }
            set
            {
                m_Tension = value;
                if (m_Tension < 0.0) m_Tension = 0.0;
                Save();
            }
        }

        // Variable for the tension reduction amount.
        private static double m_TensionReductionAmount = 500.0; // Default value

        public static double TensionReductionAmount
        {
            get { return m_TensionReductionAmount; }
            set
            {
                m_TensionReductionAmount = value;
                if (m_TensionReductionAmount < 0.0) m_TensionReductionAmount = 0.0;
                Save();
            }
        }

        // *** 新增：生物死亡时增加的张力值 ***
        private static double m_CreatureDeathTensionIncrement = 0.001d; // 默认值

        public static double CreatureDeathTensionIncrement
        {
            get { return m_CreatureDeathTensionIncrement; }
            set
            {
                m_CreatureDeathTensionIncrement = value;
                if (m_CreatureDeathTensionIncrement < 0.0) m_CreatureDeathTensionIncrement = 0.0; // 可以根据需求允许负数，但通常增量为正
                Save();
            }
        }

        static TensionManager()
        {
            Load();
        }

        public static void IncreaseTension(double amount)
        {
            m_Tension += amount;
            if (m_Tension < 0.0) m_Tension = 0.0;
			
            Save();
        }

        public static void Save()
        {
            try
            {
                string directory = Path.GetDirectoryName(FilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    writer.WriteLine(m_Tension.ToString("R", CultureInfo.InvariantCulture));
                    writer.WriteLine(m_TensionReductionAmount.ToString("R", CultureInfo.InvariantCulture));
                    // *** 新增：保存生物死亡张力增量值 ***
                    writer.WriteLine(m_CreatureDeathTensionIncrement.ToString("R", CultureInfo.InvariantCulture));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TensionManager] Error saving tension settings: {ex.Message}");
            }
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    using (StreamReader reader = new StreamReader(FilePath))
                    {
                        string tensionText = reader.ReadLine();
                        if (tensionText != null && double.TryParse(tensionText, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedTension))
                        {
                            m_Tension = parsedTension;
                        }
                        else
                        {
                            m_Tension = 0.0;
                            Console.WriteLine("[TensionManager] Could not parse tension from file. Setting to default 0.");
                        }

                        string reductionText = reader.ReadLine();
                        if (reductionText != null && double.TryParse(reductionText, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedReduction))
                        {
                            m_TensionReductionAmount = parsedReduction;
                        }
                        else
                        {
                            m_TensionReductionAmount = 500.0; // 保留默认值
                            Console.WriteLine($"[TensionManager] Could not parse tension reduction amount from file. Using default {m_TensionReductionAmount}.");
                        }

                        // *** 新增：加载生物死亡张力增量值 ***
                        string creatureDeathIncrementText = reader.ReadLine();
                        if (creatureDeathIncrementText != null && double.TryParse(creatureDeathIncrementText, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedCreatureDeathIncrement))
                        {
                            m_CreatureDeathTensionIncrement = parsedCreatureDeathIncrement;
                        }
                        else
                        {
                            m_CreatureDeathTensionIncrement = 0.001d; // 保留默认值
                            Console.WriteLine($"[TensionManager] Could not parse creature death tension increment from file. Using default {m_CreatureDeathTensionIncrement}.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"[TensionManager] Settings file not found. Initializing with default values.");
                    m_Tension = 0.0;
                    m_TensionReductionAmount = 500.0;
                    m_CreatureDeathTensionIncrement = 0.001d; // 设置为默认值
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TensionManager] Error loading tension settings: {ex.Message}");
                m_Tension = 0.0;
                m_TensionReductionAmount = 500.0;
                m_CreatureDeathTensionIncrement = 0.001d; // 发生错误时设置为默认值
            }
        }
    }
}