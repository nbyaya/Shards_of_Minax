using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WarriorsGraspQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Warrior of Winter’s Grasp"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Rurik Snowshield*, Captain of the Mountain Crest militia, tightening the straps of his frostbitten gauntlets.\n\n" +
                    "His eyes are steel-gray, his breath forming clouds in the cold air.\n\n" +
                    "“It’s back again. The **Frostbrand Warrior**. Took three of my best near the cavern mouth last night.”\n\n" +
                    "“I’ve seen many winters, but none with blades like this beast. Its greatsword drips with glacial ichor—it freezes flesh before the strike even lands.”\n\n" +
                    "“We’ll lose this pass if we don’t stop it. My men can’t hold the line forever. I’m offering coin and this bow—**CustersLastStandBow**—for the one who brings me its head.”\n\n" +
                    "**Slay the Frostbrand Warrior** near the Ice Cavern entrance, and return alive.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I’ll bury the next of my own. But this blood won’t stop until someone ends that cursed warrior.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Frostbrand still walks? We can’t hold out much longer—its chill reaches further each night.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... I knew there was strength in you.\n\n" +
                       "The patrols are safe, for now. That thing—its sword—was a weapon of nightmares.\n\n" +
                       "Take this bow. It’s seen its share of battles. May it serve you as it did me when I stood on colder fields.";
            }
        }

        public WarriorsGraspQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FrostbrandWarrior), "Frostbrand Warrior", 1));
            AddReward(new BaseReward(typeof(CustersLastStandBow), 1, "CustersLastStandBow"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Warrior of Winter’s Grasp'!");
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

    public class RurikSnowshield : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WarriorsGraspQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSwordWeapon()); // He’s a warrior captain, swords make sense for his trade.
        }

        [Constructable]
        public RurikSnowshield()
            : base("the Warrior Captain", "Rurik Snowshield")
        {
        }

        public RurikSnowshield(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 95, 70);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Pale, frost-touched skin
            HairItemID = 0x203B; // Long hair
            HairHue = 1150; // Icy white
            FacialHairItemID = 0x203E; // Thick beard
            FacialHairHue = 1150; // Same icy white
        }

        public override void InitOutfit()
        {
            AddItem(new PlateHelm() { Hue = 1152, Name = "Glacier-Crowned Helm" });
            AddItem(new PlateChest() { Hue = 1153, Name = "Snowshield’s Breastplate" });
            AddItem(new PlateArms() { Hue = 1153, Name = "Frostbitten Armguards" });
            AddItem(new PlateGloves() { Hue = 1152, Name = "Iceforged Gauntlets" });
            AddItem(new PlateLegs() { Hue = 1153, Name = "Stormbound Greaves" });
            AddItem(new Cloak() { Hue = 1157, Name = "Cloak of Winter’s Vigil" });
            AddItem(new Boots() { Hue = 1150, Name = "Frost-Tread Boots" });

            AddItem(new VikingSword() { Hue = 2407, Name = "Snowbreaker" });

            Backpack backpack = new Backpack();
            backpack.Hue = 0x47E;
            backpack.Name = "Rurik’s Gear Pack";
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
