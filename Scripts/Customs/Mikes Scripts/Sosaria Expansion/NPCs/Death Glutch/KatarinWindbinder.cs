using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ElementalExileQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Elemental Exile"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Katarin Windbinder*, Elementalist of Death Glutch, her hands crackling faintly with barely contained elemental energy.\n\n" +
                    "Her robe flickers with hues of storm and flame, eyes tired but fierce, clutching a fragment of shimmering crystal.\n\n" +
                    "\"This shard... once a core of purity, now tainted. A **SpellElemental** has turned rogue, twisting the very air in Malidor's forsaken halls. It mocks me, warping my art.\"\n\n" +
                    "\"I need it gone—banished, unmade. But I cannot risk direct confrontation. My hold is fragile, and the academy walls remember too much pain.\"\n\n" +
                    "**Find the SpellElemental** deep within the **Malidor Witches Academy**, and banish it. Take this fragment—it will weaken it, if only slightly. Bring me peace, and I shall reward you well.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware the winds, traveler. The air grows thin, the academy restless.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still stirs, doesn't it? My control slips more with each breath. End it, I beg you.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You have done it. I feel the winds clear, the skies calm.\n\n" +
                       "The SpellElemental is no more, and I am whole again. You’ve my gratitude—and this: *CreepersLeatherCap*. Woven with care, may it shelter your mind from the chaos you faced.";
            }
        }

        public ElementalExileQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SpellElemental), "SpellElemental", 1));
            AddReward(new BaseReward(typeof(CreepersLeatherCap), 1, "CreepersLeatherCap"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Elemental Exile'!");
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

    public class KatarinWindbinder : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ElementalExileQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage());
        }

        [Constructable]
        public KatarinWindbinder()
            : base("the Elementalist", "Katarin Windbinder")
        {
        }

        public KatarinWindbinder(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 100, 85);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale, storm-like
            HairItemID = 0x203B; // Long hair
            HairHue = 1150; // Silvery-blue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1154, Name = "Storm-Woven Robe" }); // Deep sky blue
            AddItem(new WizardsHat() { Hue = 1161, Name = "Flame-Touched Hat" }); // Fiery red
            AddItem(new Sandals() { Hue = 1109, Name = "Grounded Sandals" }); // Ash gray
            AddItem(new BodySash() { Hue = 1175, Name = "Windbinder's Sash" }); // Ethereal green

            AddItem(new SpellWeaversWand() { Hue = 1171, Name = "Core-Focused Wand" }); // Light energy glow

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Elementalist’s Pack";
            AddItem(backpack);
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
