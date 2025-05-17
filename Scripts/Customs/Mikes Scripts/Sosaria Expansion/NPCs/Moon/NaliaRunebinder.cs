using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ScriptedDoomQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Scripted Doom"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Nalia Runebinder*, Glyph Master of Moon’s Celestial Order.\n\n" +
                    "“Our star charts are unraveling… a vile sorcerer, the *Hieroglyph Mage*, inscribes curses atop the sacred glyphs I crafted to shield us.\n\n" +
                    "These scarabs he sends—each carries a fragment of doom, etched in ancient script. If his work is not halted, the skies themselves will betray us.”\n\n" +
                    "**Slay the Hieroglyph Mage**, and cleanse his cursed inscriptions.";
            }
        }

        public override object Refuse { get { return "Then doom will continue to script itself upon our world... may the Moon have mercy."; } }

        public override object Uncomplete { get { return "The Mage still lives? His glyphs twist deeper into our fates. You must act swiftly."; } }

        public override object Complete { get { return "The inscriptions fade... my runes breathe once more. Take this, woven from the thorns of fate itself."; } }

        public ScriptedDoomQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(HieroglyphMage), "Hieroglyph Mage", 1));

            AddReward(new BaseReward(typeof(ThornwovenBracers), 1, "ThornwovenBracers"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Scripted Doom'!");
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

    public class NaliaRunebinder : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ScriptedDoomQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHelmetArmor()); 
        }

        [Constructable]
        public NaliaRunebinder()
            : base("the Glyph Master", "Nalia Runebinder")
        {
        }

        public NaliaRunebinder(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 50);

            Female = true;
            Body = 0x191; // Female Body
            Race = Race.Human;

            Hue = Race.RandomSkinHue(); // Natural skin tone
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Deep sapphire hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1157, Name = "Runebound Dress" }); // Midnight purple
            AddItem(new Cloak() { Hue = 1150, Name = "Starveil Cloak" }); // Pale silver
            AddItem(new Sandals() { Hue = 1153, Name = "Glyphwoven Sandals" }); // Deep cosmic blue
            AddItem(new WizardsHat() { Hue = 1154, Name = "Cowl of Celestial Sight" }); // Soft grey
            AddItem(new SpellWeaversWand() { Hue = 2406, Name = "Wand of Lunar Scripts" }); // Softly glowing wood
            Backpack backpack = new Backpack();
            backpack.Hue = 38;
            backpack.Name = "Runebinder's Pack";
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
