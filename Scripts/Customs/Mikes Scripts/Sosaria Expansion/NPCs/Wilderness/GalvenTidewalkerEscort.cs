using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GalvenTidewalkerQuest : BaseQuest
    {
        public override object Title { get { return "The Weight of the Deep"; } }

        public override object Description
        {
            get
            {
                return
                    "*Galven's eyes, dark as stormy seas, lock with yours, heavy with fear and resolve.*\n\n" +
                    "I am Galven Tidewalker, shipwright of Renika. Beneath the waves, I found something I should not have—an artifact that sings in my mind, calls me to the depths. I fled, but its song grows louder. I must return to Renika before it claims me. Will you guide me home... before I lose myself forever?";
            }
        }

        public override object Refuse { get { return "*Galven shudders, looking toward the horizon.* Then may the sea forgive me for what I’ve awoken."; } }
        public override object Uncomplete { get { return "*Galven clutches his head.* The song... it's closer now. We must move."; } }

        public GalvenTidewalkerQuest() : base()
        {
            AddObjective(new EscortObjective("the town of Renika"));
            AddReward(new BaseReward(typeof(Grimmblade), "Grimmblade – A cursed sword thirsting for justice or vengeance, its true power revealed in battle."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Galven bows deeply, his voice distant.* Thank you. The sea may yet release me. Take this blade—I forged it in madness, but wielded in truth, it may serve you better.", null, 0x59B);
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

    public class GalvenTidewalkerEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(GalvenTidewalkerQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBShipwright(this));
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public GalvenTidewalkerEscort() : base()
        {
            Name = "Galven Tidewalker";
            Title = "the Shipwright";
            NameHue = 0x482;
        }

		public GalvenTidewalkerEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1020; // Sun-kissed sailor skin tone
            HairItemID = 0x2048; // Curly long hair
            HairHue = 1151; // Deep ocean blue
            FacialHairItemID = 0x203B; // Mustache + beard
            FacialHairHue = 1151;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 136, Name = "Stormwoven Shirt" }); // Dark teal
            AddItem(new LongPants() { Hue = 1109, Name = "Salt-Stained Trousers" }); // Sea-worn grey
            AddItem(new Boots() { Hue = 1175, Name = "Mariner's Boots" }); // Washed-out brown
            AddItem(new Cloak() { Hue = 1260, Name = "Cloak of the Abyssal Current" }); // Iridescent navy
            AddItem(new Bandana() { Hue = 1157, Name = "Tidewalker's Wrap" }); // Midnight indigo
            AddItem(new LeatherGloves() { Hue = 1150, Name = "Deckhand's Grasp" }); // Weathered black

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Tidewalker’s Satchel";
            AddItem(backpack);

            AddItem(new Cutlass() { Hue = 2053, Name = "Barnacle-Crusted Cutlass" }); // Rusted green
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
                        "*Galven grips his head.* The sea still calls... I hear its song in my bones.",
                        "*His eyes glaze for a moment.* Below, it waits... the thing I should not have seen.",
                        "*Galven's voice shakes.* Renika is close, I can feel it... but so is it.",
                        "*He looks at you desperately.* Promise me, if I fall, burn the relic... do not let it return.",
                        "*A chill breeze passes.* It knows I run. It pulls at me still.",
                        "*Galven hums a sailor's dirge, low and mournful.* The deep claims all in time... but not today.",
                        "*Galven clutches his cloak.* The abyss watches, patient and eternal."
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
