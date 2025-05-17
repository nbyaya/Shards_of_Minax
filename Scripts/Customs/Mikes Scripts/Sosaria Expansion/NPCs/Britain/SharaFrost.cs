using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FrostedMenaceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Frosted Menace"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Shara Frost*, the herbalist of Castle British. Her breath mists in the air despite the warmth of her greenhouse.\n\n" +
                    "\"You feel that? The chill creeping through the stones? It's not the season—it's the **CryoToad**.\"\n\n" +
                    "\"The reagent pools in Preservation Vault 44 are vital for our tinctures, but now... they're freezing. My latest salves nearly turned to ice!\"\n\n" +
                    "\"The beast must be slain before its freezing breath shatters the alchemical flasks. I’ve prepared warming salves to shield you from its frostbite venom—don’t delay. **Slay the CryoToad** and restore balance to our craft.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "The cold won’t wait. I hope you reconsider before the vault becomes a frozen ruin.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Is the toad still alive? The frost spreads, and I fear soon even my breath will freeze in my lungs.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The vault warms again! You’ve saved more than reagents—you’ve preserved our lifework.\n\n" +
                       "Take this: *StarlitDisciplineGarb*. May its resilience guide you as surely as you’ve guided us back from winter’s brink.";
            }
        }

        public FrostedMenaceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CryoToad), "CryoToad", 1));
            AddReward(new BaseReward(typeof(StarlitDisciplineGarb), 1, "StarlitDisciplineGarb"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Frosted Menace'!");
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

    public class SharaFrost : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FrostedMenaceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCook());
        }

        [Constructable]
        public SharaFrost()
            : base("the Frost Herbalist", "Shara Frost")
        {
        }

        public SharaFrost(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 100);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale, frostbitten hue
            HairItemID = 0x203C; // Long Hair
            HairHue = 1153; // Icy white-blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1154, Name = "Frostweave Gown" }); // Pale icy blue
            AddItem(new Cloak() { Hue = 1153, Name = "Glacial Cloak" }); // Shimmering frost hue
            AddItem(new Sandals() { Hue = 2101, Name = "Chillstep Sandals" }); // Snowy white
            AddItem(new SkullCap() { Hue = 1150, Name = "Icesilk Cap" }); // Cool grey-blue
            AddItem(new BodySash() { Hue = 1152, Name = "Herbalist's Sash" }); // Frost green

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Frosted Herb Satchel";
            AddItem(backpack);

            AddItem(new GnarledStaff() { Hue = 1153, Name = "Staff of the Winter Bloom" });
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
