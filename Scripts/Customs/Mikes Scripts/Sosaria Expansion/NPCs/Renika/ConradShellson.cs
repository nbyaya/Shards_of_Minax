using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EyeOfTheStoneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Eye of the Stone"; } }

        public override object Description
        {
            get
            {
                return
                    "Conrad Shellson, the renowned merchant of Renika, gestures to an elaborate map marked with a blood-red 'X'.\n\n" +
                    "\"Have you heard of the GraniteEye? A monstrosity of stone and malice, guarding a relic of immense value—a **crystal orb** that can petrify with a mere glance.\n\n" +
                    "This orb once protected my caravans from thieves, but the beast that now holds it has grown too bold, too dangerous.\n\n" +
                    "**Slay the GraniteEye** and retrieve the orb for the Merchants' Guild vault. You will be well rewarded, adventurer, and your name sung along Renika’s docks.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "A shame. The longer that creature holds the orb, the more our trade falters. Come back if you find courage—or a taste for rare coin.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still you live, yet the GraniteEye breathes? Our trade routes remain cursed, and time grows short.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You have returned victorious—and whole! The orb is safe in my possession once more. The guild owes you more than just gratitude.\n\n" +
                       "**Take these gauntlets**, forged by our finest artisans. May they serve you as well as you have served us.";
            }
        }

        public EyeOfTheStoneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GraniteEye), "GraniteEye", 1));
            AddReward(new BaseReward(typeof(ArtisansCraftedGauntlets), 1, "ArtisansCraftedGauntlets"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Eye of the Stone'!");
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

    public class ConradShellson : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(EyeOfTheStoneQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith());
        }

        [Constructable]
        public ConradShellson()
            : base("the Exotic Merchant", "Conrad Shellson")
        {
        }

        public ConradShellson(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 70, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1150; // Slightly tanned skin
            HairItemID = 0x203C; // Long Hair
            HairHue = 1153; // Deep sea blue
            FacialHairItemID = 0x204B; // Medium Beard
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1154, Name = "Seasilk Shirt" }); // Iridescent blue
            AddItem(new LongPants() { Hue = 1109, Name = "Deepcurrent Trousers" }); // Dark ocean
            AddItem(new HalfApron() { Hue = 1358, Name = "Trader’s Wrap" }); // Golden linen
            AddItem(new Boots() { Hue = 1108, Name = "Driftwood Boots" }); // Stormy gray
            AddItem(new Cloak() { Hue = 1175, Name = "Tideborne Cloak" }); // Azure green
            AddItem(new TricorneHat() { Hue = 1157, Name = "Merchant’s Crest" }); // Seafoam white

            AddItem(new Dagger() { Hue = 1151, Name = "Seafarer's Fang" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1170;
            backpack.Name = "Merchants' Satchel";
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
