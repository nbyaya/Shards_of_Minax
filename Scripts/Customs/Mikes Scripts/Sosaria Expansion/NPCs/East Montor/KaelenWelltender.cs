using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WarriorsWrathQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Warrior's Wrath"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Kaelen Welltender*, the Waterwright of East Montor.\n\n" +
                    "Draped in robes the color of deep springs, he carries a twisted staff carved with flowing wave patterns.\n\n" +
                    "“Have you heard the silence in our wells?” he asks, voice strained but resolute.\n\n" +
                    "“The **Caves of Drakkon**—once they roared with water, fed our lands, blessed our people. Now... dry as bone.”\n\n" +
                    "“A creature—a **DrakeWarrior**—lurks near the spring’s heart. My pumps fail when it draws near. Legend says it wields a spear forged in dragonfire, and with each day it stands guard, the source withers further.”\n\n" +
                    "“I’ve sent miners, hunters—none return. I’m no fighter. But you—you carry strength, and the will to act. **Slay the DrakeWarrior**. Let our waters flow once more.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then let us wither together, and watch East Montor fall to dust.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it stands? Then our springs die, and with them, our hope.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The beast lies slain? The waters stir again?\n\n" +
                       "**You’ve saved East Montor.** The land breathes. Take this—*CovenTreasuresChest*. Within, what we once withheld for darker times. You’ve earned it, and more.";
            }
        }

        public WarriorsWrathQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DrakeWarrior), "DrakeWarrior", 1));
            AddReward(new BaseReward(typeof(CovenTreasuresChest), 1, "CovenTreasuresChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Warrior's Wrath'!");
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

    public class KaelenWelltender : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WarriorsWrathQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBArchitect()); // Reflects his role managing waterworks and structures
        }

        [Constructable]
        public KaelenWelltender()
            : base("the Waterwright", "Kaelen Welltender")
        {
        }

        public KaelenWelltender(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 70, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1002; // Pale skin tone
            HairItemID = 0x2047; // Long Hair
            HairHue = 1153; // Deep aquatic blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1150, Name = "Springweave Tunic" }); // Deep teal
            AddItem(new ElvenPants() { Hue = 1266, Name = "Welltender's Trousers" }); // Riverstone gray
            AddItem(new Cloak() { Hue = 1260, Name = "Flowcloak of Montor" }); // Midnight blue with wave patterns
            AddItem(new Sandals() { Hue = 1147, Name = "Waterkeeper's Footwraps" }); // Pale blue-green
            AddItem(new BodySash() { Hue = 1271, Name = "Cascade Sash" }); // Flowing silver-blue

            AddItem(new GnarledStaff() { Hue = 2052, Name = "Streambinder Staff" }); // Twisted wood, aquamarine hues

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Welltender's Pack";
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
