using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RaptorFalloutQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Raptor Fallout"; } }

        public override object Description
        {
            get
            {
                return
                    "Barthos Gearwing adjusts the dials of a flickering, brass-clad device strapped to his wrist, its needle twitching wildly.\n\n" +
                    "“Ah! You feel that, don’t you? The tingle in the air? That’s *radiant flux decay*, that is. Means trouble. Big trouble.”\n\n" +
                    "“One of my containment designs has... well, failed. Spectacularly. That vault—Preservation Vault 44—it was never meant to house something like **the AtomicRaptor**. I’ve tracked its emissions. It's roaming the vault, *corroding my shield arrays* as it goes. If it destabilizes the core walls, we could see a chain reaction! Not good. Not good at all.”\n\n" +
                    "**Slay the AtomicRaptor** before it shreds the vault’s defenses. Bring me proof, and I’ll see you rewarded with something *charged*—Stormflight, a marvel of my own making.”\n\n" +
                    "*“Don’t mind the counter. It just clicks when danger’s close.”*";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Oh, I see. Well then, keep your distance from Vault 44, friend. And if the vault walls start glowing? Run.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lives? My counters are going mad, friend! The vault can’t withstand that beast much longer!";
            }
        }

        public override object Complete
        {
            get
            {
                return "You've done it? Truly? The AtomicRaptor... *neutralized*.\n\n" +
                       "Then the vault may yet hold. My shields can reset. My devices... can breathe again.\n\n" +
                       "Take this, as promised: **Stormflight**. Harness the skies as you’ve harnessed chaos.";
            }
        }

        public RaptorFalloutQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AtomicRaptor), "AtomicRaptor", 1));
            AddReward(new BaseReward(typeof(Stormflight), 1, "Stormflight"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Raptor Fallout'!");
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

    public class BarthosGearwing : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RaptorFalloutQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTinker(this)); // Closest vendor type for an Artificer
        }

        [Constructable]
        public BarthosGearwing()
            : base("the Vault Artificer", "Barthos Gearwing")
        {
        }

        public BarthosGearwing(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 90);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1015; // Pale, tech-lab tone
            HairItemID = 0x2047; // Short hair
            HairHue = 1150; // White hair, hint of blue
            FacialHairItemID = 0x204B; // Neatly trimmed beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherNinjaJacket() { Hue = 1152, Name = "Voltweave Coat" }); // Electric blue
            AddItem(new LeatherLegs() { Hue = 1175, Name = "Insulated Breeches" }); // Soft steel gray
            AddItem(new LeatherGloves() { Hue = 1170, Name = "Conductive Gauntlets" });
            AddItem(new Boots() { Hue = 1109, Name = "Shockproof Boots" });
            AddItem(new WizardsHat() { Hue = 1154, Name = "Capacitor's Cap" }); // Bright cyan

            AddItem(new ArtificerWand() { Hue = 1153, Name = "Geiger Rod" }); // Custom tool

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Tinkerer's Pack";
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
