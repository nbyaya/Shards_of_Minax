using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GoldenHoardQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Golden Hoard"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Wyric Hearthguard*, Stablemaster of East Montor, his leather gloves still stained with ash.\n\n" +
                    "A scent of singed hay lingers, and the sorrow in his eyes burns hotter than any dragon’s flame.\n\n" +
                    "“I raised them from foals, you know. Swift as the dawn, proud as kings. Gone. All of them.”\n\n" +
                    "“It wasn’t just fire. It was rage. **Golden rage.** A dragon, scales like molten coin, eyes like suns. Took my mounts. Burned my stables. Left nothing but a blackened hoofprint at the threshold.”\n\n" +
                    "“But I’ve heard whispers… of a vault, deep in the **Caves of Drakkon**. Said to hold treasures older than the world. And this beast guards it, drawn to gold like rot to fruit.”\n\n" +
                    "**Find the GoldDragon. End it.** Not for treasure, but for the lives stolen in flame.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the beast feast again. But know this—each breath it draws is a lash across my soul.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lives? The scorched earth calls your name, friend. Don’t let my mounts’ cries go unheard.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve slain it? The flames are gone?\n\n" +
                       "*Wyric grips your hand, hard, then pulls away, eyes damp.*\n\n" +
                       "“I see them, now. Galloping free, unburned, unbound. You’ve done more than kill a beast. You’ve given me peace.”\n\n" +
                       "**Take this: ConquistadorsHoard**. May it remind you that even the greatest monsters can fall.";
            }
        }

        public GoldenHoardQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GoldDragon), "GoldDragon", 1));
            AddReward(new BaseReward(typeof(ConquistadorsHoard), 1, "ConquistadorsHoard"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Golden Hoard'!");
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

    public class WyricHearthguard : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(GoldenHoardQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        [Constructable]
        public WyricHearthguard()
            : base("the Stablemaster", "Wyric Hearthguard")
        {
        }

        public WyricHearthguard(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1055; // Weathered tan
            HairItemID = 0x2048; // Short hair
            HairHue = 1175; // Ash-blonde
            FacialHairItemID = 0x203C; // Short beard
            FacialHairHue = 1175;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2306, Name = "Emberhide Jerkin" });
            AddItem(new LeatherLegs() { Hue = 2302, Name = "Scorched Rider’s Breeches" });
            AddItem(new LeatherGloves() { Hue = 2407, Name = "Branded Handguards" });
            AddItem(new WideBrimHat() { Hue = 2419, Name = "Ashen Stable Hat" });
            AddItem(new Boots() { Hue = 1820, Name = "Trailworn Hoofboots" });

            AddItem(new ShepherdsCrook() { Hue = 1161, Name = "Wyric’s Crook of Binding" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1108;
            backpack.Name = "Stablemaster’s Satchel";
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
