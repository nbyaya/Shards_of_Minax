using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GoblinGulagQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Goblin Gulag"; } }

        public override object Description
        {
            get
            {
                return
                    "Morren Rockshield, Guard Captain of Devil Guard, stands with his battered shield resting against a stone pillar, each dent and notch telling a tale of lost comrades.\n\n" +
                    "His voice is low, grim: “The goblins are back. Filthy wretches, crawling out of the east galleries of the Minax mines like a plague. Their leader’s clever—too clever. Ambushes my patrols, steals our ore, leaves my men bleeding in the dark.”\n\n" +
                    "“I mark the names of every fallen on this shield. And I swear I’ll add one more—**that goblin’s name**—before I’m done.”\n\n" +
                    "“Get in there. Slay their leader. Break their lines. Do this, and you’ll walk with the shadows that guard us.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then stay clear of the galleries. My men will handle it... or die trying.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still breathing, is it? Then the blood won’t stop flowing, not until you put an end to it.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The goblin’s dead? Then one more name rests here—scratched in steel, honored in blood.\n\n" +
                       "**You’ve done us proud. Take this. The shadows guard their own.**";
            }
        }

        public GoblinGulagQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CavernGoblin), "CavernGoblin Leader", 1));
            AddReward(new BaseReward(typeof(WanderersShadowhood), 1, "Wanderer's Shadowhood"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Goblin Gulag'!");
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

    public class MorrenRockshield : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(GoblinGulagQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMaceWeapon());
        }

        [Constructable]
        public MorrenRockshield()
            : base("the Guard Captain", "Morren Rockshield")
        {
        }

        public MorrenRockshield(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Weathered skin tone
            HairItemID = 0x203C; // Long hair
            HairHue = 1109; // Dark gray
            FacialHairItemID = 0x2041; // Full beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2425, Name = "Rockshield's Guardplate" }); // Slate-gray armor
            AddItem(new StuddedLegs() { Hue = 2418, Name = "Cavern-Walker Greaves" }); // Dull steel
            AddItem(new StuddedArms() { Hue = 2301, Name = "Ore-Kissed Bracers" });
            AddItem(new StuddedGloves() { Hue = 1150, Name = "Shadow-Touched Gauntlets" });
            AddItem(new NorseHelm() { Hue = 2402, Name = "Morren's Oathhelm" }); // Aged iron
            AddItem(new Cloak() { Hue = 1109, Name = "Mantle of the Fallen Watch" });
            AddItem(new Boots() { Hue = 1102, Name = "Stonebound Boots" });

            AddItem(new BashingShield() { Hue = 2420, Name = "Shield of Names" }); // Shield with engraved names
            AddItem(new WarMace() { Hue = 2405, Name = "Ore-Breaker" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Guard Captain's Pack";
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
