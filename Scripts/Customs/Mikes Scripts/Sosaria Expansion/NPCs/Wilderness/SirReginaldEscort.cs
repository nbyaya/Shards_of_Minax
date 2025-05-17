using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SirReginaldQuest : BaseQuest
    {
        public override object Title { get { return "Rest for the Forgotten Knight"; } }

        public override object Description
        {
            get
            {
                return 
                    "*The spectral form of Sir Reginald flickers before you, his voice a hollow echo of past glories.*\n\n" +
                    "\"I am Sir Reginald... once the King's Shield, now but a shadow. Betrayed in life, denied burial in my rightful tomb... I wander still. Will you guide me to the King's Chamber, that I might know peace? My sword, forged to slay the restless dead, shall be yours if you grant me this mercy.\"";
            }
        }

        public override object Refuse { get { return "*Sir Reginald's form dims, his voice tinged with sorrow.* \"Then I shall remain in the dark... forgotten still.\""; } }
        public override object Uncomplete { get { return "*The knight's gaze sharpens.* \"We are close... I feel the pull of my rest. Onward, brave one.\""; } }

        public SirReginaldQuest() : base()
        {
            AddObjective(new EscortObjective("the Kings Chamber"));
            AddReward(new BaseReward(typeof(GhoulSlayersLongsword), "GhoulSlayersLongsword – Glows in darkness, strikes true against the undead."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Sir Reginald bows, spectral light fading from his eyes.* \"You have done a great deed, champion. Take my blade, and may it strike down the darkness that plagues this world. I am free.\"");
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

    public class SirReginaldEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(SirReginaldQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBKeeperOfChivalry());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public SirReginaldEscort() : base()
        {
            Name = "Sir Reginald";
            Title = "the Forgotten";
            NameHue = 0x480;
        }

		public SirReginaldEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 75, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1150; // Ghostly pallor
            HairItemID = 0x2048; // Short hair
            HairHue = 1150; // Faded silver
            FacialHairItemID = 0x2041; // Beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 1151, Name = "Reginald's Ghostplate" }); // Pale ethereal hue
            AddItem(new PlateArms() { Hue = 1151, Name = "Spectral Vambraces" });
            AddItem(new PlateGloves() { Hue = 1151, Name = "Gauntlets of Restless Honor" });
            AddItem(new PlateLegs() { Hue = 1151, Name = "Greaves of the Betrayed" });
            AddItem(new CloseHelm() { Hue = 1151, Name = "Helm of the Lost Knight" });
            AddItem(new Cloak() { Hue = 1153, Name = "Shroud of the Forgotten Order" }); // Faded royal blue
            AddItem(new Boots() { Hue = 1175, Name = "Silent March Boots" });

            AddItem(new Broadsword() { Hue = 1102, Name = "Wraithfang" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Spectral Reliquary";
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
                        "*Sir Reginald gazes at the horizon.* Long have I waited... soon, my watch ends.",
                        "*The knight’s voice quivers.* The King's Chamber... I can almost see its golden seal.",
                        "*A spectral mist rises briefly.* Betrayal is a heavy chain. You help to lift it.",
                        "*Reginald grips his blade.* My sword knows the taste of the undead... it shall serve you well.",
                        "*He sighs, the sound like wind through hollow halls.* Honor... not forgotten, not yet.",
                        "*A flicker of light surrounds him.* I stood by the King once... and now, by you.",
                        "*His voice grows distant.* The castle remembers me... though its halls have changed."
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
