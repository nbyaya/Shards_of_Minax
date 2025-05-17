using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BladeOfTheDamnedQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Blade of the Damned"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Elara Moonbane*, her forge lit by the dull glow of cursed embers.\n\n" +
                    "Her hammer strikes the anvil with a rhythm heavy with sorrow, sparks dimmer than they should be.\n\n" +
                    "\"This forge used to craft blades for honor. Now, I fear every strike only fans the flames of old curses.\"\n\n" +
                    "\"One blade eludes me still—the **Blade of the Damned**. Wielded by a warrior twisted in the fall of Malidor’s halls, it lies within the armory. The blade sings, not with steel, but with the voices of the lost.\"\n\n" +
                    "\"I keep his helm here as a reminder. His face, long gone. His soul... still screams.\"\n\n" +
                    "**Slay the Cursed Malidor Warrior** in the academy’s depths, and bring silence to the forge once more.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then let the anvil ring hollow, and the damned blade remain unbroken.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The cursed blade still hums... and the forge grows colder with each strike.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve silenced the blade’s wail. My forge breathes again, though never without sorrow.\n\n" +
                       "Take this: *BakingBoard*. May your hands craft not for war, but for warmth.";
            }
        }

        public BladeOfTheDamnedQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CursedMalidorWarrior), "Cursed Malidor Warrior", 1));
            AddReward(new BaseReward(typeof(BakingBoard), 1, "BakingBoard"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Blade of the Damned'!");
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

    public class ElaraMoonbane : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BladeOfTheDamnedQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSwordWeapon());
        }

        [Constructable]
        public ElaraMoonbane()
            : base("the Cursed Smith", "Elara Moonbane")
        {
        }

        public ElaraMoonbane(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 100, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1157; // Midnight blue
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedBustierArms() { Hue = 1109, Name = "Moon-Tempered Bustier" }); // Dark steel
            AddItem(new StuddedHaidate() { Hue = 2306, Name = "Ashen War-Kilt" }); // Burnt gray
            AddItem(new LeatherGloves() { Hue = 2418, Name = "Forge-Hand Gauntlets" }); // Ember-burnished
            AddItem(new HoodedShroudOfShadows() { Hue = 1108, Name = "Shroud of the Fallen Flame" });
            AddItem(new Boots() { Hue = 1811, Name = "Cinder-Touched Boots" });
            AddItem(new SmithSmasher() { Hue = 1150, Name = "Moonbane's Hammer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Smith's Burden";
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
