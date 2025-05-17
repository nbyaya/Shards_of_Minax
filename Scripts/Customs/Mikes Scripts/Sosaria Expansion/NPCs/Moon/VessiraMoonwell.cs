using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BreathOfBlightQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Breath of Blight"; } }

        public override object Description
        {
            get
            {
                return
                    "You feel it too, don't you? The air shudders with sorrow.\n\n" +
                    "I am Vessira Moonwell, enchanter of wards and weaver of protective runes. " +
                    "But my craft falters... the western quarter of Moon is choked with cursed air—spirits drawn to despair and discord. " +
                    "Their breath poisons more than lungs; it seeps into magic itself.\n\n" +
                    "**Banish the Cursed Air** that haunt the western quarter of Moon. Restore the purity needed for my enchantments to hold.";
            }
        }

        public override object Refuse { get { return "Then may the blight twist your breath, as it has twisted theirs."; } }

        public override object Uncomplete { get { return "The air still trembles with corruption. They remain."; } }

        public override object Complete { get { return "The winds quieten... my wards hold once more. You have done well. Take this, a gift wrought from forgotten wails."; } }

        public BreathOfBlightQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CursedAir), "Cursed Air", 1));

            AddReward(new BaseReward(typeof(WailOfTheForgotten), 1, "WailOfTheForgotten"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Breath of Blight'!");
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

    public class VessiraMoonwell : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BreathOfBlightQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith()); 
        }

        [Constructable]
        public VessiraMoonwell() : base("Vessira Moonwell", "Enchanter of Moon")
        {
            Title = "Enchanter of Moon";
			Body = 0x191; // Male human
			Female = true;

            // Outfit
            AddItem(new HoodedShroudOfShadows { Hue = 1175, Name = "Shroud of Lunar Whispers" }); // Pale moonlight blue
            AddItem(new Sandals { Hue = 1153, Name = "Sands of Dusk" }); // Grey-blue shade
            AddItem(new BodySash { Hue = 1172, Name = "Sash of Celestial Binding" }); // Soft silver
            AddItem(new MagicWand { Hue = 1165, Name = "Wand of Aether Winds" }); // Slightly glowing, subtle aura

            SetStr(50, 60);
            SetDex(60, 70);
            SetInt(100, 110);

            SetDamage(3, 7);
            SetHits(150, 170);
        }

        public VessiraMoonwell(Serial serial) : base(serial) { }

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
