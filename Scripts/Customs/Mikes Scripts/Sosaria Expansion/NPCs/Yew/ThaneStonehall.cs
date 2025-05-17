using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HornsOfCorruptionQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Horns of Corruption"; } }

        public override object Description
        {
            get
            {
                return
                    "Thane Stonehall, the stoic blacksmith of Yew, turns from his forge with a grim expression.\n\n" +
                    "**“The horns of the Corrupted Minotaur Captain... they’re cursed, tainted—but I need them.”**\n\n" +
                    "**“I’ll forge them anew, cleanse them with fire and steel, make them a symbol for Yew’s strength—not fear.”**\n\n" +
                    "**“Bring me the horns. Let’s end this corruption.”**";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then let Yew stand alone, without the strength it deserves.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Captain lives? Then our dead still cry for justice.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The horns... they feel heavy, but not with sorrow—with *power*.\n\n" +
                       "I’ll forge them anew, and Yew will rise stronger. Take this—*WispweftRaiment*. May it remind you of the light that follows darkness.";
            }
        }

        public HornsOfCorruptionQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CorruptedMinotaurCaptain), "Corrupted Minotaur Captain", 1));
            AddReward(new BaseReward(typeof(WispweftRaiment), 1, "WispweftRaiment"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Horns of Corruption'!");
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

    public class ThaneStonehall : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HornsOfCorruptionQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith()); 
        }

        [Constructable]
        public ThaneStonehall()
            : base("the Blacksmith", "Thane Stonehall")
        {
        }

        public ThaneStonehall(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(95, 100, 45);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1108; // Charcoal black
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1108;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2101, Name = "Fire-Hardened Cuirass" }); // Deep bronze
            AddItem(new StuddedLegs() { Hue = 2105, Name = "Forge-Bound Greaves" });
            AddItem(new StuddedGloves() { Hue = 2107, Name = "Hammergrip Gloves" });
            AddItem(new HalfApron() { Hue = 1151, Name = "Ember-Stained Apron" }); // Smoky red
            AddItem(new LeatherCap() { Hue = 2406, Name = "Ironbrow Helm" });
            AddItem(new Boots() { Hue = 1819, Name = "Anvil-Tread Boots" });

            AddItem(new SmithSmasher() { Hue = 2105, Name = "Stonehall's Hammer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Blacksmith's Satchel";
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
