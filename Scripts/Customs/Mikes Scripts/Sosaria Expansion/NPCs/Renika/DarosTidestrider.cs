using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SerpentsCoilQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Serpent's Coil"; } }

        public override object Description
        {
            get
            {
                return
                    "*Daros Tidestrider* leans over a ledger, ink-stained fingers gripping the parchment tight.\n\n" +
                    "“No goods. No coin. No trade. And why? A serpent—the *ObsidianSerpent*—chokes the very lifeblood of Renika.”\n\n" +
                    "“I’m no captain, not yet. But I aim to be. Help me rid these waters of the beast, and perhaps... perhaps I’ll earn the right to sail under my own banner.”\n\n" +
                    "**Slay the ObsidianSerpent** near Mountain Stronghold and let trade flow once more.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the tides favor us still, but I fear the docks will remain silent.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it coils? The merchants grow restless, and my dreams of the sea slip away.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The serpent lies dead?\n\n" +
                       "The tides will turn for Renika—and for me. I owe you more than this *Doomsickle*, but take it with my deepest thanks. One day, I’ll sail, and your name will be sung on every wave.";
            }
        }

        public SerpentsCoilQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ObsidianSerpent), "ObsidianSerpent", 1));
            AddReward(new BaseReward(typeof(Doomsickle), 1, "Doomsickle"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Serpent's Coil'!");
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

    public class DarosTidestrider : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SerpentsCoilQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner());
        }

        [Constructable]
        public DarosTidestrider()
            : base("the Merchant's Assistant", "Daros Tidestrider")
        {
        }

        public DarosTidestrider(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(60, 50, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Sun-kissed tone
            HairItemID = 0x2047; // Long hair
            HairHue = 1153; // Deep ocean blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1355, Name = "Tidestrider's Silk" }); // Seafoam Green
            AddItem(new LongPants() { Hue = 1365, Name = "Wave-Bound Breeches" }); // Deep Teal
            AddItem(new HalfApron() { Hue = 1150, Name = "Ledger-Keeper's Apron" }); // Midnight Blue
            AddItem(new Shoes() { Hue = 1109, Name = "Dockside Striders" }); // Storm Gray
            AddItem(new TricorneHat() { Hue = 1157, Name = "First Mate’s Hope" }); // Sapphire Blue
            AddItem(new Cutlass() { Hue = 2405, Name = "Trade Defender" }); // Dull Steel

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Merchant's Satchel";
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
