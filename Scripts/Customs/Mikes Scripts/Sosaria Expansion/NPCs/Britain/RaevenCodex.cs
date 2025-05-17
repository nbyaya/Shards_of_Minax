using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ProtocolOverrideQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Protocol Override"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Raeven Codex*, Automaton Engineer of Castle British.\n\n" +
                    "Her eyes are sharp, scanning your form like a schematic. She adjusts her cloak of shimmering bronze thread, voice clipped and precise.\n\n" +
                    "“A breach has occurred in **Preservation Vault 44**. My maintenance logs flagged an emergent threat: **ProtocolDragonX**. It was a failsafe... a construct designed to maintain order.”\n\n" +
                    "“Something went wrong. Its firmware has corrupted, overridden its core directives. It now commands Vault constructs, twisting them with errant code. If it seizes full control, the Vault’s knowledge—and power—will be lost... or worse, weaponized.”\n\n" +
                    "She taps her *Decoding Staff*, arcs of light spiraling from its tip.\n\n" +
                    "“I cannot face it. My tools are for analysis, not battle. But you—you can **terminate ProtocolDragonX**. Disable it before the Vault falls. Bring me proof, and I will share a relic encoded in the Vault's very heart.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the Vault's silence will echo louder, until its secrets are lost to chaos. I pray you'll reconsider.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "ProtocolDragonX still persists? My instruments flicker with warning. Each moment gives it more command lines.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The signals are clear now... you've done it. The Vault stabilizes.\n\n" +
                       "This *HeartwoodCurio* was encoded in the Vault's deepest layers. Let it remind you: code, like destiny, can be rewritten by the brave.";
            }
        }

        public ProtocolOverrideQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ProtocolDragonX), "ProtocolDragonX", 1));
            AddReward(new BaseReward(typeof(HeartwoodCurio), 1, "HeartwoodCurio"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Protocol Override'!");
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

    public class RaevenCodex : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ProtocolOverrideQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard()); // Closest profession for Automaton Engineer
        }

        [Constructable]
        public RaevenCodex()
            : base("the Automaton Engineer", "Raeven Codex")
        {
        }

        public RaevenCodex(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale metallic skin hue
            HairItemID = 0x203C; // Long Hair
            HairHue = 1153; // Silver-blue
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 2425, Name = "Engineer’s Tunic" }); // Polished bronze
            AddItem(new StuddedLegs() { Hue = 2301, Name = "Circuitwoven Greaves" }); // Dark iron
            AddItem(new LeatherGloves() { Hue = 2500, Name = "Calibration Gloves" }); // Light gold
            AddItem(new WizardsHat() { Hue = 2418, Name = "Resonance Cap" }); // Glimmering brass
            AddItem(new Cloak() { Hue = 2419, Name = "Electrum-thread Cloak" }); // Sparkling electrum
            AddItem(new Sandals() { Hue = 2403, Name = "Grounded Soles" }); // Bronze-brown

            AddItem(new MysticStaff() { Hue = 1260, Name = "Decoding Staff" }); // Crackling blue staff
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
