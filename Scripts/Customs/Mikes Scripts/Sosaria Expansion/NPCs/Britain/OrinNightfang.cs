using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RedFoxHuntQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Red Fox Hunt"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Orin Nightfang*, Beastwarden of Castle British, near a series of torn training pens.\n\n" +
                    "His silver-threaded nets hang over his shoulders, shimmering faintly in the light. His eyes are sharp, watchful—like one who has spent too many nights under cursed moons.\n\n" +
                    "“The vault livestock are dead. Not just killed—*bled dry*. I tracked the creature through sealed sectors of **Preservation Vault 44**. It left behind claw marks that sing with lunar energy.”\n\n" +
                    "“A Hemovulpine. Fox in shape, demon in hunger. It drinks life itself, and it’s hunting close.”\n\n" +
                    "“Help me end this. Slay the beast before it slips into deeper vaults. I’ll see you rewarded with gear designed for rough terrain—**Pathspike Greaves**.”\n\n" +
                    "“And take care... its blood burns like silver flame.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then be wary. The Hemovulpine will not stop at livestock—it knows the scent of fear now.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it prowls? The vault echoes with death. Even the walls feel colder.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done? You’ve stopped its hunt...\n\n" +
                       "I’ve prepared these *Pathspike Greaves* for one brave enough to face a blood-born predator.\n\n" +
                       "May they keep your footing sure where shadows fall.";
            }
        }

        public RedFoxHuntQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Hemovulpine), "Hemovulpine", 1));
            AddReward(new BaseReward(typeof(PathspikeGreaves), 1, "Pathspike Greaves"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Red Fox Hunt'!");
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

    public class OrinNightfang : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RedFoxHuntQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer()); // Fits his Beastwarden role.
        }

        [Constructable]
        public OrinNightfang()
            : base("the Beastwarden", "Orin Nightfang")
        {
        }

        public OrinNightfang(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1002; // Pale, moon-touched skin tone.
            HairItemID = 0x2047; // Long Hair
            HairHue = 1150; // Midnight black.
            FacialHairItemID = 0x203B; // Short beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2101, Name = "Nightfang's Vest" }); // Deep silver-grey
            AddItem(new LeatherLegs() { Hue = 2405, Name = "Vaultstalker Trousers" }); // Stone-grey
            AddItem(new StuddedGloves() { Hue = 2500, Name = "Moon-Grip Gloves" }); // Faint silver
            AddItem(new LeatherCap() { Hue = 2413, Name = "Beastwarden's Hood" }); // Dark charcoal
            AddItem(new Boots() { Hue = 1819, Name = "Tracker’s Boots" }); // Earth-brown
            AddItem(new Cloak() { Hue = 2106, Name = "Silver-Threaded Cloak" }); // Faint shimmering cloak

            AddItem(new GnarledStaff() { Hue = 2500, Name = "Beastward Pole" }); // Used for guiding beasts, laced with silver bands.

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Beastwarden’s Pack";
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
