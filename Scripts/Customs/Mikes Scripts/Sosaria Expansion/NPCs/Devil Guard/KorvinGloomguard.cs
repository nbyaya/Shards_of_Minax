using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class PixiesEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Pixie's End"; } }

        public override object Description
        {
            get
            {
                return
                    "Korvin Gloomguard stands at the edge of Devil Guard's steaming baths, his eyes hollow with sleepless dread.\n\n" +
                    "“There’s something in the Mines of Minax,” he growls, “something small, winged, but crueler than any beast. **The GrittyPixie.** She comes at night, whispering, laughing... twisting dreams into madness.”\n\n" +
                    "“I wear these blessed earplugs, but still I hear her. She won’t let me sleep. Won’t let my men work in peace.”\n\n" +
                    "“You’ve got to end it. **Kill the GrittyPixie** before she drives us all off the edge.”";
            }
        }

        public override object Refuse
        {
            get { return "Then may you never know what it’s like to fear sleep."; }
        }

        public override object Uncomplete
        {
            get { return "She still laughs... louder now. The miners are breaking. I can’t hold them back much longer."; }
        }

        public override object Complete
        {
            get
            {
                return "Silence...\n\n" +
                       "I can finally rest. You’ve done more than you know. Here—take this, the *BreakersHauberk*. May it shield you from the things that haunt us in the dark.";
            }
        }

        public PixiesEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GrittyPixie), "GrittyPixie", 1));
            AddReward(new BaseReward(typeof(BreakersHauberk), 1, "BreakersHauberk"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Pixie's End'!");
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

    public class KorvinGloomguard : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(PixiesEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHairStylist());
        }

        [Constructable]
        public KorvinGloomguard()
            : base("the Night Watcher", "Korvin Gloomguard")
        {
        }

        public KorvinGloomguard(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1102; // Pale, sleepless complexion
            HairItemID = 0x203C; // Short Hair
            HairHue = 1109; // Dark gray
            FacialHairItemID = 0x2041; // Long Beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedDo() { Hue = 2401, Name = "Watcher’s Hauberk" }); // Ash-grey armor
            AddItem(new StuddedLegs() { Hue = 2419, Name = "Midnight Greaves" }); // Deep steel blue
            AddItem(new StuddedGloves() { Hue = 2407, Name = "Grip of Vigilance" }); // Shadowed leather
            AddItem(new StuddedGorget() { Hue = 2413, Name = "Neckguard of Silence" }); // Steel blue
            AddItem(new Cloak() { Hue = 1109, Name = "Gloomshroud Cloak" }); // Dark gray
            AddItem(new Boots() { Hue = 1108, Name = "Stonewalker Boots" }); // Dust-black

            AddItem(new WarMace() { Hue = 1109, Name = "Sleepbreaker" });

            // Blessed earplugs item (decorative only)
            Item earplugs = new SkullCap() { Hue = 2412, Name = "Blessed Earplugs" };
            earplugs.Movable = false;
            AddItem(earplugs);
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
