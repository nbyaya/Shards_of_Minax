using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EyesOfTheImperiumQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Eyes of the Imperium"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Garrick Oathkeeper*, a Templar of Castle British known for his unwavering vigilance.\n\n" +
                    "His armor gleams with holy symbols, but his eyes are fixed beyond the horizon.\n\n" +
                    "“A shadow moves within the ruins of **Preservation Vault 44**. I have seen its like before, in the legends of the **Planar Imperium**.”\n\n" +
                    "“A sentinel—**ImperiumWatcher**—once designed to root out heresy and spy upon those who would defy order. Its gaze... cold, unwavering. It glows red when it finds threat.”\n\n" +
                    "“I recognized it from the scrolls. And now, it watches again, prowling the Vault's corridors. I fear it seeks to uncover cultist intrusions—and worse, awaken old Imperium programs.”\n\n" +
                    "“We cannot let such a relic remain active. **Slay the ImperiumWatcher**, before it calls others from beyond.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the eyes shall remain open... and the Vault's secrets may yet undo us.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it watches? The Vault grows restless. I can feel it... like something behind a mirror.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The red eye fades? Good. Then the Vault will sleep... for now.\n\n" +
                       "Take this: *AshVialMarkI*. A relic from the Imperium, inert yet valuable. May it serve you better than that sentinel ever served its creators.";
            }
        }

        public EyesOfTheImperiumQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ImperiumWatcher), "ImperiumWatcher", 1));
            AddReward(new BaseReward(typeof(AshVialMarkI), 1, "AshVialMarkI"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Eyes of the Imperium'!");
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

    public class GarrickOathkeeper : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(EyesOfTheImperiumQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBKeeperOfChivalry());
        }

        [Constructable]
        public GarrickOathkeeper()
            : base("the Templar of Castle British", "Garrick Oathkeeper")
        {
        }

        public GarrickOathkeeper(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 80);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Cold steel
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 2407, Name = "Templar's Aegis" }); // Storm-silver
            AddItem(new PlateArms() { Hue = 2407, Name = "Vowbound Pauldrons" });
            AddItem(new PlateGloves() { Hue = 2407, Name = "Oathkeeper's Gauntlets" });
            AddItem(new PlateLegs() { Hue = 2407, Name = "Honorbound Greaves" });
            AddItem(new WingedHelm() { Hue = 2407, Name = "Helm of the Vigilant Eye" });
            AddItem(new Cloak() { Hue = 1153, Name = "Imperium Watch-Cloak" }); // Deep navy
            AddItem(new BodySash() { Hue = 1175, Name = "Sash of the Planar Guard" }); // Arcane purple

            AddItem(new Longsword() { Hue = 1154, Name = "Blade of Oaths" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Templar's Pack";
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
