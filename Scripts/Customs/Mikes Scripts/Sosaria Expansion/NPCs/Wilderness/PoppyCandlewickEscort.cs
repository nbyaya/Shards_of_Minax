using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class PoppyCandlewickQuest : BaseQuest
    {
        public override object Title { get { return "Before the Shadows Fall"; } }

        public override object Description
        {
            get
            {
                return
                    "*Poppy beams at you, though her hands fidget with a sprig of lavender.*\n\n" +
                    "Hello, friend! I’m Poppy Candlewick, a humble gatherer of herbs and lover of tales. I’ve wandered a bit too far from the inn, and these woods… well, they’re quite a bit darker than I remember. Would you be a dear and walk me back before nightfall? They say shadows walk when the moon rises, and I’d rather not find out if that's true!";
            }
        }

        public override object Refuse { get { return "*Poppy gives a nervous laugh.* I suppose I’ll just… pick a few more flowers and hope for the best."; } }
        public override object Uncomplete { get { return "*Poppy glances around, clutching her satchel.* Oh dear, do you hear that? Let’s not dawdle!"; } }

        public PoppyCandlewickQuest() : base()
        {
            AddObjective(new EscortObjective("Inn"));
            AddReward(new BaseReward(typeof(DistilledEssence), "DistilledEssence – A rare alchemical elixir with healing and magical properties."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Poppy smiles brightly, pressing a small vial into your hand.* You have the heart of a true guardian! Here, take this. It’s distilled from moonflowers and morning dew – may it keep you safe in your journeys.");
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

    public class PoppyCandlewickEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(PoppyCandlewickQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public PoppyCandlewickEscort() : base()
        {
            Name = "Poppy Candlewick";
            Title = "the Traveling Herbalist";
            NameHue = 0x22; // Bright Yellow name hue for cheerfulness
        }

		public PoppyCandlewickEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 65, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1053; // Light rosy complexion
            HairItemID = 0x203B; // Braided hair
            HairHue = 1154; // Sunlit Blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1153, Name = "Candlewick’s Bloom" }); // Soft lavender color
            AddItem(new FlowerGarland() { Hue = 1177, Name = "Garland of the Morning Star" }); // Floral crown
            AddItem(new Sandals() { Hue = 2101, Name = "Traveler’s Petal Slippers" }); // Pale green, nature tone
            AddItem(new Cloak() { Hue = 1171, Name = "Wanderer's Shawl" }); // Soft sky blue

            AddItem(new HerbSatchel() { Hue = 1157, Name = "Poppy’s Herb Pouch" }); // Custom container for flavor

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Scented Pack";
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
                        "*Poppy hums a cheerful tune.* This place isn't so scary with you around!",
                        "*She glances nervously at the trees.* Did you hear something? Maybe just the wind...",
                        "*Poppy clutches a vial.* I always carry moonflower essence – it keeps nightmares away!",
                        "*Her eyes widen slightly.* They say the woods are alive... but I think they're just lonely.",
                        "*She smiles warmly.* You remind me of a knight from the stories I read!",
                        "*Poppy gathers a sprig of mint.* Just a little longer... we’ll be safe at the inn soon!",
                        "*She laughs softly.* I really must stop wandering off... but the flowers call to me!"
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));
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

    public class HerbSatchel : Bag
    {
        [Constructable]
        public HerbSatchel() : base()
        {
            Name = "Poppy’s Herb Pouch";
            Hue = 1157;
            DropItem(new Garlic(5));
            DropItem(new Ginseng(5));
            DropItem(new MandrakeRoot(5));
            DropItem(new Nightshade(5));
        }

        public HerbSatchel(Serial serial) : base(serial) { }

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
