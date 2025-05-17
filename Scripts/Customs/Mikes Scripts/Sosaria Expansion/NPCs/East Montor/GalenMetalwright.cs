using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FeatheredFuryQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Feathered Fury"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Galen Metalwright*, the renowned armorer of East Montor, hammering furiously at a dented breastplate.\n\n" +
                    "His forge burns hot, but his eyes burn hotter—filled with frustration and fatigue.\n\n" +
                    "“Ever hear a parrot mimic a dragon’s roar? I have. Every night this past week.”\n\n" +
                    "“It’s no ordinary bird, mind you. **The Draconian Parrot**, it’s called. Little beast with scales like polished steel and a beak sharp enough to clip your fingers clean.”\n\n" +
                    "“It’s been sneaking into my forge, nicking my best tools—tongs, hammers, chisels! Can't finish a cuirass without 'em.”\n\n" +
                    "“I’ve tracked it back to the **Caves of Drakkon**, where it nests amidst the bones of the Firstborn. I’d go myself, but my hands are for crafting, not killing.”\n\n" +
                    "“Bring me its head, and I’ll see you properly clad.”\n\n" +
                    "**Hunt down the Draconian Parrot** and end its feathered fury.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I suppose I’ll keep losing tools and temper both. But know this—if it starts stealing swords instead, East Montor will feel the loss.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still alive, is it? My forge is silent tonight. Can’t shape steel with empty hands.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You did it? The forge will sing once more!\n\n" +
                       "Here—**AvatarsVestments**. Wrought for a champion, and now yours. May your enemies fear you as much as that blasted bird feared silence.";
            }
        }

        public FeatheredFuryQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DraconianParrot), "Draconian Parrot", 1));
            AddReward(new BaseReward(typeof(AvatarsVestments), 1, "AvatarsVestments"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Feathered Fury'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class GalenMetalwright : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FeatheredFuryQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWeaponSmith());
        }

        [Constructable]
        public GalenMetalwright()
            : base("the Armorer", "Galen Metalwright")
        {
        }

        public GalenMetalwright(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 95, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Weathered bronze tone
            HairItemID = 0x2049; // Short hair
            HairHue = 1154; // Soot-black
            FacialHairItemID = 0x203B; // Thick beard
            FacialHairHue = 1154;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 2213, Name = "Forge-Kissed Cuirass" }); // Burnished steel
            AddItem(new StuddedLegs() { Hue = 2418, Name = "Tongsman's Greaves" }); // Ash-grey
            AddItem(new PlateGloves() { Hue = 2306, Name = "Anvil-Grip Gauntlets" });
            AddItem(new FullApron() { Hue = 2401, Name = "Metalwright's Apron" });
            AddItem(new PlateHelm() { Hue = 2208, Name = "Hammerhead Helm" });
            AddItem(new Boots() { Hue = 1819, Name = "Charred Boots" });

            AddItem(new SmithSmasher() { Hue = 2507, Name = "Galen's Forgehammer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 2101;
            backpack.Name = "Toolmaster’s Pack";
            AddItem(backpack);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
