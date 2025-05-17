using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HornedAnomalyQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Horned Anomaly"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands **Nadira Bonecarver**, Castle British’s Paleomancer—her eyes hollow, yet burning with a cold resolve.\n\n" +
                    "Fragments of bone and fossil dust cling to her robes, and a pale shard hums faintly in her hand.\n\n" +
                    "“The Vault. My exhibits. My work—it’s been shattered by that *thing*. A beast of bone and madness, trampling through **Preservation Vault 44**, breaking centuries in seconds.”\n\n" +
                    "“It’s not just a beast... it’s an echo of something older. It leaves tremors, momentum... my pale tables still *shake*.”\n\n" +
                    "“I need you to slay the **VaultCeratops**, before it turns relic into rubble, and memory into dust.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the echoes will worsen, and soon all that remains will crumble to powder beneath its fury.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still tramples? My bone trinkets crack from its passage. End it, before all sense is lost to motion.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The echoes still... *no*. They fade.\n\n" +
                       "You've quelled the beast, and stilled the tremors. My tables rest, and so, for a time, do I.\n\n" +
                       "Take this, forged of **Steelbloom** and fossil thought—it carries the strength to **resist what moves unseen.**";
            }
        }

        public HornedAnomalyQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(VaultCeratops3), "VaultCeratops", 1));
            AddReward(new BaseReward(typeof(SteelbloomCrest), 1, "SteelbloomCrest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Horned Anomaly'!");
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

    public class NadiraBonecarver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HornedAnomalyQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic()); // Paleomancer with mystical knowledge of bones and relics
        }

        [Constructable]
        public NadiraBonecarver()
            : base("the Paleomancer", "Nadira Bonecarver")
        {
        }

        public NadiraBonecarver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 90);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale-skinned, reflecting her bonecraft focus
            HairItemID = 0x2049; // Long hair
            HairHue = 1150; // White/Silver
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 2301, Name = "Bone-Dusted Robe" }); // Aged bone-white
            AddItem(new LeatherGloves() { Hue = 2306, Name = "Shard-Binder Gloves" });
            AddItem(new WizardsHat() { Hue = 2301, Name = "Hood of Residual Whispers" });
            AddItem(new Sandals() { Hue = 2301, Name = "Steps of Silence" });
            AddItem(new BoneHarvester() { Hue = 2306, Name = "Pale Shard Blade" });

            Backpack backpack = new Backpack();
            backpack.Hue = 2301;
            backpack.Name = "Trinket Pouch";
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
