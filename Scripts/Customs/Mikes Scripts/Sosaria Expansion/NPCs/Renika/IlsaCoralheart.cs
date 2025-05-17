using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FiddlersFollyQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Fiddler’s Folly"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Ilsa Coralheart*, renowned Coral Sculptor of Renika.\n\n" +
                    "Her hands are flecked with rare pigments, and her eyes glisten like the reefs she loves.\n\n" +
                    "“Have you heard it? The shrill screech that echoes over the tide at dusk? That’s the *CragFiddler*, a monstrous wretch hiding in the Mountain Stronghold.”\n\n" +
                    "“Its discordant tune has shattered more than the peace of this island—my coral sculptures, my life’s work, are cracking. The reefs themselves tremble at its noise.”\n\n" +
                    "“I’ve tried muting the tides, using pigments from the deepest pools, but nothing holds. If this keeps on, Renika’s reefs will crumble—and with them, our trade and beauty.”\n\n" +
                    "**Slay the CragFiddler**, and let harmony return to our shores. I cannot offer gold, but I will part with something dear—a *WitchesBrewedHat*, crafted under moonlit tides.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the reefs will continue to sing their death song. I will watch my life’s work crumble, one shard at a time.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The CragFiddler still sings? I can feel it—each note like a crack in my heart, and in the coral.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The silence… oh, how sweet it is! You’ve done more than silence a beast—you’ve saved the soul of Renika’s reefs.\n\n" +
                       "Here, take this *WitchesBrewedHat*—a token of tides calmed, and storms soothed. May it ward your dreams from discord.";
            }
        }

        public FiddlersFollyQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CragFiddler), "CragFiddler", 1));
            AddReward(new BaseReward(typeof(WitchesBrewedHat), 1, "WitchesBrewedHat"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Fiddler’s Folly'!");
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

    public class IlsaCoralheart : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FiddlersFollyQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGlassblower());
        }

        [Constructable]
        public IlsaCoralheart()
            : base("the Coral Sculptor", "Ilsa Coralheart")
        {
        }

        public IlsaCoralheart(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 60, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Coral-pink hair
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1358, Name = "Tidewoven Gown" }); // Pale coral pink
            AddItem(new FlowerGarland() { Hue = 1153, Name = "Reefbloom Circlet" }); // Coral-pink flowers
            AddItem(new Sandals() { Hue = 1150, Name = "Pearlshell Sandals" }); // Pearlescent hue
            AddItem(new BodySash() { Hue = 1260, Name = "Pigment-Stained Sash" }); // Deep sea green

            Backpack backpack = new Backpack();
            backpack.Hue = 1165;
            backpack.Name = "Pigment Satchel";
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
