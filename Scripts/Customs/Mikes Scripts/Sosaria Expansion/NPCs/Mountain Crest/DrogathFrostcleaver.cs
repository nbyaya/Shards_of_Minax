using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class AxeAgainstIceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Axe Against Ice"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Drogath Frostcleaver*, the grizzled orc-hunter of Mountain Crest.\n\n" +
                    "His armor glints with frost, his eyes sharp as ice, and a jagged scar runs across his face.\n\n" +
                    "“There’s a beast in the Ice Cavern. An orc, but not like the others—this one took the Frost Queen’s ramparts for its own. Wields an axe **dripping with frozen blood**. Slain warbands, taken hunters… Now it’s my hunt.”\n\n" +
                    "“I’ve faced orcs my whole life. Led raids. Broke bones. But this one—**this one is mine**. Bring me its head, and I’ll give you what I was saving for my last battle.”\n\n" +
                    "**Slay the Frost-Axe Orc** atop the Ice Cavern ramparts.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Hesitate if you must. But remember, each moment that beast breathes, our mountain grows colder.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You still breathe, but so does it? Don’t let the frost settle in your bones. Strike now.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The Frost-Axe Orc falls... and the cold lifts, if only slightly.\n\n" +
                       "**You’ve ended my final hunt.** The axe, the beast, the curse—it’s done.\n\n" +
                       "Take this, my last hoard: the *SocialMediaMavensChest*. Use it well, and remember, **ice remembers blood**.";
            }
        }

        public AxeAgainstIceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FrostAxeOrc), "Frost-Axe Orc", 1));
            AddReward(new BaseReward(typeof(SocialMediaMavensChest), 1, "SocialMediaMavensChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Axe Against Ice'!");
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

    public class DrogathFrostcleaver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(AxeAgainstIceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBButcher()); // Drogath, being an orc-hunter, deals in axe weapons.
        }

        [Constructable]
        public DrogathFrostcleaver()
            : base("the Veteran Orc Hunter", "Drogath Frostcleaver")
        {
        }

        public DrogathFrostcleaver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 70);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1150; // Pale, frost-touched skin.
            HairItemID = 8251; // Wild, frost-streaked mane.
            HairHue = 1153; // Ice white.
            FacialHairItemID = 8265; // Thick beard.
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedDo() { Hue = 1150, Name = "Frost-Hardened Chest" });
            AddItem(new StuddedLegs() { Hue = 1152, Name = "Orc-Hunter’s Greaves" });
            AddItem(new StuddedGloves() { Hue = 2101, Name = "Ice-Bound Grips" });
            AddItem(new OrcHelm() { Hue = 1153, Name = "Trophy Helm of Frostfang" });
            AddItem(new Cloak() { Hue = 2105, Name = "Hunter’s Frostcloak" });
            AddItem(new Boots() { Hue = 2101, Name = "Snow-Stompers" });

            AddItem(new DoubleAxe() { Hue = 2105, Name = "Frostcleaver" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Orc-Hunter's Pack";
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
