using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ThornGrumbletoeQuest : BaseQuest
    {
        public override object Title { get { return "Ticking Trouble"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Thorn’s eyes dart nervously, and a small puff of smoke escapes a contraption on his back.*\n\n" +
                    "\"You there! Yes, you look nimble! I'm Thorn Grumbletoe, genius inventor of gadgets most grand. Trouble is, one of them might be... um, unstable. I must get back to my workshop in Dawn before something EXPLODES. Escort me swiftly, friend, or we may both end up rather singed!\"";
            }
        }

        public override object Refuse { get { return "*Thorn frowns, fiddling with a sparking device.* \"Well, don't blame me if the sky lights up... again.\""; } }
        public override object Uncomplete { get { return "*A sudden POP startles you.* \"Oh no! That was the Lume-o-Spark! We best hurry!\""; } }

        public ThornGrumbletoeQuest() : base()
        {
            AddObjective(new EscortObjective("the town of Dawn"));
            AddReward(new BaseReward(typeof(RareGrease), "RareGrease – Used for repairing and enhancing mechanical pets or tools."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Thorn breathes a sigh of relief as he reaches his workshop.* \"Safe! For now. Here, take this RareGrease—may your tools never squeak nor spark!\"", null, 0x59B);
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

    public class ThornGrumbletoeEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(ThornGrumbletoeQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTinker(this));
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public ThornGrumbletoeEscort() : base()
        {
            Name = "Thorn Grumbletoe";
            Title = "the Inventor";
            NameHue = 0x59B;
        }

		public ThornGrumbletoeEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(50, 40, 60);

            Female = false;
            Body = 0x190; // Male


            Hue = 1022; // Tanned gnomish complexion
            HairItemID = 0x203B; // Short, spiky hair
            HairHue = 1153; // Copper-red
            FacialHairItemID = 0x2041; // Braided beard
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1165, Name = "Sparkweave Shirt" }); // Vibrant teal
            AddItem(new ShortPants() { Hue = 1173, Name = "Tinkerer's Breeches" }); // Brass-toned
            AddItem(new HalfApron() { Hue = 1150, Name = "Oil-Stained Apron" }); // Soot-black
            AddItem(new Sandals() { Hue = 1175, Name = "Copper-Toe Sandals" }); // Burnished copper
            AddItem(new WideBrimHat() { Hue = 1153, Name = "Gearwheel Cap" }); // Rust-red, with gears
            AddItem(new Cloak() { Hue = 1166, Name = "Smokeplume Cloak" }); // Faintly shimmering grey

            AddItem(new GearLauncher() { Hue = 1177, Name = "Bolt-o-Matic" }); // A quirky mechanical weapon

            Backpack backpack = new Backpack();
            backpack.Hue = 1155; // Steely grey
            backpack.Name = "Gadgeteer's Pack";
            AddItem(backpack);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.15)
                {
                    string[] lines = new string[]
                    {
                        "*A puff of smoke bursts from Thorn’s pack.* Mind the cart! It’s... sensitive.",
                        "*Thorn mutters, tightening a bolt.* Did I remember to disarm the Boom-o-Tron 3000? Hmm...",
                        "*You hear a loud ticking.* Oh! That’s just the Chrono-Tick. Probably harmless!",
                        "*He fumbles with a gadget.* This thing should whistle... or explode. Let's hope for whistle.",
                        "*Thorn chuckles nervously.* Gnomish engineering! Brilliant, dangerous, and rarely predictable.",
                        "*A small spark flies past.* That wasn’t supposed to happen. Let’s keep moving!",
                        "*He beams at you.* I’ve almost got the design perfect! Just a few more tweaks... and no more fires."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_NextTalkTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextTalkTime = reader.ReadDateTime();
        }
    }
}
