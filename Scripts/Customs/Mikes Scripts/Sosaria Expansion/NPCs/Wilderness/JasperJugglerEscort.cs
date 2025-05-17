using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class JasperJugglerQuest : BaseQuest
    {
        public override object Title { get { return "Juggling with Fate"; } }

        public override object Description
        {
            get
            {
                return 
                    "Oh no, no, no! They’re getting worse! I’m Jasper, Jasper the Juggler—but these enchanted juggling balls have minds of their own now! One flew off and nearly broke a window, another keeps whispering numbers! I must reach Britain Inn to find a mage who can break the spell. Please, friend, I need your help! Escort me safely, or I'll be pelted to death by my own act!";
            }
        }

        public override object Refuse { get { return "What? You’re just going to leave me here with these things? One of them just called me ‘clumsy’!"; } }
        public override object Uncomplete { get { return "Please, we must keep moving! They're starting to argue about who gets to bonk me next!"; } }

        public JasperJugglerQuest() : base()
        {
            AddObjective(new EscortObjective("Britain Inn"));
            AddReward(new BaseReward(typeof(AerobicsInstructorsLegwarmers), "Aerobics Instructor's Legwarmers"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("You did it! I can breathe again without dodging! Take these legwarmers, they'll make you lighter on your feet—trust me, I know a thing or two about dodging!", null, 0x59B);
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

    public class JasperJugglerEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(JasperJugglerQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public JasperJugglerEscort() : base()
        {
            Name = "Jasper the Mad Juggler";
            Title = "the Enchanted Performer";
            NameHue = 0x488;
        }

		public JasperJugglerEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(40, 40, 30);
            Female = false;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x8405;
            HairItemID = 0x203B;
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new JesterHat() { Hue = 0x482 });
            AddItem(new JesterSuit() { Hue = 0x47E });
            AddItem(new JesterShoes() { Hue = 0x48F });
            AddItem(new Cloak() { Hue = 0x5BE });
            AddItem(new LeatherGloves() { Hue = 0x59B });
            AddItem(new MagicWand());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.1)
                {
                    string[] lines = new string[]
                    {
                        "*Jasper ducks suddenly* 'Woah! That one nearly clipped my ear!'",
                        "'They said enchantments were fun... this is NOT fun!'",
                        "*He nervously juggles mid-walk* 'Calm down, please! Not the flaming one again!'",
                        "'Do you think the mage at Britain Inn can handle... disobedient balls?'",
                        "*He points at one floating nearby* 'See that? It’s watching us!'",
                        "'Please, if we make it out, drinks are on me... if I’m not in bandages!'",
                        "*Jasper mutters* 'No more enchanted props... next time, just scarves... scarves don’t bite.'"
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
