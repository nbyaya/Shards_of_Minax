using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class BlacksmithCollectionType
    {
        public static List<BlacksmithCollectionType> Items = new List<BlacksmithCollectionType>();

        public Type ItemType { get; private set; }
        public string Name { get; private set; }
        public double MinDifficulty { get; private set; }
        public double MaxDifficulty { get; private set; }
        public Type MaterialType { get; private set; }
        public int MaterialAmount { get; private set; }

        // Additional resources (optional)
        public Dictionary<Type, int> AdditionalResources { get; private set; }

        public BlacksmithCollectionType(Type itemType, string name, double minDifficulty, double maxDifficulty, Type materialType, int materialAmount, Dictionary<Type, int> additionalResources = null)
        {
            ItemType = itemType;
            Name = name;
            MinDifficulty = minDifficulty;
            MaxDifficulty = maxDifficulty;
            MaterialType = materialType;
            MaterialAmount = materialAmount;
            AdditionalResources = additionalResources ?? new Dictionary<Type, int>();
        }
        public static void PopulateBlacksmithCollection()
        {
            Items = new List<BlacksmithCollectionType>
            {
                // Ringmail
                new BlacksmithCollectionType(typeof(RingmailGloves), "Ringmail Gloves", 12.0, 62.0, typeof(IronIngot), 10),
                new BlacksmithCollectionType(typeof(RingmailLegs), "Ringmail Legs", 19.4, 69.4, typeof(IronIngot), 16),
                new BlacksmithCollectionType(typeof(RingmailArms), "Ringmail Arms", 16.9, 66.9, typeof(IronIngot), 14),
                new BlacksmithCollectionType(typeof(RingmailChest), "Ringmail Chest", 21.9, 71.9, typeof(IronIngot), 18),

                // Chainmail
                new BlacksmithCollectionType(typeof(ChainCoif), "Chain Coif", 14.5, 64.5, typeof(IronIngot), 10),
                new BlacksmithCollectionType(typeof(ChainLegs), "Chain Legs", 36.7, 86.7, typeof(IronIngot), 18),
                new BlacksmithCollectionType(typeof(ChainChest), "Chain Chest", 39.1, 89.1, typeof(IronIngot), 20),

                // Platemail
                new BlacksmithCollectionType(typeof(PlateArms), "Plate Arms", 66.3, 116.3, typeof(IronIngot), 18),
                new BlacksmithCollectionType(typeof(PlateGloves), "Plate Gloves", 58.9, 108.9, typeof(IronIngot), 12),
                new BlacksmithCollectionType(typeof(PlateGorget), "Plate Gorget", 56.4, 106.4, typeof(IronIngot), 10),
                new BlacksmithCollectionType(typeof(PlateLegs), "Plate Legs", 68.8, 118.8, typeof(IronIngot), 20),
                new BlacksmithCollectionType(typeof(PlateChest), "Plate Chest", 75.0, 125.0, typeof(IronIngot), 25),
                new BlacksmithCollectionType(typeof(FemalePlateChest), "Female Plate Chest", 44.1, 94.1, typeof(IronIngot), 20),
                new BlacksmithCollectionType(typeof(DragonBardingDeed), "Dragon Barding Deed", 72.5, 122.5, typeof(IronIngot), 750),

                // Helmets
                new BlacksmithCollectionType(typeof(Bascinet), "Bascinet", 8.3, 58.3, typeof(IronIngot), 15),
                new BlacksmithCollectionType(typeof(CloseHelm), "Close Helm", 37.9, 87.9, typeof(IronIngot), 15),
                new BlacksmithCollectionType(typeof(Helmet), "Helmet", 37.9, 87.9, typeof(IronIngot), 15),
                new BlacksmithCollectionType(typeof(NorseHelm), "Norse Helm", 37.9, 87.9, typeof(IronIngot), 15),
                new BlacksmithCollectionType(typeof(PlateHelm), "Plate Helm", 62.6, 112.6, typeof(IronIngot), 15),

                // Shields
                new BlacksmithCollectionType(typeof(Buckler), "Buckler", -25.0, 25.0, typeof(IronIngot), 10),
                new BlacksmithCollectionType(typeof(BronzeShield), "Bronze Shield", -15.2, 34.8, typeof(IronIngot), 12),
                new BlacksmithCollectionType(typeof(HeaterShield), "Heater Shield", 24.3, 74.3, typeof(IronIngot), 18),
                new BlacksmithCollectionType(typeof(MetalShield), "Metal Shield", -10.2, 39.8, typeof(IronIngot), 14),
                new BlacksmithCollectionType(typeof(MetalKiteShield), "Metal Kite Shield", 4.6, 54.6, typeof(IronIngot), 16),

                // Bladed Weapons
                new BlacksmithCollectionType(typeof(Broadsword), "Broadsword", 35.4, 85.4, typeof(IronIngot), 10),
                new BlacksmithCollectionType(typeof(Cutlass), "Cutlass", 24.3, 74.3, typeof(IronIngot), 8),
                new BlacksmithCollectionType(typeof(Dagger), "Dagger", -0.4, 49.6, typeof(IronIngot), 3),
                new BlacksmithCollectionType(typeof(Katana), "Katana", 44.1, 94.1, typeof(IronIngot), 8),
                new BlacksmithCollectionType(typeof(Kryss), "Kryss", 36.7, 86.7, typeof(IronIngot), 8),

                // Axes
                new BlacksmithCollectionType(typeof(Axe), "Axe", 34.2, 84.2, typeof(IronIngot), 14),
                new BlacksmithCollectionType(typeof(BattleAxe), "Battle Axe", 30.5, 80.5, typeof(IronIngot), 14),
                new BlacksmithCollectionType(typeof(DoubleAxe), "Double Axe", 29.3, 79.3, typeof(IronIngot), 12),
                new BlacksmithCollectionType(typeof(ExecutionersAxe), "Executioner's Axe", 34.2, 84.2, typeof(IronIngot), 14),
                new BlacksmithCollectionType(typeof(LargeBattleAxe), "Large Battle Axe", 28.0, 78.0, typeof(IronIngot), 12),

                // Pole Arms
                new BlacksmithCollectionType(typeof(Bardiche), "Bardiche", 31.7, 81.7, typeof(IronIngot), 18),
                new BlacksmithCollectionType(typeof(Halberd), "Halberd", 39.1, 89.1, typeof(IronIngot), 20),
                new BlacksmithCollectionType(typeof(Spear), "Spear", 49.0, 99.0, typeof(IronIngot), 12),

                // Bashing Weapons
                new BlacksmithCollectionType(typeof(HammerPick), "Hammer Pick", 34.2, 84.2, typeof(IronIngot), 16),
                new BlacksmithCollectionType(typeof(Mace), "Mace", 14.5, 64.5, typeof(IronIngot), 6),
                new BlacksmithCollectionType(typeof(Maul), "Maul", 19.4, 69.4, typeof(IronIngot), 10),
                new BlacksmithCollectionType(typeof(WarMace), "War Mace", 28.0, 78.0, typeof(IronIngot), 14),
                new BlacksmithCollectionType(typeof(WarHammer), "War Hammer", 34.2, 84.2, typeof(IronIngot), 16),
            };
        }

        public static BlacksmithCollectionType GetRandomItem()
        {
            return Items[Utility.Random(Items.Count)];
        }
    }
}
