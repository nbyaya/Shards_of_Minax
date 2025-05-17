using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MiloTheSleeperQuest : BaseQuest
    {
        public override object Title { get { return "Dreams Within Dreams"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Milo’s eyes seem distant, flickering with the glow of a thousand stars.*\n\n" +
                    "\"I am Milo the Sleeper. Or… a part of him. My true form rests in the Etherial Inn, caught between dreams and waking. I wandered too far, and now I fear I cannot return alone. If I do not reach the Inn soon, both I and the dreams I hold may unravel. Will you walk with me, through the veil?\"";
            }
        }

        public override object Refuse { get { return "*Milo’s form flickers slightly.* \"Then the dreams must fade alone.\""; } }
        public override object Uncomplete { get { return "*Milo grips your arm, eyes wide.* \"The threads unravel… we must hurry.\""; } }

        public MiloTheSleeperQuest() : base()
        {
            AddObjective(new EscortObjective("Etherial Inn"));
            AddReward(new BaseReward(typeof(MasterShrubbery), "MasterShrubbery – A decorative item with secret regenerative properties."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Milo’s form stabilizes as he smiles faintly.* \"Thank you, dreamer. You have saved more than just me. Take this, a gift from beyond the veil. It remembers, even when we do not.\"", null, 0x59B);
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

    public class MiloTheSleeperEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(MiloTheSleeperQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFortuneTeller());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public MiloTheSleeperEscort() : base()
        {
            Name = "Milo";
            Title = "the Sleeper";
            NameHue = 0x47E; // Dreamy blue hue
        }

		public MiloTheSleeperEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(50, 40, 25);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1150; // Pale, dreamlike skin tone
            HairItemID = 0x2049; // Curly hair
            HairHue = 1153; // Silvery blue
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1153, Name = "Dreamweaver’s Shroud" }); // Ethereal blue
            AddItem(new Sandals() { Hue = 1175, Name = "Veilwalker's Steps" }); // Iridescent silver
            AddItem(new BodySash() { Hue = 1150, Name = "Sash of the Slumbering Mind" }); // Soft moonlight
            AddItem(new WizardsHat() { Hue = 1154, Name = "Hat of Restful Stars" }); // Starry indigo
            AddItem(new SpellWeaversWand() { Hue = 1170, Name = "Lullaby Rod" }); // Dream-hued staff

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Dreamer's Satchel";
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
                        "*Milo gazes into the distance.* The veil is thin here… can you feel it?",
                        "*His voice trembles.* I hear echoes of myself… they call from beyond the dream.",
                        "*He sighs softly.* The Inn… it waits. But the path is never straight.",
                        "*Milo grips his wand.* If I vanish, remember: the dreamer wakes elsewhere.",
                        "*A shimmer flickers across his form.* The waking world feels heavy… I must return.",
                        "*Milo hums an otherworldly tune.* It’s a song to lull the stars to sleep.",
                        "*He glances at you.* Do dreams have weight, or is it the weight of forgetting?"
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
