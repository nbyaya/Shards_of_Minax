using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ForgeFuryQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Forge Fury"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Cedric Ironborn*, Master Blacksmith of Castle British. His arms are thick with soot-streaked muscle, and his eyes glint like molten steel.\n\n" +
                    "“You smell it? That tang of scorched iron? That’s no normal forgefire—it’s the stench of ruin.”\n\n" +
                    "“Down in Preservation Vault 44, I installed a prototype forge vent—meant to draw heat from the old blast ducts, refine it. But a beast got in... a damned drake, part machine now. **FurnaceDrakeMK1** they call it, roosting in the ducts, turning my dream into a nightmare.”\n\n" +
                    "“It’s overloading the vents, threatening to blow the Vault’s core. I rigged extra water vents to cool things, but it’s not enough. I need you to put the beast down.”\n\n" +
                    "**Slay FurnaceDrakeMK1** before the Vault melts from within. Bring me peace, and I’ll forge you something worthy.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then leave me to my forge and my fears. But if the Vault blows, this whole city may burn with it.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The beast still rages? My forges weep. I can hear the ducts groan at night—each one sounding like a death knell.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve quenched the fire at its source? Praise the anvils! You’ve saved the Vault—and me.\n\n" +
                       "Take this: *FlameOfFinalJudgement*. I forged it under the old laws, tempered in my fear, now hardened by your valor.";
            }
        }

        public ForgeFuryQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FurnaceDrakeMK1), "FurnaceDrakeMK1", 1));
            AddReward(new BaseReward(typeof(FlameOfFinalJudgement), 1, "FlameOfFinalJudgement"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Forge Fury'!");
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

    public class CedricIronborn : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ForgeFuryQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith());
        }

        [Constructable]
        public CedricIronborn()
            : base("the Master Blacksmith", "Cedric Ironborn")
        {
        }

        public CedricIronborn(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Slightly tanned

            HairItemID = 0x2049; // Short Hair
            HairHue = 1109; // Ash-gray
            FacialHairItemID = 0x2041; // Full Beard
            FacialHairHue = 1109; // Ash-gray
        }

        public override void InitOutfit()
        {
            AddItem(new FullApron() { Hue = 1830, Name = "Ember-Streaked Apron" }); // Blackened leather with glowing seams
            AddItem(new StuddedGloves() { Hue = 2309, Name = "Forgeguard Gauntlets" }); // Dark steel
            AddItem(new LeatherDo() { Hue = 2424, Name = "Cinder-Singed Tunic" }); // Charred red
            AddItem(new LeatherLegs() { Hue = 2423, Name = "Blast-Duct Greaves" }); // Ashen grey
            AddItem(new Boots() { Hue = 2307, Name = "Anvil-Walkers" }); // Black with a metallic shine
            AddItem(new SmithSmasher() { Hue = 1157, Name = "Ironborn’s Maul" }); // Dark hammer with runes

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Forge Pack";
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
