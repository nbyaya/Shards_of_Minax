using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class KnightfallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Knightfall"; } }

        public override object Description
        {
            get
            {
                return
                    "Thessa Bonebind, mortician of Devil Guard, greets you with solemn eyes, her hands stained with ash.\n\n" +
                    "“I am the keeper of our fallen, guardian of their memory. But something has gone terribly wrong.”\n\n" +
                    "“Among the relics of our dead, a suit of armor—*the GloomKnight’s plate*—has begun to move on its own. A spirit, lost and furious, has risen in the Mines of Minax.”\n\n" +
                    "“Slay this tormented soul. Reclaim its blade, and let me lay it to rest beside its kin. Only then can honor return to the guard it once served.”\n\n" +
                    "**Slay the GloomKnight** and return its cursed blade.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the restless dead haunt us both, until another takes up this charge.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The GloomKnight still roams, its blade still drips with shadow. Free it, before others fall.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The blade... it weeps.\n\n" +
                       "You’ve done more than kill a beast—you’ve **restored the honor of the fallen**.\n\n" +
                       "Take this, the *Hollowbeak Visage*. Wear it as a symbol of death faced—and death conquered.";
            }
        }

        public KnightfallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GloomKnight), "GloomKnight", 1));
            AddReward(new BaseReward(typeof(HollowbeakVisage), 1, "Hollowbeak Visage"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Knightfall'!");
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

    public class ThessaBonebind : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(KnightfallQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBLeatherArmor()); // For mortician's leather crafting & ceremonial gear
        }

        [Constructable]
        public ThessaBonebind()
            : base("the Mortician", "Thessa Bonebind")
        {
        }

        public ThessaBonebind(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1030; // Pale hue, ash-touched
            HairItemID = 0x203C; // Long hair
            HairHue = 1102; // Raven black
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1150, Name = "Ashen Deathrobe" }); // Soft gray/white
            AddItem(new LeatherGloves() { Hue = 2301, Name = "Binding Gloves" }); // Dusty brown
            AddItem(new HoodedShroudOfShadows() { Hue = 1175, Name = "Cowl of Remembrance" }); // Dark veil, nearly black
            AddItem(new Sandals() { Hue = 2301, Name = "Silent Steps" }); // Quiet brown
            AddItem(new BodySash() { Hue = 1157, Name = "Sash of the Fallen" }); // Light grayish blue

            AddItem(new VivisectionKnife() { Hue = 1109, Name = "Bonebinder’s Blade" }); // Shadow-touched surgical tool

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Mortician's Pack";
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
