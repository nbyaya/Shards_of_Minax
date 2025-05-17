using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class CrabbedChaosQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Crabbed Chaos"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Eadric Grainkeeper*, Mill Overseer of East Montor.\n\n" +
                    "His clothes bear the dust of the mill, yet his stance is proud, if tense. His hands clench an old ledger, the pages torn and stained.\n\n" +
                    "“The harvest… it won’t survive another attack.”\n\n" +
                    "“There’s a beast in the Caves of Drakkon—a vile *DraconianCrab*. It crawls down under moonlight, pincers gleaming like cursed scythes, and lays waste to my silos. It’s not just grain—it’s our lives, our winters. Locals swear its shell glows when the moon is full, a sick light that calls it to feed.”\n\n" +
                    "“I’ve patched walls, raised alarms, even paid smugglers for traps. Nothing holds. You—you're not from here. Maybe you’ve got a chance. *Kill that crab*, before East Montor’s fields are emptied.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may you never hunger, stranger. For we might not have grain enough for the season.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it claws? I hear it at night, scraping… laughing. We won’t hold much longer.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The silos stand, and the moon’s light doesn’t chill me so. You’ve saved more than grain—you’ve given us time.\n\n" +
                       "Take these: *RedoranDefendersGreaves*. They were my father’s, from the old wars. He'd want them worn by someone strong enough to guard the harvest.";
            }
        }

        public CrabbedChaosQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DraconianCrab), "DraconianCrab", 1));
            AddReward(new BaseReward(typeof(RedoranDefendersGreaves), 1, "RedoranDefendersGreaves"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Crabbed Chaos'!");
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

    public class EadricGrainkeeper : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(CrabbedChaosQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiller());
        }

        [Constructable]
        public EadricGrainkeeper()
            : base("the Mill Overseer", "Eadric Grainkeeper")
        {
        }

        public EadricGrainkeeper(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 80, 25);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1815; // Wheat-blonde
            FacialHairItemID = 0x203B; // Full beard
            FacialHairHue = 1815;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2125, Name = "Threshing Tunic" }); // Warm Gold
            AddItem(new HalfApron() { Hue = 2117, Name = "Millstone Apron" }); // Grain Brown
            AddItem(new LeatherCap() { Hue = 2118, Name = "Dusty Workman's Cap" }); // Pale Tan
            AddItem(new LeatherGloves() { Hue = 2106, Name = "Wheatfield Gloves" }); // Light Tan
            AddItem(new LeatherLegs() { Hue = 2407, Name = "Chaff-Hardened Pants" }); // Earthy Brown
            AddItem(new Boots() { Hue = 1820, Name = "Threshwalkers" }); // Muddy Brown

            AddItem(new Pitchfork() { Hue = 0, Name = "Grainkeeper’s Fork" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Mill Overseer's Pack";
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
