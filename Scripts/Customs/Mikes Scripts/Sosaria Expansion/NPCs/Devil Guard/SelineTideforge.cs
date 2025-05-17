using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ElementalEchoQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Elemental Echo"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Seline Tideforge*, the Spa Alchemist of Devil Guard, her brow furrowed with frustration as she stirs a viscous brew of steaming herbs.\n\n" +
                    "She sighs, setting down a crystal vial that glows faintly blue.\n\n" +
                    "“Our healing pools are failing. The waters have gone... wrong. Twisted. The **MineralWaterElemental** emerged from the depths, corrupting the springs. My brews sour, and the baths now harm rather than heal.”\n\n" +
                    "“If you can slay this elemental and bring me a shard of its core, I can cleanse the pools. Perhaps even more—I believe such a shard could heal any malady.”\n\n" +
                    "**Slay the MineralWaterElemental** in the Mines of Minax and restore balance to Devil Guard’s springs.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the waters remain tainted, and we must endure the sickness it spreads.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Elemental still corrupts the waters. I feel it in the steam... heavy, poisonous. We need that shard.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it! The shard pulses with life—pure, radiant. With this, the springs will flow clear once more.\n\n" +
                       "Take this, the *MarinersEmbrace*. May its strength guide you through any storm.";
            }
        }

        public ElementalEchoQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MineralWaterElemental), "MineralWaterElemental", 1));
            AddReward(new BaseReward(typeof(MarinersEmbrace), 1, "MarinersEmbrace"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Elemental Echo'!");
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

    public class SelineTideforge : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ElementalEchoQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAlchemist(this));
        }

        [Constructable]
        public SelineTideforge()
            : base("the Spa Alchemist", "Seline Tideforge")
        {
        }

        public SelineTideforge(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 90);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale, spa-like hue
            HairItemID = 0x203C; // Long Hair
            HairHue = 1150; // Frosty blue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1154, Name = "Mistweave Robe" }); // Light blue
            AddItem(new Sandals() { Hue = 1150, Name = "Steamwalk Sandals" });
            AddItem(new HoodedShroudOfShadows() { Hue = 1153, Name = "Alchemist's Hood" }); // Matching pale hue
            AddItem(new BodySash() { Hue = 1260, Name = "Infusion Sash" }); // Subtle shimmer
            AddItem(new Doublet() { Hue = 2101, Name = "Tideforged Vest" }); // Deep sea blue

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Herbalist's Pack";
            AddItem(backpack);

            AddItem(new ArtificerWand() { Hue = 1175, Name = "Hydrostatic Wand" }); // Magical tool
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
