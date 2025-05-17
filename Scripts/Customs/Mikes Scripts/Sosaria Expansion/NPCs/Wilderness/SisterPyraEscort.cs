using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SisterPyraFlameboundQuest : BaseQuest
    {
        public override object Title { get { return "Return to the Fires"; } }

        public override object Description
        {
            get
            {
                return 
                    "*The air around Sister Pyra crackles with heat, her eyes burning with an inner fire.*\n\n" +
                    "\"I am Sister Pyra, once a humble priestess of flame. Now... cursed to burn within, my soul tethered to the Fires of Hell. The heat sustains me, yet I wandered too far in search of forbidden truths. If I do not return soon, I shall be consumed. Will you escort me back, before the fire takes me whole?\"";
            }
        }

        public override object Refuse { get { return "*She flinches, embers dancing on her skin.* \"Then let the flames claim me, and may they never touch you.\""; } }
        public override object Uncomplete { get { return "*Her voice weakens, smoke curling from her lips.* \"We must reach the flames... or I am lost.\""; } }

        public SisterPyraFlameboundQuest() : base()
        {
            AddObjective(new EscortObjective("the Fires Of Hell Dungeon"));
            AddReward(new BaseReward(typeof(AlamoDefendersAxe), "AlamoDefendersAxe – A heavy axe that boosts fire resistance and cleave damage."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Sister Pyra stands within the flames, at peace.* \"Thank you, flamebearer. The fire no longer torments me—it tempers me. Take this axe, forged in the inferno’s heart. May it shield you where fire reigns.\"", null, 0x489);
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

    public class SisterPyraEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(SisterPyraFlameboundQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHolyMage()); // As a fire priestess, this fits her mystic, elemental role
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public SisterPyraEscort() : base()
        {
            Name = "Sister Pyra Flamebound";
            Title = "the Firebound Priestess";
            NameHue = 0x489; // Fiery orange-red
        }

		public SisterPyraEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 60, 45);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 2932; // Lightly tanned, flame-touched skin
            HairItemID = 0x2049; // Braided hair
            HairHue = 1358; // Bright red
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1358, Name = "Flame-Touched Robe" }); // Bright flame-red robe
            AddItem(new LeatherGloves() { Hue = 1175, Name = "Cindershroud Gloves" }); // Ashen grey gloves
            AddItem(new Sandals() { Hue = 1259, Name = "Emberstep Sandals" }); // Glowing ember orange sandals
            AddItem(new Cloak() { Hue = 1367, Name = "Inferno Cloak" }); // Deep crimson cloak
            AddItem(new WizardsHat() { Hue = 1359, Name = "Crown of Embers" }); // Bright orange-red wizard hat

            AddItem(new GnarledStaff() { Hue = 1254, Name = "Scepter of Inner Flame" }); // Smoldering wooden staff

            Backpack backpack = new Backpack();
            backpack.Hue = 1175; // Ashen grey
            backpack.Name = "Satchel of Sacred Ashes";
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
                        "*Pyra clutches her chest, smoke curling from her fingertips.* The fire... it grows restless.",
                        "*A flicker of flame dances in her eyes.* I was chosen by the flame, but now it seeks to devour me.",
                        "*She winces, a heat haze blurring the air.* We are close... I can feel the fires call to me.",
                        "*Pyra's voice echoes with heat.* In the flames, there is truth... and torment.",
                        "*Her breath comes heavy, labored.* Every step away from the fire... is agony.",
                        "*She murmurs a chant.* Burn away the false... ignite the soul’s core."
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
