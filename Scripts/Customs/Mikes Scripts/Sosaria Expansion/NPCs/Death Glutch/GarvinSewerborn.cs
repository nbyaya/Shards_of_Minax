using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SwarmOfShadowsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Swarm of Shadows"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet **Garvin Sewerborn**, the Rat Catcher of Death Glutch.\n\n" +
                    "He smells of old damp and iron, a tangle of scars marking his hands. One hand is gloved, the other—missing a finger—tightens around a worn net.\n\n" +
                    "\"I've hunted vermin since I could walk. My family? Generations. Cleaned every crawlspace from here to the Vale. But this—this ain’t no rat.\" \n\n" +
                    "\"Down in the Malidor Academy, something's bred a monster in the dark. A **Cursed Rat**, big as a boar and thrice as foul. Spreads disease like fire in dry grass. I faced it once... and lost this.\" He holds up his maimed hand.\n\n" +
                    "\"You’ve got the guts? Then purge the tunnels. Kill it. End it, for me, for my name. My family’s legacy depends on it.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "\"Aye, I thought not. Most don’t. But it’ll keep growing. One day, this whole town’ll rot from its bite.\"";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "\"Still breathin'? Then that beast still breathes too. The air’s heavier now... folks are coughing more. Don't wait too long, friend.\"";
            }
        }

        public override object Complete
        {
            get
            {
                return "\"Dead? Truly? Ha! By the pits, you’ve done it! That thing’s shadow won't stain my name no more. Take these—**AstartesWarBoots**—they’ve served rat catchers well in dark places. May they walk you clear of filth forevermore.\"";
            }
        }

        public SwarmOfShadowsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CursedRat), "Cursed Rat", 1));
            AddReward(new BaseReward(typeof(AstartesWarBoots), 1, "AstartesWarBoots"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Swarm of Shadows'!");
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

    public class GarvinSewerborn : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SwarmOfShadowsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBButcher()); // Closest fit for a rat catcher, deals with vermin and the dirty work.
        }

        [Constructable]
        public GarvinSewerborn()
            : base("the Rat Catcher", "Garvin Sewerborn")
        {
        }

        public GarvinSewerborn(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Short hair
            HairHue = 1107; // Greasy black
            FacialHairItemID = 0x2041; // Short beard
            FacialHairHue = 1107;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherCap() { Hue = 2406, Name = "Sewer-Stained Cap" }); // Dirty grey-green
            AddItem(new StuddedChest() { Hue = 2418, Name = "Verminproof Vest" }); // Dull brownish leather
            AddItem(new StuddedLegs() { Hue = 2403, Name = "Rat-Catcher's Breeches" });
            AddItem(new LeatherGloves() { Hue = 2309, Name = "Muck-Covered Gloves" });
            AddItem(new HalfApron() { Hue = 2413, Name = "Filth-Warden Apron" });
            AddItem(new Boots() { Hue = 1825, Name = "Trencher's Boots" });

            AddItem(new Pitchfork() { Hue = 2101, Name = "Rat Piercer" }); // A battered tool of his trade

            Backpack backpack = new Backpack();
            backpack.Hue = 1109; // Dirty black
            backpack.Name = "Rat Catcher's Satchel";
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
