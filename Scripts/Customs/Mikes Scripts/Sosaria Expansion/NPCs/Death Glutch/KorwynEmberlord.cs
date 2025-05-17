using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DemonsDebtQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Demon’s Debt"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Korwyn Emberlord*, the Infernal Scholar of Death Glutch.\n\n" +
                    "Clad in crimson robes etched with black runes, he stares into a flame that dances without fuel.\n\n" +
                    "He turns, revealing a hand **burned with a summoning rune**, still glowing faintly.\n\n" +
                    "“You feel that, don’t you? The tremor in the ley lines. A student's arrogance birthed a fiend in the halls of *Malidor Witches Academy*—a beast of magic and malice, tied to me by a bond I never sought.”\n\n" +
                    "“I study infernal pacts, but this… this *Magic Fiend* is an abomination. Its breath warps the weave, its presence a stain I cannot cleanse until it is slain.”\n\n" +
                    "“I would go myself, but the rune... it binds me. If I step near, it will *consume* me.”\n\n" +
                    "“But you... you can end this.”\n\n" +
                    "**Slay the Magic Fiend** and break the bond. Do this, and you shall wield what was once meant to destroy me.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the fiend grows stronger with each passing breath. If it devours another soul, the rune may no longer be content with just me.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lives? The rune burns hotter. I cannot hold it much longer. Hurry... or we are both undone.";
            }
        }

        public override object Complete
        {
            get
            {
                return "**The bond is broken.** The rune darkens, and Korwyn exhales deeply.\n\n" +
                       "“You have no idea what you’ve done. Or perhaps you do. Either way, take this: *Joan’s Divine Longsword*. It was meant to slay me, should the fiend prevail. Now it shall serve you, as it served fate.”";
            }
        }

        public DemonsDebtQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MagicFiend), "Magic Fiend", 1));
            AddReward(new BaseReward(typeof(JoansDivineLongsword), 1, "Joan’s Divine Longsword"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Demon’s Debt'!");
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

    public class KorwynEmberlord : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DemonsDebtQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBNecromancer());
        }

        [Constructable]
        public KorwynEmberlord()
            : base("the Infernal Scholar", "Korwyn Emberlord")
        {
        }

        public KorwynEmberlord(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1153; // Pale, cursed tone
            HairItemID = 0x2044; // Long hair
            HairHue = 1157; // Charcoal black
            FacialHairItemID = 0x203B; // Short beard
            FacialHairHue = 1157;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1109, Name = "Runebound Robe" }); // Deep crimson
            AddItem(new LeatherGloves() { Hue = 1154, Name = "Ashen Grips" }); // Smoke-grey
            AddItem(new Sandals() { Hue = 1175, Name = "Infernal Striders" }); // Ember red
            AddItem(new Cloak() { Hue = 1109, Name = "Shroud of Binding" }); // Deep crimson matching robe
            AddItem(new WizardsHat() { Hue = 1154, Name = "Cindercrest Hat" }); // Smoke-grey

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Tomebinder’s Pack";
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
