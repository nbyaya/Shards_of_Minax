using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class CrawlOfBrokenRunesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Crawl of Broken Runes"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Talia Runeweaver*, the Sigil Scribe of Castle British, in the dim glow of enchanted candles, parchment strewn across her desk.\n\n" +
                    "Her robes are marked with stains of a viscous, dark ink, and her eyes are heavy with sleepless determination.\n\n" +
                    "“The seals… they’re breaking. It’s in Vault 44. A crawling horror, something vile from beyond the ink, is dismantling the wards I crafted to hold the Vault’s magic at bay.”\n\n" +
                    "“They call it the **SigilCrawler**—a beast born of broken glyphs and corrupted scribe’s blood. My scriptorium was the first to fall victim, my parchments devoured, my sigils warped.”\n\n" +
                    "“I’ve infused parchment with wards strong enough to repel its corruption, but we need more than paper now. We need action.”\n\n" +
                    "**Slay the SigilCrawler** before it unwrites the magic that binds the Vault. If it finishes its task, all contained within will be loosed upon Sosaria.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then we are already lost. Each rune it breaks brings us closer to chaos.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it crawls? I feel the wards tearing. The ink seeps through the walls now.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It is done? The Vault remains intact… barely. My thanks are ink and breath, but take these gauntlets—**GauntletsOfTheFinalHammer**—crafted for those who wield strength to preserve the written world.";
            }
        }

        public CrawlOfBrokenRunesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SigilCrawler), "SigilCrawler", 1));
            AddReward(new BaseReward(typeof(GauntletsOfTheFinalHammer), 1, "GauntletsOfTheFinalHammer"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Crawl of Broken Runes'!");
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

    public class TaliaRuneweaver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(CrawlOfBrokenRunesQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe(this)); // She is a scribe
        }

        [Constructable]
        public TaliaRuneweaver()
            : base("the Sigil Scribe", "Talia Runeweaver")
        {
        }

        public TaliaRuneweaver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1001; // Pale skin
            HairItemID = 0x2047; // Long hair
            HairHue = 1153; // Midnight blue
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1175, Name = "Shroud of Runic Wards" });
            AddItem(new FancyShirt() { Hue = 1150, Name = "Ink-Stained Shirt" });
            AddItem(new LongPants() { Hue = 1109, Name = "Midnight Scribe's Pants" });
            AddItem(new ThighBoots() { Hue = 1102, Name = "Boots of Silent Steps" });
            AddItem(new Cloak() { Hue = 1175, Name = "Cloak of the Sealed Sigil" });
            AddItem(new ScribeSword() { Hue = 0, Name = "Rune-etched Blade" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Scriptorium Satchel";
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
