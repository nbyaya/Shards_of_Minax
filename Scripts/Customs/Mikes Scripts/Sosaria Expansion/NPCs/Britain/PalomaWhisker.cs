using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class NineLivesEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Nine Lives End"; } }

        public override object Description
        {
            get
            {
                return
                    "Paloma Whisker, Castle British's Feline Tamer, stands with a poised grace, her eyes gleaming with feline cunning.\n\n" +
                    "She adjusts her gloves—padded with cushioned claws—before addressing you:\n\n" +
                    "“The VaultKitty—yes, that impish terror—has taken its chaos to the Preservation Vault 44. My darlings were always clever, but this one? **This one has defied death more times than I can count.**”\n\n" +
                    "“It mocks the traps, pounces from nowhere, and my dear vault friends? They can barely keep up with the *clawful* havoc it’s wreaking.”\n\n" +
                    "“I raised it once, you know. Tamed it from the wild sands of Moon. But something in that Vault... **changed** it.”\n\n" +
                    "“End its rampage. End its nine lives. And I’ll share with you something... star-bound.”\n\n" +
                    "**Slay the VaultKitty** in Preservation Vault 44.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "You... refuse? Then may the Vault staff find solace in their scratched arms and broken pride.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still leaping? Still laughing in the dark? Please—don’t let it reach a tenth life.";
            }
        }

        public override object Complete
        {
            get
            {
                return "Gone? Truly gone? Oh, my poor mischievous darling... You’ve done well, friend. Take this: **StarforgedVow**.\n\n" +
                       "It shines with the promise of loyalty—a promise I once gave to the VaultKitty. May it serve you better than it served me.";
            }
        }

        public NineLivesEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(VaultKitty), "VaultKitty", 1));
            AddReward(new BaseReward(typeof(StarforgedVow), 1, "StarforgedVow"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Nine Lives End'!");
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

    public class PalomaWhisker : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(NineLivesEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        [Constructable]
        public PalomaWhisker()
            : base("the Feline Tamer", "Paloma Whisker")
        {
        }

        public PalomaWhisker(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 25);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Light skin tone
            HairItemID = 0x2047; // Long Hair
            HairHue = 1157; // Deep purple
            NameHue = 1378;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1153, Name = "Whisker’s Softweave Blouse" }); // Soft lavender
            AddItem(new Skirt() { Hue = 2117, Name = "Vaultshadow Skirt" }); // Midnight blue
            AddItem(new Cloak() { Hue = 1150, Name = "Moonlit Cloak" }); // Silvery-blue shimmer
            AddItem(new Sandals() { Hue = 1154, Name = "Silent Paws" }); // Light grey
            AddItem(new LeatherGloves() { Hue = 1175, Name = "Cushioned Claws" }); // Deep blue leather with claw-like tips
            AddItem(new BearMask() { Hue = 1170, Name = "Feline Visage Mask" }); // Styled like a cat’s face, silvered

            Backpack backpack = new Backpack();
            backpack.Hue = 1137;
            backpack.Name = "Whisker's Satchel";
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
