using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ControlTheFlameQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Control the FlameController"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself in the study of *Mirra Spellbinder*, Arcane Scholar of West Montor.\n\n" +
                    "Surrounded by glowing glyphs and flickering scrolls, she turns to you with urgency in her eyes.\n\n" +
                    "“The **FlameController** must be stopped. Its meddling with the ley lines threatens to twist the very foundation of our magic.”\n\n" +
                    "“I’ve tracked the distortions to the **Gate of Hell**. Each pulse of its power bends the ley further toward chaos. If left unchecked, our lands—our lives—could be consumed by unrelenting fire.”\n\n" +
                    "“I’ve exchanged runic formulas with the old scholars of *Malidor Academy*. We agree: this creature is not merely a guardian, but a corruptor.”\n\n" +
                    "**Slay the FlameController** before its influence spreads beyond control.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then watch the skies, for they may soon burn with the price of inaction.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still the ley lines writhe... you must act before the damage is irreversible.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You have severed the corruption. The ley lines breathe easier now.\n\n" +
                       "Take this, *StoneHead*. A relic, inert but potent. Use it wisely, and may the flame now serve rather than consume.";
            }
        }

        public ControlTheFlameQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FlameController), "FlameController", 1));
            AddReward(new BaseReward(typeof(StoneHead), 1, "StoneHead"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Control the FlameController'!");
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

    public class MirraSpellbinder : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ControlTheFlameQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter());
        }

        [Constructable]
        public MirraSpellbinder()
            : base("the Arcane Scholar", "Mirra Spellbinder")
        {
        }

        public MirraSpellbinder(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 90);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2049; // Long Hair
            HairHue = 1153; // Mystical violet
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 2213, Name = "Enigma Robe" }); // Deep arcane blue
            AddItem(new WizardsHat() { Hue = 1157, Name = "Hat of Ley Vision" }); // Glowing teal
            AddItem(new Sandals() { Hue = 2101, Name = "Ash-Woven Sandals" });
            AddItem(new BodySash() { Hue = 2425, Name = "Rune-Wrapped Sash" });
            AddItem(new GnarledStaff() { Hue = 1109, Name = "Staff of Ley Binding" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Scroll Satchel";
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
