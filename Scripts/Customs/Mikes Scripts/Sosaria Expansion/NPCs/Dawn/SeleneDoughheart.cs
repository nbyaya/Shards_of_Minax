using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BreadAndDarknessQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bread and Darkness"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Selene Doughheart*, beloved baker of Dawn.\n\n" +
                    "Her hands are dusted in flour, yet her eyes brim with fear and determination.\n\n" +
                    "“Do you smell that? It’s not yeast nor honey—it’s rot. It clings to my flour, my ovens, my soul.”\n\n" +
                    "“The villagers whisper of the **Blighted Beast Lord**, how it slinks from Doom dungeon to curse our grain. My sacks blacken overnight, spoiled by spores unseen but ever-hungry.”\n\n" +
                    "“I’ve baked bread since I could knead, and never have I known such decay. If the Beast Lord is not slain, Dawn will starve before the next harvest.”\n\n" +
                    "“Please, brave soul—**slay the Blighted Beast Lord** and let our hearths burn clean again.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the ovens cool and the fields fail. I pray another will have the courage to save us.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it breathes? The rot deepens. Even the air tastes bitter. I hear its roar in my dreams... and wake to blackened dough.";
            }
        }

        public override object Complete
        {
            get
            {
                return "**The air is sweet again.** My bread rises. My ovens sing. And Dawn will feast once more.\n\n" +
                       "Take this: *SunmirrorDome*. It shall shield you from the darkness, as you have shielded us from hunger.";
            }
        }

        public BreadAndDarknessQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlightedBeastLord), "Blighted Beast Lord", 1));
            AddReward(new BaseReward(typeof(SunmirrorDome), 1, "SunmirrorDome"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Bread and Darkness'!");
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

    public class SeleneDoughheart : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BreadAndDarknessQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBaker());
        }

        [Constructable]
        public SeleneDoughheart()
            : base("the Hearth-Keeper", "Selene Doughheart")
        {
        }

        public SeleneDoughheart(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 90, 20);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1001; // Warm, fair skin tone
            HairItemID = 0x203C; // Long Hair
            HairHue = 1153; // Wheat-gold
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1157, Name = "Honey-Glaze Gown" });
            AddItem(new HalfApron() { Hue = 2129, Name = "Flour-Dusted Apron" });
            AddItem(new Sandals() { Hue = 2419, Name = "Hearth-Worn Sandals" });
            AddItem(new FlowerGarland() { Hue = 1260, Name = "Blooming Wheat Wreath" });

            AddItem(new CooksCleaver() { Hue = 2101, Name = "Bread-Bane Cleaver" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1171;
            backpack.Name = "Baker's Satchel";
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
