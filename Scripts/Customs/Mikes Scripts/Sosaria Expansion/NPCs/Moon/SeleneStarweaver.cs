using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class PharaohsHemoglobinQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Pharaoh’s Hemoglobin"; } }

        public override object Description
        {
            get
            {
                return
                    "Ink flows freely in the Archive, but now so does blood...\n\n" +
                    "I am Selene Starweaver, Lunar Archivist of Moon. Our scrolls speak softly of the Pharaoh, " +
                    "but now, they bleed his truth. Crimson taints the vellum, whispering forbidden hieroglyphs " +
                    "when struck by moonlight.\n\n" +
                    "**Slay the Blood of the Pharaoh**, lest his cursed essence defile the Archive forever.";
            }
        }

        public override object Refuse { get { return "Then beware the moonlit pages, for they will call to you."; } }

        public override object Uncomplete { get { return "The Pharaoh's blood still pulses through the sand. You must end it."; } }

        public override object Complete { get { return "The crimson flow ceases. The Archive breathes again. Take this—may it echo the silence you have restored."; } }

        public PharaohsHemoglobinQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BloodOfThePharaoh), "Blood of the Pharaoh", 1));

            AddReward(new BaseReward(typeof(VestmentOfEchoingRings), 1, "VestmentOfEchoingRings"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Pharaoh’s Hemoglobin'!");
            Owner.PlaySound(CompleteSound);
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

    public class SeleneStarweaver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(PharaohsHemoglobinQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGardener()); 
        }

        [Constructable]
        public SeleneStarweaver() : base("Selene Starweaver", "Lunar Archivist")
        {
            Title = "Lunar Archivist";
			Body = 0x191; // Male human
			Female = true;

            // Outfit - ethereal, mystical tones
            AddItem(new Robe { Hue = 1153, Name = "Moonshadow Robe" }); // Deep midnight blue
            AddItem(new Cloak { Hue = 1150, Name = "Veil of Stars" }); // Pale silver-blue cloak, shimmers faintly
            AddItem(new FeatheredHat { Hue = 1154, Name = "Archivist's Plume" }); // Elegant dark indigo with a silver feather
            AddItem(new Sandals { Hue = 2407, Name = "Whispering Steps" }); // Pale greyish white
            AddItem(new BodySash { Hue = 1157, Name = "Sash of Lunar Echoes" }); // Light lavender tint, symbolic runes
            AddItem(new SpellWeaversWand { Hue = 1155, Name = "Staff of Moonlit Insight" }); // Glows faintly under moonlight

            // Stats for lore flavor
            SetStr(50, 60);
            SetDex(60, 70);
            SetInt(120, 130);

            SetDamage(3, 5);
            SetHits(150, 160);
        }

        public SeleneStarweaver(Serial serial) : base(serial) { }

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
}
