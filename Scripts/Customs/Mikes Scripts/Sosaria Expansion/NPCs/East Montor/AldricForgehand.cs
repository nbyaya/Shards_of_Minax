using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EbonysEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Ebony’s End"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Aldric Forgehand*, the stalwart Master Blacksmith of East Montor.\n\n" +
                    "His forge glows dimly, the air thick with ash and frustration. Tools lie untouched, and the anvil bears blackened scars.\n\n" +
                    "“I’ve hammered blades for kings and peasants alike... but none can withstand the blight from the **BlackDragon** nesting near the western forges.”\n\n" +
                    "“This beast’s smoke ruins steel, and its presence curses the mountain’s breath. My forge’s heart is dying.”\n\n" +
                    "“Long ago, my great-grandfather tamed a wyrmling of this same bloodline, but this one... it will not yield.”\n\n" +
                    "“Will you slay it? Cleanse the mountain’s fire? Do this, and I will gift you with a chest made not just with hands—but with honor.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then let the fires die and the mountain crumble... I can forge no more with ash in my veins.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lives? The air grows thick, the forge dimmer. Each day the mountain groans more heavily.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The beast is gone? Truly? Then the forge may yet burn bright again.\n\n" +
                       "Take this: the *SamuraiHonorChest*. Forged in tribute, sealed with the mountain’s restored breath.\n\n" +
                       "You’ve done what I could not—brought **Ebony’s End**.";
            }
        }

        public EbonysEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlackDragon), "BlackDragon", 1));
            AddReward(new BaseReward(typeof(SamuraiHonorChest), 1, "SamuraiHonorChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Ebony’s End'!");
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

    public class AldricForgehand : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(EbonysEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith());
        }

        [Constructable]
        public AldricForgehand()
            : base("the Master Blacksmith", "Aldric Forgehand")
        {
        }

        public AldricForgehand(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Soot-darkened skin
            HairItemID = 0x2047; // Long hair
            HairHue = 1109; // Ash-gray
            FacialHairItemID = 0x203B; // Thick beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 1825, Name = "Forge-Hardened Tunic" }); // Burnished iron
            AddItem(new StuddedLegs() { Hue = 1830, Name = "Ash-Worn Leggings" });
            AddItem(new PlateGloves() { Hue = 2407, Name = "Cindergrip Gauntlets" });
            AddItem(new LeatherCap() { Hue = 1823, Name = "Smith’s Embercap" });
            AddItem(new HalfApron() { Hue = 1835, Name = "Char-Marked Apron" });
            AddItem(new Boots() { Hue = 1102, Name = "Mountain Treaders" });

            AddItem(new SmithSmasher() { Hue = 2207, Name = "Forgehand’s Maul" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Forgemaster’s Pack";
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
