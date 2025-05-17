using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class NellaLorekeeperQuest : BaseQuest
    {
        public override object Title { get { return "The Lore Must Survive"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Archivist Nella clutches a bundle of tattered scrolls, her eyes darting with urgency.*\n\n" +
                    "\"You there! I am Nella, keeper of scattered truths. I have gathered what remains of ancient knowledge—fragments torn from the edge of oblivion. But something... something comes for them. If I do not reach the Town Room soon, all will be lost. Will you escort me to safety, before the dark veils what must be known?\"";
            }
        }

        public override object Refuse { get { return "*Nella’s voice falls, barely above a whisper.* \"Then may the past forgive our forgetfulness.\""; } }
        public override object Uncomplete { get { return "*Nella gasps, clutching her satchel tighter.* \"They are drawing near... we must not falter now.\""; } }

        public NellaLorekeeperQuest() : base()
        {
            AddObjective(new EscortObjective("the Town Room"));
            AddReward(new BaseReward(typeof(StrongBox), "EliteFoursVault – A strongbox containing powerful gear for elite adventurers."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Nella breathes a sigh of relief as she unrolls the scrolls within the Town Room.* \"You have done Sosaria a great service. The knowledge endures... for now. Take this vault, may its strength shield you from the shadows that seek to erase us.\"", null, 0x59B);
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

    public class ArchivistNellaEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(NellaLorekeeperQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe(this));
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public ArchivistNellaEscort() : base()
        {
            Name = "Nella";
            Title = "the Lorekeeper";
            NameHue = 0x59B; // Scholarly blue
        }
		public ArchivistNellaEscort(Serial serial) : base(serial) { }
        public override void InitBody()
        {
            InitStats(50, 60, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Light complexion
            HairItemID = 0x2049; // Wavy hair
            HairHue = 1153; // Midnight blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1153, Name = "Moonshadow Blouse" }); // Deep blue
            AddItem(new ElvenPants() { Hue = 1157, Name = "Starwoven Trousers" }); // Soft silver
            AddItem(new Sandals() { Hue = 1175, Name = "Whisperstep Sandals" }); // Shadowed gray
            AddItem(new Cloak() { Hue = 1151, Name = "Cloak of the Hidden Truths" }); // Midnight with soft shimmer
            AddItem(new LeatherArms() { Hue = 1164, Name = "Scriptbound Sleeves" }); // Faded parchment hue
            AddItem(new BodySash() { Hue = 1161, Name = "Sash of Eternal Lore" }); // Deep indigo

            AddItem(new ScribeSword() { Hue = 1109, Name = "Inkfang Blade" }); // Blackened steel with runes

            Backpack backpack = new Backpack();
            backpack.Hue = 1108;
            backpack.Name = "Archivist's Reliquary";
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
                        "*Nella grips a scroll tightly.* I can feel them... trying to make me forget.",
                        "*She murmurs.* If I fall, promise me the scrolls reach the Town Room.",
                        "*Her eyes dart around.* We are not alone... something watches from beyond.",
                        "*Nella speaks rapidly.* These words were almost lost once. Never again.",
                        "*A chill runs down your spine.* Darkness feeds on forgotten truths.",
                        "*She whispers.* The Town Room is hallowed ground... we must reach it.",
                        "*Nella clutches her sash.* They say knowledge is power, but it is also a burden.",
                        "*Her voice steadies.* Thank you for standing between me and oblivion."
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
