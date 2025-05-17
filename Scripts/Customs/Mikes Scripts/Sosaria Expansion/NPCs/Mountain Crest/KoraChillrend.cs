using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RaggedRevenantQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Ragged Revenant"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Kora Chillrend*, a healer with frostbitten hands wrapped in silken bandages.\n\n" +
                    "She shivers despite the warmth of her hearth, her eyes cold as the peaks above.\n\n" +
                    "“I’ve mended wounds from beasts and blades alike, but nothing for those it touches... the **Frostbound Bandage**. Travelers limp home chilled to the bone, their injuries... alive, feeding on their pain.”\n\n" +
                    "“They say it’s just rags and ice. I say it’s cursed. My salves are useless against it. Each time I try, it only spreads—tearing flesh anew.”\n\n" +
                    "“Please. End it. **Slay the Frostbound Bandage** before it devours more lives.”\n\n" +
                    "**Beware sudden gusts**—the monster rides the wind.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I pray your path never crosses its icy trail. My hands... I’ll do what I can, for those it hasn’t touched.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it prowls the Cavern? I've heard the winds wail... and another soul was lost last night.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve slain it? I can feel it—the air's lighter, the cold... less cruel.\n\n" +
                       "Take this: **Telvanni Magister’s Cap**. A gift from a traveler I once healed. May it protect your mind as you’ve shielded my people.";
            }
        }

        public RaggedRevenantQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FrostboundBandage), "Frostbound Bandage", 1));
            AddReward(new BaseReward(typeof(TelvanniMagistersCap), 1, "Telvanni Magister’s Cap"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Ragged Revenant'!");
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

    public class KoraChillrend : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RaggedRevenantQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner());
        }

        [Constructable]
        public KoraChillrend()
            : base("the Bandage Maker", "Kora Chillrend")
        {
        }

        public KoraChillrend(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale, frost-touched skin
            HairItemID = 0x203C; // Long Hair
            HairHue = 1152; // Icy blue
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1153, Name = "Chillrend’s Robe" }); // Frost-blue
            AddItem(new Cloak() { Hue = 2101, Name = "Windveil Cloak" }); // Pale silver
            AddItem(new LeatherGloves() { Hue = 1157, Name = "Healer’s Wrappings" });
            AddItem(new FurBoots() { Hue = 1152, Name = "Snowstep Boots" });
            AddItem(new FeatheredHat() { Hue = 1151, Name = "Frostfeather Cap" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Healer’s Satchel";
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
