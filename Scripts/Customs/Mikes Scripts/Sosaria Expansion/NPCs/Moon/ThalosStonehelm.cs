using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ClawsOfTheDunesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Claws of the Dunes"; } }

        public override object Description
        {
            get
            {
                return
                    "You walk under Moon’s light, traveler? Beware the sands.\n\n" +
                    "Name's Thalos Stonehelm, Bounder-for-Hire. I've walked this desert a hundred times, kept the sandwalls clear for merchant convoys. " +
                    "But there's a beast now—Desert Bear they call it. Took two good men from me last moonrise. I've sworn to see it dead.\n\n" +
                    "**Track and slay the Desert Bear** that lurks in the rock overhangs east of Moon. It only stirs when the moon touches the dunes. End this savage threat, for all our sakes.";
            }
        }

        public override object Refuse { get { return "Then stay clear of the eastern dunes, lest it makes a meal of you too."; } }

        public override object Uncomplete { get { return "The DesertBear still stalks the sands. My companions deserve justice."; } }

        public override object Complete { get { return "You’ve avenged them... and saved more lives than you’ll ever know. Take these Wings, forged for those who rise above fear."; } }

        public ClawsOfTheDunesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DesertBear), "DesertBear", 1));

            AddReward(new BaseReward(typeof(WingsOfTheSkyborne), 1, "WingsOfTheSkyborne"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Claws of the Dunes'!");
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

    public class ThalosStonehelm : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ClawsOfTheDunesQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMapmaker()); 
        }

        [Constructable]
        public ThalosStonehelm() : base("Thalos Stonehelm", "Bounder-for-Hire")
        {
            Title = "Bounder-for-Hire";
			Body = 0x190; // Male human

            // Outfit
            AddItem(new StuddedChest { Hue = 2125, Name = "Dusthide Vest" }); // Worn desert-brown leather
            AddItem(new LeatherLegs { Hue = 2403, Name = "Sunscarred Greaves" }); // Faded, sun-bleached tones
            AddItem(new PlateGorget { Hue = 2107, Name = "Ironbound Oath" }); // Old steel, engraved with protective runes
            AddItem(new Sandals { Hue = 2112, Name = "Trailworn Sandals" }); // Hardened leather, rough
            AddItem(new Bandana { Hue = 2225, Name = "Helm of the Eastern Winds" }); // Desert-blue wrap
            AddItem(new LeatherGloves { Hue = 2130, Name = "Grips of the Fallen" }); // Brown, rough-stitch leather gloves

            // Weapon for flair
            AddItem(new Scimitar { Hue = 2406, Name = "Caravan’s Edge" }); // Curved blade with notch marks

            // Optional Flair Item
            AddItem(new Cloak { Hue = 2109, Name = "Sunveil Mantle" }); // Light, desert cloak

            // Stats and characteristics
            SetStr(90, 100);
            SetDex(80, 90);
            SetInt(65, 75);

            SetDamage(8, 14);
            SetHits(250, 270);
        }

        public ThalosStonehelm(Serial serial) : base(serial) { }

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
