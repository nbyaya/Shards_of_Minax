using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ArchmagesSilenceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Archmage's Silence"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself face-to-face with *Helena Brightshore*, an elegant mage whose robes shimmer like twilight on the sea.\n\n" +
                    "She gazes toward the distant silhouette of the **Exodus Dungeon**, her eyes haunted by knowledge too vast to bear.\n\n" +
                    "“I have made a grievous error… The Sepulchral Archmage, once a peer of mine in the Circle, is no longer bound by mortal reason. Her spells echo through the catacombs, **ripping holes in the fabric of our world**.”\n\n" +
                    "“Her spellbook remains in the dungeon, pulsing with power. If it falls into the wrong hands, the consequences could be unthinkable. Worse still, it is calling to her even now, feeding her madness.”\n\n" +
                    "**Slay the Sepulchral Archmage** and retrieve her spellbook. End this nightmare before her silence consumes us all.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the Circle’s wards hold… though I fear each moment we delay, her power grows.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "She yet lives? The winds carry her voice—chanting, breaking, *twisting* the weave of magic.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The silence… blessed silence.\n\n" +
                       "You have slain her, then? The spellbook? Yes… I shall see it sealed away, beyond reach of mind or time.\n\n" +
                       "Take this—**the Wand of Woh**—a relic once used to still storms. May it grant you peace, and power, in the face of future darkness.";
            }
        }

        public ArchmagesSilenceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SepulchralArchmage), "Sepulchral Archmage", 1));
            AddReward(new BaseReward(typeof(WandOfWoh), 1, "Wand of Woh"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Archmage's Silence'!");
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

    public class HelenaBrightshore : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ArchmagesSilenceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard());
        }

        [Constructable]
        public HelenaBrightshore()
            : base("Mage of the Circle", "Helena Brightshore")
        {
        }

        public HelenaBrightshore(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 100, 100);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Long hair
            HairHue = 1153; // Deep ocean blue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1153, Name = "Void-Wrapped Robe" }); // Deep ocean blue
            AddItem(new WizardsHat() { Hue = 1175, Name = "Twilight-Crested Hat" }); // Glimmering indigo
            AddItem(new Sandals() { Hue = 1109, Name = "Storm-Touched Sandals" }); // Dusty grey
            AddItem(new BodySash() { Hue = 1175, Name = "Sash of the Arcane Circle" });

            AddItem(new ArtificerWand() { Hue = 1175, Name = "Arcane Channeling Wand" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Brightshore's Satchel";
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
