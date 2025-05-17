using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HowlOfTheFrozenPack : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Howl of the Frozen Pack"; } }

        public override object Description
        {
            get
            {
                return
                    "Solvi Timbervoice, a weathered woodcutter of Mountain Crest, meets you with frost on her lashes and fear in her voice.\n\n" +
                    "\"They call it the *Glacial Timberwolf*—a beast of ice and blood. It hunts not for food, but for sport. It’s tearing through my camps. I can hear the howls at night... echoing through the trees like death itself.\"\n\n" +
                    "\"This forest is my life. My workers are afraid. If the beast isn't slain, there won’t be any camps left to return to.\"\n\n" +
                    "**Slay the Glacial Timberwolf** prowling the Ice Cavern and bring back proof of your kill. But beware—the wolves don’t hunt alone.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the trees stand silent, and the frost take what remains. But know this—you’ll hear the howls soon enough.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The beast still lives? My people won’t last much longer. Every night we hear it—closer now. Always closer.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it! The howls have ceased, and the forest breathes again.\n\n" +
                       "Here—take this helm. It's called *DarkKnightsObsidianHelm*. It won’t keep you warm, but it might keep you alive.\n\n" +
                       "Thank you... you've saved more than just trees.";
            }
        }

        public HowlOfTheFrozenPack() : base()
        {
            AddObjective(new SlayObjective(typeof(GlacialTimberwolf), "Glacial Timberwolf", 1));
            AddReward(new BaseReward(typeof(DarkKnightsObsidianHelm), 1, "DarkKnightsObsidianHelm"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Howl of the Frozen Pack'!");
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

    public class SolviTimbervoice : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HowlOfTheFrozenPack) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter());
        }

        [Constructable]
        public SolviTimbervoice()
            : base("the Woodcutter", "Solvi Timbervoice")
        {
        }

        public SolviTimbervoice(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 70);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 8251; // Long, windswept hair
            HairHue = 1153; // Icy Blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FurSarong() { Hue = 1109, Name = "Snowhide Sarong" }); // Frosted grey
            AddItem(new LeatherDo() { Hue = 2401, Name = "Timberworker's Vest" }); // Pale Oak
            AddItem(new LeatherGloves() { Hue = 2503, Name = "Frostbitten Gloves" });
            AddItem(new FurBoots() { Hue = 2101, Name = "Icewalker Boots" });
            AddItem(new BearMask() { Hue = 1150, Name = "White Bear Mask" }); // Symbol of strength

            AddItem(new DoubleAxe() { Hue = 2500, Name = "Timberfang" }); // Her custom axe

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Woodswoman's Pack";
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
