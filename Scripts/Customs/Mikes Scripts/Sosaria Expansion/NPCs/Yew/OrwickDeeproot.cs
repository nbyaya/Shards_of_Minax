using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TusksInDismayQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Tusks in Dismay"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Orwick Deeproot*, a solemn figure hunched beneath a cloak woven with fungal patterns.\n\n" +
                    "His eyes are bright, but ringed with sleepless shadows. He clutches a gnarled staff sprouting moss at its tip.\n\n" +
                    "“The forest speaks, traveler... but now it wails.”\n\n" +
                    "“From the depths of Catastrophe, a beast has surfaced. A **Decaying Walrus**, bloated and broken, its tusks corrupted by the very rot it carries.”\n\n" +
                    "“I believe those tusks, if cleansed, could serve in ancient **ice-bound rituals**—rituals that bind fungus and frost, stave off the creeping mold of the tunnels.”\n\n" +
                    "“Slay the beast, bring me its tusks. The cries in the dark may hush, and the forest may breathe once more.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "The spores will spread, with no tusks to bind them. Beware the roots—they remember neglect.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You’ve yet to silence the beast? The cries grow louder. The tunnels shiver in dread.";
            }
        }

        public override object Complete
        {
            get
            {
                return "Ah... the tusks. Heavy with sorrow, yet still salvageable.\n\n" +
                       "**You’ve done more than kill a beast—you’ve severed a limb of the decay itself.**\n\n" +
                       "Take these *LegguardsOfTheCrashingLine*. May they root you firmly when the earth trembles.";
            }
        }

        public TusksInDismayQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DecayingWalrus), "Decaying Walrus", 1));
            AddReward(new BaseReward(typeof(LegguardsOfTheCrashingLine), 1, "LegguardsOfTheCrashingLine"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Tusks in Dismay'!");
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

    public class OrwickDeeproot : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(TusksInDismayQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        [Constructable]
        public OrwickDeeproot()
            : base("the Fungal Forager", "Orwick Deeproot")
        {
        }

        public OrwickDeeproot(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 70, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1764; // Mossy green-brown
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1764;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherCap() { Hue = 1421, Name = "Spore-Woven Hood" }); // Earthy green
            AddItem(new Robe() { Hue = 1109, Name = "Mycelial Shroud" }); // Dark grey, with a hint of fungal lore
            AddItem(new LeatherGloves() { Hue = 2101, Name = "Rot-Touched Gloves" }); // Pale moldy hue
            AddItem(new Sandals() { Hue = 1108, Name = "Root-Bound Sandals" }); // Soil-black
            AddItem(new BodySash() { Hue = 1446, Name = "Forager’s Belt of Spores" }); // Fungal purple
            AddItem(new GnarledStaff() { Hue = 1157, Name = "Staff of Hollow Roots" }); // Twisted, moss-covered

            Backpack backpack = new Backpack();
            backpack.Hue = 2205; // Muted green
            backpack.Name = "Satchel of Spores";
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
