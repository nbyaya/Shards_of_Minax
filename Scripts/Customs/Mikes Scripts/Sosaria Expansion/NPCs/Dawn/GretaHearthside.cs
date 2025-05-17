using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MatronOfMaledictionQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Matron of Malediction"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Greta Hearthside*, the innkeeper of Dawn, nervously polishing a mug behind the counter.\n\n" +
                    "Her eyes dart to the floorboards beneath you.\n\n" +
                    "“I can’t ignore it any longer,” she murmurs. “Guests are vanishing. And each night, I hear chanting—low and cold—from below my tavern. I dared lift a board... and the symbols there—they twist like they breathe.”\n\n" +
                    "“They match the runes I once saw in an old tome, from the sealed catacombs of Doom Dungeon. I fear... no, I *know*—someone, or something, is conducting blood rites beneath my hearth.”\n\n" +
                    "**Slay the Cult Matriarch** who dares turn my tavern into a den of curses, and I’ll see you rewarded. For all our sakes.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then pray you sleep well, for I cannot. Nor can my guests, wherever they now are...";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still she chants? The walls hum with it. And the floor... I think it’s bleeding.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s over, then? The chanting has stopped... the symbols faded. Bless you.\n\n" +
                       "Take this—*Harvester’s Curse*. It was my grandmother’s, a relic from darker times. It’s said to bind harvest and shadow alike. May it serve you as well as you’ve served Dawn.";
            }
        }

        public MatronOfMaledictionQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CultMatriarch), "the Cult Matriarch", 1));
            AddReward(new BaseReward(typeof(HarvestersCurse), 1, "Harvester’s Curse"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Matron of Malediction'!");
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

    public class GretaHearthside : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MatronOfMaledictionQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTavernKeeper());
        }

        [Constructable]
        public GretaHearthside()
            : base("the Innkeeper", "Greta Hearthside")
        {
        }

        public GretaHearthside(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 65, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Warm auburn
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1150, Name = "Hearthspun Blouse" });
            AddItem(new Skirt() { Hue = 2411, Name = "Emberweave Skirt" });
            AddItem(new HalfApron() { Hue = 1354, Name = "Innkeeper's Apron" });
            AddItem(new Boots() { Hue = 2401, Name = "Well-Worn Boots" });
            AddItem(new Cloak() { Hue = 1823, Name = "Ashen Hearthcloak" });

            AddItem(new Pitcher() { Name = "Greta's Ale Jug", Hue = 1109 });

            Backpack backpack = new Backpack();
            backpack.Hue = 1107;
            backpack.Name = "Innkeeper’s Pouch";
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
