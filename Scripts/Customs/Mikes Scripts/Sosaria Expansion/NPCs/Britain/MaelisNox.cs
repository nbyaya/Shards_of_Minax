using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class PollutedBreathQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Polluted Breath"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Maelis Nox*, Mistwalker and sentinel of Castle British’s deeper halls.\n\n" +
                    "Veiled in a faint shimmer of warding mist, she removes a sleek obsidian mask to address you, her eyes glowing faintly with moonlit silver.\n\n" +
                    "“The vault breathes... but now it chokes.”\n\n" +
                    "“During my nightly patrol, the crystals turned black. My mask tasted poison in the air—the work of the **MiasmaResiduum**, polluting the vents of *Preservation Vault 44*.”\n\n" +
                    "“If left unchecked, this blight could seep beyond the vault and corrupt Castle British itself. **You must descend**. Seek the source. End the Residuum’s breath before the fumes claim us all.”\n\n" +
                    "“I will reward you with something the vault itself entrusted to me—a gift that dances in the crescent light.”\n\n" +
                    "**Slay the MiasmaResiduum**, and restore the vault’s air to purity.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“Then watch the castle’s breath darken... and pray you do not feel the weight of poisoned dreams.”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“The vault still gasps in pain. The air thickens... shadows grow restless.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "“The vault sighs once more... in relief.”\n\n" +
                       "“You have slain the source of corruption. Take this: *DancersCrescent*. It carries the purity of night air and the sharpness of twilight blades. Let it guide your steps as surely as you guided mine.”";
            }
        }

        public PollutedBreathQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MiasmaResiduum), "MiasmaResiduum", 1));
            AddReward(new BaseReward(typeof(DancersCrescent), 1, "DancersCrescent"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Polluted Breath'!");
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

    public class MaelisNox : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(PollutedBreathQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic());
        }

        [Constructable]
        public MaelisNox()
            : base("the Mistwalker", "Maelis Nox")
        {
        }

        public MaelisNox(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 90, 90);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale, moonlit hue
            HairItemID = 0x2048; // Long hair
            HairHue = 2101; // Dark, midnight blue
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1154, Name = "Veil of the Mistwalker" }); // Ethereal blue
            AddItem(new ClothNinjaHood() { Hue = 2101, Name = "Twilight Hood" });
            AddItem(new Sandals() { Hue = 1175, Name = "Mist-Touched Sandals" });
            AddItem(new BodySash() { Hue = 1157, Name = "Sash of Purified Breath" });

            AddItem(new SpellWeaversWand() { Hue = 1260, Name = "Crystalline Fume Staff" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Mistwalker’s Satchel";
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
