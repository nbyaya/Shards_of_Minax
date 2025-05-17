using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DrakefallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Drakefall"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Xavier Runeblood*, the enigmatic Enchanter of Death Glutch.\n\n" +
                    "His eyes flicker with violet light as he adjusts glowing runes on a hovering wardstone.\n\n" +
                    "“This town clings to stability through fragile threads of arcane balance. And now… a beast, a **SpellDrake**, draws upon my wards like a leech on a lifeline.”\n\n" +
                    "“The drake siphons more than power—it **distorts** it. Warps it. Soon, our protections will collapse, and the backlash could ignite Death Glutch itself.”\n\n" +
                    "“I’ve crafted a rune-laced net—its sigils should bind the drake’s wings, grounding its chaos. But I’m no hunter.”\n\n" +
                    "**Slay the SpellDrake** before its feeding fractures our world’s tether to order.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware the crackling air, friend. When the wards burst, even thought may betray you.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The drake still feeds. I can feel the wards thinning—hear the runes weep.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The surge… it quiets. The wards hold.\n\n" +
                       "Take this: *FunkyFashionChest*. It holds more than garb—it holds my gratitude, woven into every thread.\n\n" +
                       "Perhaps you’ll wear it in defiance of chaos, as I do.";
            }
        }

        public DrakefallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SpellDrake), "SpellDrake", 1));
            AddReward(new BaseReward(typeof(FunkyFashionChest), 1, "FunkyFashionChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Drakefall'!");
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

    public class XavierRuneblood : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DrakefallQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage()); // Closest match for an enchanter
        }

        [Constructable]
        public XavierRuneblood()
            : base("the Enchanter", "Xavier Runeblood")
        {
        }

        public XavierRuneblood(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 100, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1150; // Pale, almost ethereal skin
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Midnight blue
            FacialHairItemID = 0x2040; // Short beard
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1260, Name = "Void-Touched Robe" }); // Deep violet
            AddItem(new WizardsHat() { Hue = 1270, Name = "Runebound Hat" }); // Glimmering purple
            AddItem(new Sandals() { Hue = 1153, Name = "Arcane-Step Sandals" }); // Midnight blue
            AddItem(new BodySash() { Hue = 1157, Name = "Wardweaver's Sash" }); // Dark silver

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Rune-etched Pack";
            AddItem(backpack);

            AddItem(new MagicWand() { Hue = 1272, Name = "Runeblood Wand" }); // Pulsing violet wand
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
}
