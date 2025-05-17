using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MortimerGravesQuest : BaseQuest
    {
        public override object Title { get { return "From Depths to Dust"; } }

        public override object Description
        {
            get
            {
                return
                    "*Mortimer’s voice echoes softly, eyes wide with fear yet oddly resolute.*\n\n" +
                    "\"The dead should rest above, not below. I went too deep... the old vaults under Britain Cemetery—they whispered, called me closer. But now I am lost, and they would keep me. Will you guide me back before they make me one of theirs? I must return... or be claimed forever.\"";
            }
        }

        public override object Refuse { get { return "*Mortimer’s shoulders slump.* \"Then I am but one more lost beneath the earth...\""; } }
        public override object Uncomplete { get { return "*His voice trembles.* \"The air thickens. We must press on, or I will not last.\""; } }

        public MortimerGravesQuest() : base()
        {
            AddObjective(new EscortObjective("the Caverns under the Britain Cemetery"));
            AddReward(new BaseReward(typeof(EvilCandle), "EvilCandle – A cursed candle that reveals hidden doors, but lures dark things."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Mortimer breathes in deeply as light returns to his face.* \"You’ve given me life again, stranger. This candle... take it. It shows what hides in shadow, but beware—light draws more than sight.\"");
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

    public class MortimerGravesEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(MortimerGravesQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMonk());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public MortimerGravesEscort() : base()
        {
            Name = "Mortimer Graves";
            Title = "the Wandering Undertaker";
            NameHue = 0x455;
        }
		
		public MortimerGravesEscort(Serial serial) : base(serial) { }
        
		public override void InitBody()
        {
            InitStats(50, 60, 20);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1002; // Ashen complexion
            HairItemID = 0x203C; // Longish, disheveled
            HairHue = 1109; // Dusty grey
            FacialHairItemID = 0x203F; // Short beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1151, Name = "Mortimer's Mourning Robe" }); // Shadowed black
            AddItem(new LeatherGloves() { Hue = 1107, Name = "Gravedigger's Grasp" }); // Faded brown
            AddItem(new Sandals() { Hue = 1175, Name = "Silent Step Sandals" }); // Deep shadow
            AddItem(new Cloak() { Hue = 1154, Name = "Cloak of Final Rites" }); // Dusty grey-blue
            AddItem(new SkullCap() { Hue = 1151, Name = "Undertaker's Cap" }); // Matches robe

            AddItem(new Scythe() { Hue = 1102, Name = "Gravewarden's Scythe" }); // Bone-iron colored

            Backpack pack = new Backpack();
            pack.Hue = 1108;
            pack.Name = "Undertaker's Satchel";
            AddItem(pack);
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
                        "*Mortimer shivers.* The air... it listens, it waits.",
                        "*A whisper echoes faintly.* They stir when we walk too loud.",
                        "*Mortimer grips his scythe.* I dug too deep, I saw too much.",
                        "*His voice cracks.* Don’t let them take me... not yet.",
                        "*You feel a chill.* This place remembers the dead... and the living.",
                        "*Mortimer murmurs.* I left lanterns behind... but none stayed lit.",
                        "*Mortimer gazes upward.* Light... I must see the light once more."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 40));
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
