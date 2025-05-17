using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class VaporsOfSorrowQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Vapors of Sorrow"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Nymora Tideweaver*, Seer of the Salted Depths, her eyes veiled in mist.\n\n" +
                    "“The stones... I cannot see. The visions, clouded. The Vapors drift again.”\n\n" +
                    "She lifts a pouch of Devil Guard salt, sprinkling it in spirals on the ground.\n\n" +
                    "“These mists, the *MournfulVapors*, seep from the old halls of Exodus. Each time they rise, the future dims. I need clear sight, or all we know may twist into shadow.”\n\n" +
                    "“Slay them. Scatter their sorrow. Let the vision stones shine once more.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the mists claim us both. But I will keep warding, until my sight fails completely.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Vapors still linger. My stones remain silent, my dreams—lost in haze.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The air clears... yes, I feel it. The threads of fate realign. The mists recoil.\n\n" +
                       "You’ve given me back the stars, traveler. And I gift you this, drawn from beyond sight—a vault of arcade wonders, *ArcadeMastersVault*.";
            }
        }

        public VaporsOfSorrowQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MournfulVapors), "Mournful Vapors", 1));
            AddReward(new BaseReward(typeof(ArcadeMastersVault), 1, "ArcadeMastersVault"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Vapors of Sorrow'!");
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

    public class NymoraTideweaver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(VaporsOfSorrowQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSATailor());
        }

        [Constructable]
        public NymoraTideweaver()
            : base("the Salt-Seer", "Nymora Tideweaver")
        {
        }

        public NymoraTideweaver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 90);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale Seafoam skin hue
            HairItemID = 0x203C; // Long hair
            HairHue = 1153; // Deep sea blue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1260, Name = "Tidewoven Robe" }); // Dark teal
            AddItem(new Sandals() { Hue = 1153, Name = "Salt-worn Sandals" });
            AddItem(new WizardsHat() { Hue = 1260, Name = "Mistcaller's Hat" });
            AddItem(new Cloak() { Hue = 1271, Name = "Veil of Vapors" }); // Light mist-gray
            AddItem(new BodySash() { Hue = 1266, Name = "Sash of Foresight" }); // Stormy blue-gray

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Salt-Seer's Satchel";
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
