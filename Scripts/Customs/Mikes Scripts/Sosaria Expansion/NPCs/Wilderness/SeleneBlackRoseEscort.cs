using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SeleneBlackRoseQuest : BaseQuest
    {
        public override object Title { get { return "Thorns of the Past"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Her eyes gleam with both sorrow and defiance, a rose clasped tightly in one hand.*\n\n" +
                    "I am Selene, once of the Dark Tower, now cast out for love deemed forbidden. But love, like magic, does not yield to fear. I must return, to end what was begun in shadows. The way is perilous, the curses deep... will you walk with me into the heart of darkness, that I may reclaim what was stolen?";
            }
        }

        public override object Refuse { get { return "*Selene’s lips curl in a bitter smile.* Then leave me to fade with the night."; } }
        public override object Uncomplete { get { return "*The rose in her hand wilts slightly.* The thorns grow sharper... we must press on."; } }

        public SeleneBlackRoseQuest() : base()
        {
            AddObjective(new EscortObjective("the Dark Tower"));
            AddReward(new BaseReward(typeof(RasSearingDagger), "RasSearingDagger – A dagger of flame, burning with passion and vengeance."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Selene breathes deeply, her eyes closing in peace.* The past is buried... and you have freed me from its weight. Take this, forged in flame and loss. May it strike true for you, as it did for me.", null, 0x21F);
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

    public class SeleneBlackRoseEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(SeleneBlackRoseQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBNecromancer());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public SeleneBlackRoseEscort() : base()
        {
            Name = "Selene";
            Title = "the Black Rose";
            NameHue = 2075;
        }

		public SeleneBlackRoseEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 65);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale complexion
            HairItemID = 0x203B; // Wavy hair
            HairHue = 1157; // Deep midnight blue
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1175, Name = "Shroud of Lost Love" }); // Black with crimson lining
            AddItem(new LeatherGloves() { Hue = 1154, Name = "Thorn-Touched Gloves" }); // Blood red
            AddItem(new ThighBoots() { Hue = 1175, Name = "Boots of Silent Roads" }); // Black leather
            AddItem(new Cloak() { Hue = 1157, Name = "Cloak of the Mourning Star" }); // Deep midnight blue
            AddItem(new BodySash() { Hue = 1153, Name = "Petal Sash" }); // Dark rose

            AddItem(new AssassinSpike() { Hue = 1154, Name = "Lover’s Dagger" }); // Gleaming red blade

            Backpack backpack = new Backpack();
            backpack.Hue = 1157;
            backpack.Name = "Selene’s Satchel";
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
                        "*Selene clutches the black rose tighter.* He waits... bound in silence, as I was.",
                        "*Her voice trembles.* The Tower remembers... and it does not forgive.",
                        "*A soft sigh escapes her lips.* Love should not be a crime. And yet, I was condemned.",
                        "*Selene’s gaze hardens.* We are close... I can feel the thorns of fate tighten.",
                        "*The shadows ripple.* When we reach the Tower, there will be no turning back.",
                        "*She hums a haunting melody.* It was his song... I still hear it in dreams.",
                        "*Selene’s eyes flare briefly.* The flames within me have never died."
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
