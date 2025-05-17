using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HeadOfTheClassQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Head of the Class"; } }

        public override object Description
        {
            get
            {
                return
                    "*Professor Ithran Vex*, once a promising mind at Malidor, now walks the ashen streets of Death Glutch, his eyes heavy with dread.\n\n" +
                    "“The Dean was once my mentor… until he chose to bind knowledge with darkness.”\n\n" +
                    "“His lectures warp minds now. His classroom is a trap of illusions and pain. I escaped, barely, cast out for questioning his path. But the Academy... it *calls*. And students still vanish into its halls.”\n\n" +
                    "**Find the Dean. End his cursed teachings**. If his sanctum is sealed, these notes may open the way. He taught me well… now it’s time I taught him something in return.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the Academy’s shadows will only grow deeper. The Dean will not stop, and soon, Death Glutch itself may feel his lessons.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Dean lives? His words twist the winds—I hear them even here. You must stop him before all is lost.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So… the Dean’s voice is silenced. You’ve done more than kill a man—you’ve broken a chain of corruption centuries long.\n\n" +
                       "This was meant for my final lecture. *Take it*. May it serve you better than it would have served him.";
            }
        }

        public HeadOfTheClassQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DeanOfMalidor), "Corrupted Dean of Malidor", 1));
            AddReward(new BaseReward(typeof(WeaponBottle), 1, "WeaponBottle"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Head of the Class'!");
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

    public class ProfessorIthranVex : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HeadOfTheClassQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage());
        }

        [Constructable]
        public ProfessorIthranVex()
            : base("the Arcane Outcast", "Professor Ithran Vex")
        {
        }

        public ProfessorIthranVex(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 30);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1107; // Dark ash
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1107;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1154, Name = "Shroud of Severed Wisdom" }); // Midnight blue
            AddItem(new WizardsHat() { Hue = 1175, Name = "Hat of Forbidden Thought" }); // Pale gray
            AddItem(new Sandals() { Hue = 1109, Name = "Ashen Walkers" }); // Charcoal
            AddItem(new BodySash() { Hue = 1161, Name = "Sash of Lost Students" }); // Deep violet
            AddItem(new Spellbook() { Hue = 1150, Name = "Expelled Grimoire" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Satchel of Notes";
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
