using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class PhantomInTheMachineQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Phantom in the Machine"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Jareth Codewright*, the Aetherwright of Castle British.\n\n" +
                    "Wearing lenses that flicker with arcane runes, he meticulously adjusts glowing conduits on a floating holocube.\n\n" +
                    "“I’ve charted the flow of ether through **Preservation Vault 44**, but something—or *someone*—is disrupting the sequence.”\n\n" +
                    "“A specter, half-code, half-malice. It’s infiltrating the vault’s arcane conduits, corrupting data crystals, threatening to erase knowledge that predates even Castle British.”\n\n" +
                    "“I’ve isolated its interference to the lower levels. My runic patch can shield the conduits temporarily, but only if the core threat is removed.”\n\n" +
                    "**Slay the DigitalPhantom** that haunts the Vault and restore the sanctity of the archives.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "The phantom’s corruption grows. Without intervention, the vault’s memory will be lost... and perhaps more.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lingers? Then I shall recalibrate the patch—but know this, the longer it stays, the more it *learns* our patterns.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done? The vault readings are stabilizing...\n\n" +
                       "You’ve not just slain a phantom, you’ve preserved the **living memory of Sosaria**.\n\n" +
                       "Take this: *AncestorsGaze*. It will help you see truth through even the deepest distortions.";
            }
        }

        public PhantomInTheMachineQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DigitalPhantom), "DigitalPhantom", 1));
            AddReward(new BaseReward(typeof(AncestorsGaze), 1, "AncestorsGaze"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Phantom in the Machine'!");
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

    public class JarethCodewright : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(PhantomInTheMachineQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBArchitect());
        }

        [Constructable]
        public JarethCodewright()
            : base("the Aetherwright", "Jareth Codewright")
        {
        }

        public JarethCodewright(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 100, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1152; // Pale with a faint shimmer
            HairItemID = 0x203C; // Long Hair
            HairHue = 1150; // Silvery Blue
            FacialHairItemID = 0x204B; // Long Beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FormalShirt() { Hue = 1154, Name = "Runewoven Shirt" }); // Deep indigo
            AddItem(new ElvenPants() { Hue = 1175, Name = "Aetherbound Leggings" }); // Arcane green shimmer
            AddItem(new LeatherGloves() { Hue = 1157, Name = "Conduit Gloves" }); // Azure-etched
            AddItem(new WizardsHat() { Hue = 1171, Name = "Holo-Spire Cap" }); // Iridescent blue
            AddItem(new Cloak() { Hue = 2101, Name = "Cloak of Flickering Code" }); // Metallic grey with shifting patterns
            AddItem(new Sandals() { Hue = 2105, Name = "Vaultwalkers" }); // Pewter-tinted
            AddItem(new Scepter() { Hue = 1153, Name = "Dataflow Rod" }); // Silver with glowing nodes

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Patchwork Satchel";
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
