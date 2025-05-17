using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HorrorsBelowQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Horrors Below"; } }

        public override object Description
        {
            get
            {
                return
                    "Horan Ironsoul stands with arms crossed, clad in battered but gleaming armor, his eyes sharp as tempered steel.\n\n" +
                    "\"You call yourself a warrior? Then prove it. My students—good lads—died screaming in the dark below. The *MineHorror* broke them like twigs. Crushed their courage with those cursed limbs.\"\n\n" +
                    "\"I won’t see their deaths go unanswered. If you want to walk the path of iron and flame, go to the **Mines of Minax** and slay the Horror. Return with its blackened heart—or don’t return at all.\"\n\n" +
                    "**Face the Horror, or remain a pretender in my eyes.**";
            }
        }

        public override object Refuse
        {
            get
            {
                return "\"No shame in fear—but no glory either. You'll never wear the arms of the Hearthguard hiding behind excuses.\"";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "\"Still breathing? Then why does the Horror still walk? You’ve seen it, haven't you? Now *be* the one to end it.\"";
            }
        }

        public override object Complete
        {
            get
            {
                return "\"You faced it. And you won. The Horror won’t claim another soul today.\"\n\n" +
                       "*Horan clasps your shoulder, his grip like stone.*\n\n" +
                       "\"Wear this, and let all who see it know you stood tall when the darkness came. **Welcome to the Hearthguard.**\"";
            }
        }

        public HorrorsBelowQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MineHorror), "MineHorror", 1));
            AddReward(new BaseReward(typeof(ArmsOfTheHearthguard), 1, "Arms of the Hearthguard"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Horrors Below'!");
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

    public class HoranIronsoul : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HorrorsBelowQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBowyer()); // Trainer of warriors, fitting for melee combat.
        }

        [Constructable]
        public HoranIronsoul()
            : base("the Hearthguard Trainer", "Horan Ironsoul")
        {
        }

        public HoranIronsoul(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1108; // Ash-black
            FacialHairItemID = 0x203F; // Long Beard
            FacialHairHue = 1108;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 1157, Name = "Ironsoul’s Breastplate" }); // Dull Steel Grey
            AddItem(new PlateLegs() { Hue = 1157, Name = "Hearthguard Greaves" });
            AddItem(new PlateArms() { Hue = 1157, Name = "Forged Vow Armguards" });
            AddItem(new PlateGloves() { Hue = 1157, Name = "Grip of the Fallen" });
            AddItem(new CloseHelm() { Hue = 1154, Name = "Hearthguard Helm" }); // Dark Blue
            AddItem(new Cloak() { Hue = 2405, Name = "Mantle of the Guard" }); // Charcoal Grey Cloak
            AddItem(new Boots() { Hue = 1812, Name = "Stonewalker Boots" });

            AddItem(new Broadsword() { Hue = 2425, Name = "Ironsoul's Edge" }); // Dark Iron-hued sword

            Backpack backpack = new Backpack();
            backpack.Hue = 2101;
            backpack.Name = "Guard Trainer's Satchel";
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
