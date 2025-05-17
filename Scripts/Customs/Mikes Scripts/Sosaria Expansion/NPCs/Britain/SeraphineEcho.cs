using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ResonantTerminationQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Resonant Termination"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Seraphine Echo*, Soundwarden of Castle British. Her attire shimmers faintly, layered in silken tones that shift with every footstep, like ripples in the air.\n\n" +
                    "Her eyes are sharp, yet her voice carries a melodic, haunting quality:\n\n" +
                    "“The Preservation Vault hums. Its walls vibrate with unseen tension. The EchoWielder, a creature birthed from fractured resonance, now threatens to collapse the vault’s inner sanctum.”\n\n" +
                    "“I crafted the soundproofing seals... and now they shudder under its power. Each note it emits weakens our defenses.”\n\n" +
                    "“Lady Isobel has entrusted me with an enchanted tuning fork. It will lead you to the source of the discord.”\n\n" +
                    "**Find and silence the EchoWielder**—before the vault’s collapse becomes irreversible.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the vault hold—for now. But know this: sound travels... and it carries doom if left unchecked.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still the vault quakes? Its echoes grow louder, closer. If we do not act swiftly, there may be no vault left to protect.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The hum has faded. You’ve silenced the EchoWielder... and saved the vault from collapse.\n\n" +
                       "Take this: *CraftmothersHand.* A relic as old as the vault itself—imbued with the strength of creation and silence alike.";
            }
        }

        public ResonantTerminationQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(EchoWielder), "EchoWielder", 1));
            AddReward(new BaseReward(typeof(CraftmothersHand), 1, "CraftmothersHand"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Resonant Termination'!");
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

    public class SeraphineEcho : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ResonantTerminationQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBShipwright(this)); // She works with resonance, sound – fits closest as a bard/vendor of sound artifacts
        }

        [Constructable]
        public SeraphineEcho()
            : base("the Soundwarden", "Seraphine Echo")
        {
        }

        public SeraphineEcho(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1152; // Pale luminescent hue
            HairItemID = 0x2047; // Long Hair
            HairHue = 1150; // Silver-blue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1260, Name = "Resonant Robe" }); // Shimmering indigo
            AddItem(new Cloak() { Hue = 1153, Name = "Echoweave Cloak" }); // Soft silver
            AddItem(new WizardsHat() { Hue = 1109, Name = "Harmonic Cowl" }); // Dust-gray
            AddItem(new Sandals() { Hue = 1150, Name = "Silent Step Sandals" }); // Moonlight blue
            AddItem(new BodySash() { Hue = 1161, Name = "Soundbinder’s Sash" }); // Light violet

            AddItem(new MagicWand() { Hue = 1175, Name = "Tuning Fork of Isobel" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Soundwarden's Satchel";
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
