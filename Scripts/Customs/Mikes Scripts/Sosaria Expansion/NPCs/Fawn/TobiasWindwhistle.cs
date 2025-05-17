using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class VenomsVeilQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Venom's Veil"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Tobias Windwhistle*, Fawn’s resident flutist, standing by the garden’s edge, his breath shallow, eyes watering.\n\n" +
                    "“The winds, they’re *poisoned*,” he croaks, clutching a slender reed flute to his lips. “It’s that vile **Shavarak**—a beast whose stingers foul the air itself!”\n\n" +
                    "“My melodies… they used to bloom the flowers. Now they **wilt**. Even the trees recoil when I play.”\n\n" +
                    "“If this venomous curse spreads, Fawn will become silent. Lifeless. You must go, find this **Shavarak**, and end its blight.”\n\n" +
                    "“Slay it, so the winds may carry songs again.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then Fawn's music fades, and we are left in silence. I pray the winds do not choke us all.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still the air carries venom? Each note I play dies quicker than the last...";
            }
        }

        public override object Complete
        {
            get
            {
                return "The air clears... I can breathe again.\n\n" +
                       "**Thank you, brave soul**. Fawn owes you its voice. Take this: a token born of wings and breath—*WingedHusChest*. Let it shield you as you’ve shielded us.";
            }
        }

        public VenomsVeilQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Shavarak), "Shavarak", 1));
            AddReward(new BaseReward(typeof(WingedHusChest), 1, "WingedHusChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Venom's Veil'!");
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

    public class TobiasWindwhistle : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(VenomsVeilQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard());
        }

        [Constructable]
        public TobiasWindwhistle()
            : base("the Melancholic Flutist", "Tobias Windwhistle")
        {
        }

        public TobiasWindwhistle(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2044; // Short hair
            HairHue = 1150; // Pale silver
            FacialHairItemID = 0x203C; // Short beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1367, Name = "Lamented Sky-Tunic" }); // Pale blue
            AddItem(new LongPants() { Hue = 1153, Name = "Windwoven Trousers" }); // Light teal
            AddItem(new HalfApron() { Hue = 1345, Name = "Melody-Keeper's Sash" }); // Soft lavender
            AddItem(new Sandals() { Hue = 2101, Name = "Reedwalker’s Sandals" }); // Soft green
            AddItem(new FeatheredHat() { Hue = 1157, Name = "Breezebound Cap" }); // Muted violet

            AddItem(new GnarledStaff() { Hue = 0, Name = "Flutewood Cane" }); // His flute doubles as a staff.

            Backpack backpack = new Backpack();
            backpack.Hue = 1175; // Light sky blue
            backpack.Name = "Flutist's Pouch";
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
