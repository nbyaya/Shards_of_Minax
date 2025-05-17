using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TilrinEscortQuest : BaseQuest
    {
        public override object Title { get { return "Not Cut Out for This"; } }

        public override object Description
        {
            get
            {
                return
                    "*Tilrin fidgets with his belt, wide-eyed and clearly flustered.*\n\n" +
                    "\"I thought I was ready... I really did! The world’s just so much bigger, scarier than I imagined. Please, can you take me back to the Start Room? I need to rethink everything. It’s embarrassing, but I’ll make it worth your while!\"";
            }
        }

        public override object Refuse { get { return "*Tilrin sighs, kicking a small stone.* \"Yeah... I get it. No one wants to escort the quitter.\""; } }
        public override object Uncomplete { get { return "*Tilrin stumbles a bit, gripping his sword.* \"Uh... can we keep going? I’m not feeling great out here.\""; } }

        public TilrinEscortQuest() : base()
        {
            AddObjective(new EscortObjective("the Start Room"));
            AddReward(new BaseReward(typeof(RockNRollVault), "RockNRollVault – A music box that plays epic tunes and stores items."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Tilrin smiles sheepishly.* \"Thanks for not laughing. This... this is something cool I picked up! You deserve it more than I do. Maybe we’ll meet again when I’m actually ready.\"", null, 0x59B);
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

    public class NoviceTilrin : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(TilrinEscortQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBVarietyDealer());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public NoviceTilrin() : base()
        {
            Name = "Tilrin";
            Title = "the Novice Adventurer";
            NameHue = 0x59B;
        }

		public NoviceTilrin(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(50, 50, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1002; // Fair skin
            HairItemID = 0x203B; // Short, tousled hair
            HairHue = 1153; // Bright brown
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1161, Name = "Tilrin's Lucky Shirt" }); // Bright green
            AddItem(new ShortPants() { Hue = 1150, Name = "Novice's Breeches" }); // Deep black
            AddItem(new LeatherCap() { Hue = 1165, Name = "Cap of Questionable Taste" }); // Vivid purple
            AddItem(new Sandals() { Hue = 1172, Name = "Wanderer's Sandals" }); // Tan brown
            AddItem(new BodySash() { Hue = 1157, Name = "Sash of Determination" }); // Blue
            AddItem(new Dagger() { Hue = 1109, Name = "Practice Blade" }); // Grey metal

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Beginner's Pack";
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
                        "*Tilrin adjusts his cap.* This was supposed to be... fun?",
                        "*Tilrin looks around nervously.* Did you hear that? No? Oh... okay.",
                        "*He sighs.* I miss the Start Room already.",
                        "*Tilrin frowns.* I thought the goblins would be... smaller.",
                        "*Tilrin grips his dagger.* I’m not sure this thing’s even sharp.",
                        "*Tilrin chuckles nervously.* You’ve done this before, right? Escorting people, I mean?",
                        "*He scratches his head.* Maybe I’m just more of a tavern guy.",
                        "*Tilrin hums a silly tune.* They say music soothes the soul... hope it soothes monsters too."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
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
