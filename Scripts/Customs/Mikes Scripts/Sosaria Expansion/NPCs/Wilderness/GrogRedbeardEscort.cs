using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GrogRedbeardQuest : BaseQuest
    {
        public override object Title { get { return "A Barrel Too Heavy"; } }

        public override object Description
        {
            get
            {
                return
                    "Aye, ye look like someone with strong legs and a curious spirit. Name’s Grog Redbeard, and I’ve got one last errand to run before I hang up me boots. I’ve a barrel of cursed rum and a map that’ll lead me home... back to Pirate Isle. Trouble is, I can’t make the journey alone—old debts and darker things are watchin'. Escort me there, and I’ll see you get somethin’ worth more than gold.";
            }
        }

        public override object Refuse { get { return "Aye, I understand. It’s not an easy road, and the rum’s got a way of drawin’ the wrong kind of eyes."; } }
        public override object Uncomplete { get { return "We ain't there yet, matey. The sea calls, and so do me enemies."; } }

        public GrogRedbeardQuest() : base()
        {
            AddObjective(new EscortObjective("Pirate Isle"));
            AddReward(new BaseReward(typeof(GrungeBandana), "GrungeBandana – boosts intimidation and charm."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("Aye, thank ye! You’ve earned this, fair and square. Wear it with pride, and let ‘em know Grog Redbeard sends his regards.", null, 0x59B);
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

    public class GrogRedbeardEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(GrogRedbeardQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman()); // Grog sells rare fish tales and maps, linked to his pirate past.
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public GrogRedbeardEscort() : base()
        {
            Name = "Grog Redbeard";
            Title = "the Retired Pirate";
            NameHue = 0x22;
        }

		public GrogRedbeardEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 50);
            Female = false;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x8402;
            HairItemID = 0x203B;
            HairHue = 1157;
            FacialHairItemID = 0x204B;
            FacialHairHue = 1157;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 0x59B });
            AddItem(new ShortPants() { Hue = 0x455 });
            AddItem(new HalfApron() { Hue = 0x497 });
            AddItem(new Bandana() { Hue = 0x46F });
            AddItem(new ThighBoots() { Hue = 0x486 });
            AddItem(new Cutlass()); // His trusty old weapon.
            AddItem(new Backpack());

            // Unique cursed barrel as a flavor prop.
            AddItem(new Barrel() { Name = "Cursed Rum Barrel", Hue = 0x480 });
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
                        "*Grog adjusts the barrel on his back* 'Blasted thing gets heavier with each step...'",
                        "'You ever heard the rum talk? I have. Don't listen too close.'",
                        "'Pirate Isle's close now. Smell that salt? Smells like home... and trouble.'",
                        "*He chuckles darkly* 'They say the cursed never die. Guess we’ll find out, eh?'",
                        "'Map says we go east... but me gut says west. Ah, hell with it, follow the gut.'",
                        "*Grog peers at you* 'Tell me, friend... you ever wish you’d never set out on a path?'"
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
