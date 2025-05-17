using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GuardShatteredQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Guard Shattered"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Rowan Storyleaf*, a weaver of tales beneath the golden eaves of Dawn.\n\n" +
                    "They clutch a tattered scroll, eyes darting to the hills where **Doom** casts its shadow.\n\n" +
                    "“Have you heard the footsteps? Thunder on the edge of sense... The **Doom Guardian** walks again.”\n\n" +
                    "“It came from the borderlands, shield emblazoned with a forgotten order's crest, and stole what I hold dear—scrolls of lore, stories of our past.”\n\n" +
                    "“I cannot let those tales fade into the void. You must **slay the Doom Guardian**, and bring back what was taken, before silence becomes our only history.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then we are undone by silence. And the Guardian shall walk unchallenged, our stories lost beneath its tread.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Guardian still walks? My heart falters with each echo of its march. Please, find the strength to face it.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You return with more than scrolls—you've reclaimed **our voice** from the jaws of the void.\n\n" +
                       "Take this, *BakersHexguard*, enchanted by the fires of Dawn itself, as thanks. And may our stories never fall silent again.";
            }
        }

        public GuardShatteredQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DoomGuardian), "Doom Guardian", 1));
            AddReward(new BaseReward(typeof(BakersHexguard), 1, "BakersHexguard"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Guard Shattered'!");
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

    public class RowanStoryleaf : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(GuardShatteredQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner());
        }

        [Constructable]
        public RowanStoryleaf()
            : base("the Storyteller", "Rowan Storyleaf")
        {
        }

        public RowanStoryleaf(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1130; // Warm autumn-brown
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 2124, Name = "Loreweaver's Dress" }); // Autumn-leaf orange
            AddItem(new Cloak() { Hue = 2213, Name = "Twilight Cloak" }); // Deep twilight blue
            AddItem(new Sandals() { Hue = 1153, Name = "Wanderer's Sandals" }); // Dark forest green
            AddItem(new BodySash() { Hue = 1157, Name = "Scrollbinder's Sash" }); // Pale parchment color
            AddItem(new FeatheredHat() { Hue = 1166, Name = "Teller's Plume" }); // Crimson feathered hat
            AddItem(new GnarledStaff() { Hue = 2211, Name = "Talespinner's Cane" }); // Mossy green staff
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
