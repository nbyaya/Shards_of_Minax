using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RikkaRatkinQuest : BaseQuest
    {
        public override object Title { get { return "Shadows Beneath Britain"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Rikka narrows her eyes, tail twitching nervously.*\n\n" +
                    "\"So, you're not one of them? Good... I need out. The sewers ain't safe anymore—not for me. The Ratkin clans turned on me when I wouldn't sell out to the Thieves' Guild. There's a way out, but I can't risk it alone. Get me past their traps and claws, and I'll make it worth your while. Deal?\"";
            }
        }

        public override object Refuse { get { return "*Rikka spits to the side.* \"Suit yourself. Guess I'll take my chances with the rats.\""; } }
        public override object Uncomplete { get { return "*Rikka snarls.* \"Keep movin', or you'll end up as bait like the rest.\""; } }

        public RikkaRatkinQuest() : base()
        {
            AddObjective(new EscortObjective("the Sewers under Britain"));
            AddReward(new BaseReward(typeof(RoguesShadowCloak), "RoguesShadowCloak – Enhances agility and evasion."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Rikka grins, baring sharp teeth.* \"You did good, better than most. Here, take this cloak. Wove it meself from shadows and silk. It'll serve ya well... if you stay quick.\"", null, 0x497);
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

    public class RikkaRatkinEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(RikkaRatkinQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBThief());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public RikkaRatkinEscort() : base()
        {
            Name = "Rikka";
            Title = "the Ratkin Outcast";
            NameHue = 0x83F8;
        }

		public RikkaRatkinEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Slightly pale/greyish
            HairItemID = 0x203B; // Short hair
            HairHue = 1154; // Dark grey
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherNinjaJacket() { Hue = 2101, Name = "Ratkin Hidecoat" }); // Shadowy brown
            AddItem(new LeatherNinjaPants() { Hue = 2106, Name = "Stitchrat Leggings" }); // Worn black
            AddItem(new ClothNinjaHood() { Hue = 2406, Name = "Sewerveil Hood" }); // Deep grey
            AddItem(new NinjaTabi() { Hue = 2117, Name = "Silentfoot Tabi" }); // Black
            AddItem(new BodySash() { Hue = 2212, Name = "Rat's Tail Sash" }); // Dark crimson
            AddItem(new ShadowSai() { Hue = 1175, Name = "Rikka’s Shiv" }); // Polished bone

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Pack of Pilfered Maps";
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
                        "*Rikka sniffs the air.* The sewers ain’t safe… never were, really.",
                        "*She mutters.* I know a way out, but they’re watchin’. Always watchin’.",
                        "*Her tail flicks.* You hear that? That’s them… claws in the dark.",
                        "*Rikka chuckles darkly.* If we make it, maybe I’ll tell ya why they really want me dead.",
                        "*She clutches her sash.* This? Belonged to my brother. Didn’t run fast enough.",
                        "*Her eyes gleam.* They think I’m weak… they’ll learn."
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
