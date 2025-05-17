using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class CryOfWoeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Cry of Woe"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Barnaby Quill*, the Poet Laureate of Pirate Isle, seated upon a weathered crate, quill in hand, parchment torn asunder.\n\n" +
                    "He looks up, eyes rimmed with sleeplessness, voice thick with despair:\n\n" +
                    "“Every verse I pen withers beneath the wail of that *damned* thing—**the EternalWoe**. Once my words soared, filled taverns with cheer! Now they crumble into ash and gibberish.”\n\n" +
                    "“This monster haunts the Exodus Dungeon. Its moans are not just sound—they’re **thieves of inspiration**, snuffing out the soul of song itself.”\n\n" +
                    "“I’ve heard it. I’ve recorded its lament in a ballad I dare not sing. Slay it, brave soul, and I shall reclaim my voice—and the Isle shall hear true verse once more.”\n\n" +
                    "**Slay the EternalWoe**—before all poets fall silent.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may your dreams remain untroubled, unlike mine... which are stitched with screams.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it wails? Still my verses twist into nonsense? I cannot bear it...";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The Woe is silenced?\n\n" +
                       "Bless you, bold soul! Already I feel the whispers of rhyme returning...\n\n" +
                       "Take this *ConfederationCache*. The poets of Pirate Isle shall sing your praises long after the tides forget your name.";
            }
        }

        public CryOfWoeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(EternalWoe), "EternalWoe", 1));
            AddReward(new BaseReward(typeof(ConfederationCache), 1, "ConfederationCache"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Cry of Woe'!");
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

    public class BarnabyQuill : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(CryOfWoeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBArchitect());
        }

        [Constructable]
        public BarnabyQuill()
            : base("the Poet Laureate", "Barnaby Quill")
        {
        }

        public BarnabyQuill(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1154; // Pale silver
            FacialHairItemID = 0; // Clean-shaven
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1153, Name = "Poet's Silk Shirt" }); // Midnight blue
            AddItem(new LongPants() { Hue = 1150, Name = "Shadowstitch Breeches" }); // Deep gray
            AddItem(new Cloak() { Hue = 1157, Name = "Wailing Cloak" }); // Stormy violet
            AddItem(new FeatheredHat() { Hue = 1154, Name = "Muse's Crest" }); // Pale silver with a dark plume
            AddItem(new Shoes() { Hue = 1109, Name = "Cobblestone Treaders" }); // Dust-gray

            AddItem(new ResonantHarp() { Hue = 1165, Name = "Songbreaker" }); // A harp, muted by sorrow

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Scrollcase of Sorrows";
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
