using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class InscribeCollectionType
    {
        public static List<InscribeCollectionType> Items = new List<InscribeCollectionType>();

        public Type ScrollType { get; private set; }
        public string Name { get; private set; }
        public double MinSkill { get; private set; }
        public double MaxSkill { get; private set; }
        public int ManaCost { get; private set; }

        public InscribeCollectionType(Type scrollType, string name, double minSkill, double maxSkill, int manaCost)
        {
            ScrollType = scrollType;
            Name = name;
            MinSkill = minSkill;
            MaxSkill = maxSkill;
            ManaCost = manaCost;
        }

        public static void PopulateInscribeCollection()
        {
            Items = new List<InscribeCollectionType>();

            // Circle 0
            AddCircleItems(0, 4, -25.0, 25.0, new[]
            {
                typeof(ReactiveArmorScroll),
                typeof(ClumsyScroll),
                typeof(CreateFoodScroll),
                typeof(FeeblemindScroll),
                typeof(HealScroll),
                typeof(MagicArrowScroll),
                typeof(NightSightScroll),
                typeof(WeakenScroll),
            });

            // Circle 1
            AddCircleItems(1, 6, -10.8, 39.2, new[]
            {
                typeof(AgilityScroll),
                typeof(CunningScroll),
                typeof(CureScroll),
                typeof(HarmScroll),
                typeof(MagicTrapScroll),
                typeof(MagicUnTrapScroll),
                typeof(ProtectionScroll),
                typeof(StrengthScroll),
            });

            // Circle 2
            AddCircleItems(2, 9, 3.5, 53.5, new[]
            {
                typeof(BlessScroll),
                typeof(FireballScroll),
                typeof(MagicLockScroll),
                typeof(PoisonScroll),
                typeof(TelekinisisScroll),
                typeof(TeleportScroll),
                typeof(UnlockScroll),
                typeof(WallOfStoneScroll),
            });

            // Circle 3
            AddCircleItems(3, 11, 17.8, 67.8, new[]
            {
                typeof(ArchCureScroll),
                typeof(ArchProtectionScroll),
                typeof(CurseScroll),
                typeof(FireFieldScroll),
                typeof(GreaterHealScroll),
                typeof(LightningScroll),
                typeof(ManaDrainScroll),
                typeof(RecallScroll),
            });

            // Circle 4
            AddCircleItems(4, 14, 32.1, 82.1, new[]
            {
                typeof(BladeSpiritsScroll),
                typeof(DispelFieldScroll),
                typeof(IncognitoScroll),
                typeof(MagicReflectScroll),
                typeof(MindBlastScroll),
                typeof(ParalyzeScroll),
                typeof(PoisonFieldScroll),
                typeof(SummonCreatureScroll),
            });

            // Circle 5
            AddCircleItems(5, 20, 46.4, 96.4, new[]
            {
                typeof(DispelScroll),
                typeof(EnergyBoltScroll),
                typeof(ExplosionScroll),
                typeof(InvisibilityScroll),
                typeof(MarkScroll),
                typeof(MassCurseScroll),
                typeof(ParalyzeFieldScroll),
                typeof(RevealScroll),
            });

            // Circle 6
            AddCircleItems(6, 40, 60.7, 110.7, new[]
            {
                typeof(ChainLightningScroll),
                typeof(EnergyFieldScroll),
                typeof(FlamestrikeScroll),
                typeof(GateTravelScroll),
                typeof(ManaVampireScroll),
                typeof(MassDispelScroll),
                typeof(MeteorSwarmScroll),
                typeof(PolymorphScroll),
            });

            // Circle 7
            AddCircleItems(7, 50, 75.0, 125.0, new[]
            {
                typeof(EarthquakeScroll),
                typeof(EnergyVortexScroll),
                typeof(ResurrectionScroll),
                typeof(SummonAirElementalScroll),
                typeof(SummonDaemonScroll),
                typeof(SummonEarthElementalScroll),
                typeof(SummonFireElementalScroll),
                typeof(SummonWaterElementalScroll),
            });
        }

        private static void AddCircleItems(int circle, int manaCost, double minSkill, double maxSkill, Type[] spells)
        {
            foreach (var spell in spells)
            {
                Items.Add(new InscribeCollectionType(spell, spell.Name, minSkill, maxSkill, manaCost));
            }
        }

        public static InscribeCollectionType GetRandomItem()
        {
            return Items[Utility.Random(Items.Count)];
        }
    }
}
