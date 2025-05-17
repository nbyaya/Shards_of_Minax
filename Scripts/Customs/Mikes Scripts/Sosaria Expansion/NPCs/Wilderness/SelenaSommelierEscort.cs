using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SelenaSommelierQuest : BaseQuest
    {
        public override object Title { get { return "Vintage Peril"; } }

        public override object Description
        {
            get
            {
                return
                    "*Selena adjusts the delicate bottle in her satchel, her voice smooth and precise.*\n\n" +
                    "\"I am Selena, sommelier of renown, bearer of Sosaria’s last cask of the fabled 'Crimson Reverie'. This vintage must reach Dawn Inn before the sun sets thrice, lest its essence turn sour—or worse, fall into the hands of those who would corrupt it. Bandits, rivals, and worse trail my steps. Will you escort me? The harvest of a hundred years depends on it.\"";
            }
        }

        public override object Refuse { get { return "*Selena narrows her eyes, clutching her satchel protectively.* \"Very well. But know this: once ruined, such a vintage cannot be restored.\""; } }
        public override object Uncomplete { get { return "*Selena breathes nervously, checking the bottle.* \"Time is of the essence. We must proceed.\""; } }

        public SelenaSommelierQuest() : base()
        {
            AddObjective(new EscortObjective("Dawn Inn"));
            AddReward(new BaseReward(typeof(SommelierBodySash), "SommelierBodySash – Enhances charisma in trade and negotiation."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Selena presents the sash with a graceful bow.* \"To you, I owe not just safety, but preservation of history. May your words ever charm, and your ventures prosper.\"", null, 0x59B);
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

    public class SelenaSommelierEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(SelenaSommelierQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWaiter());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public SelenaSommelierEscort() : base()
        {
            Name = "Selena";
            Title = "the Sommelier";
            NameHue = 2075; // Wine red hue
        }

		public SelenaSommelierEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Olive skin tone
            HairItemID = 0x203C; // Long hair
            HairHue = 1161; // Rich chestnut
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2075, Name = "Crimson Taster's Blouse" }); // Deep wine red
            AddItem(new Skirt() { Hue = 2406, Name = "Vintner's Velvet Skirt" }); // Dark plum
            AddItem(new HalfApron() { Hue = 1154, Name = "Apron of the Aged Vine" }); // Silvered grey
            AddItem(new Cloak() { Hue = 1150, Name = "Cloak of Refined Palate" }); // Midnight black
            AddItem(new Shoes() { Hue = 1109, Name = "Sommelier's Step" }); // Dusty brown leather
            AddItem(new FeatheredHat() { Hue = 1153, Name = "Hat of the Harvest Moon" }); // Dark blue with a silver feather
            AddItem(new BodySash() { Hue = 2075, Name = "Sommelier's Sash" }); // Crimson hue matching the shirt

            Backpack backpack = new Backpack();
            backpack.Hue = 1154;
            backpack.Name = "Winekeeper's Pack";
            backpack.DropItem(new BottleOfWine() { Name = "Crimson Reverie", Hue = 2075 });
            AddItem(backpack);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.12)
                {
                    string[] lines = new string[]
                    {
                        "*Selena glances over her shoulder.* I swear I saw a rival sommelier lurking behind that ridge.",
                        "*She cradles her satchel.* The bouquet must remain unshaken... tread softly.",
                        "*Selena hums an old vintner's song.* Each note reminds me of the harvest long past.",
                        "*She adjusts her cloak.* The road is no place for fine wine or fine folk, yet here we are.",
                        "*Her eyes narrow.* I’ve seen bandits spoil more than fortunes—they spoil legacies.",
                        "*Selena breathes deeply.* Dawn Inn awaits. Its cellar shall be graced with a masterpiece.",
                        "*She taps the bottle lightly.* This vintage was sealed on a blood moon... it carries power."
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

}
