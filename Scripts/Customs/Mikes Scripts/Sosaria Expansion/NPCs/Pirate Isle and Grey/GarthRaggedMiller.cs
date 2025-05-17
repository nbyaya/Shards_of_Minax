using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RemainsToAshesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Remains to Ashes"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Garth “Ragged” Miller*, Rogue Trader of Pirate Isle, hunched over a weatherworn crate, his fingers tracing gouges left by some unnatural force.\n\n" +
                    "His eyes flash as he looks up, a spark of defiance underlined by desperation.\n\n" +
                    "“That damned thing... the **RaggedRemains**. It took everything. My caravan—gone. My crates—swallowed whole. Left me with nothing but these scars and a grudge.”\n\n" +
                    "“But I know it wasn’t random. Someone sent that beast. A rival, no doubt, hoping to see me ruined.”\n\n" +
                    "“You? You look like you’ve seen your share of horrors. Maybe you’ve even put a few down. **Find the RaggedRemains. Burn it. Slash it. I don’t care. Just kill it and bring back my crates**—what’s left of ‘em.”\n\n" +
                    "**Slay the RaggedRemains** in the depths of Exodus Dungeon. Do this, and I’ll see you well rewarded—more than just coin. You’ll have a friend in Pirate Isle, and that’s worth more than gold.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Cowards don’t last long around here. Don’t come crying when the Remains comes for you next.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still out there, is it? That thing won’t stop until it’s fed. You’d best hurry, before someone else ends up in pieces.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The beast is dead? Ha! **Burnt to cinders, I hope!**\n\n" +
                       "And my crates? Torn up, but some of it’s salvageable. That’s good enough.\n\n" +
                       "Here—*BlackthornesSpur*. May it drive your enemies to ruin, like you’ve done for me.";
            }
        }

        public RemainsToAshesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(RaggedRemains), "RaggedRemains", 1));
            AddReward(new BaseReward(typeof(BlackthornesSpur), 1, "BlackthornesSpur"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Remains to Ashes'!");
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

    public class GarthRaggedMiller : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RemainsToAshesQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBThief());
        }

        [Constructable]
        public GarthRaggedMiller()
            : base("the Rogue Trader", "Garth “Ragged” Miller")
        {
        }

        public GarthRaggedMiller(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 90, 70);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1810; // Sea-storm grey
            FacialHairItemID = 0x203E; // Short Beard
            FacialHairHue = 1810;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1154, Name = "Stormweave Shirt" }); // Deep Navy
            AddItem(new LongPants() { Hue = 1109, Name = "Salt-Cracked Breeches" }); // Washed Grey
            AddItem(new HalfApron() { Hue = 1908, Name = "Trader’s Hide Apron" }); // Weathered Brown
            AddItem(new Boots() { Hue = 1908, Name = "Seafarer’s Boots" });
            AddItem(new Bandana() { Hue = 1151, Name = "Ragged’s Tied Bandana" }); // Midnight Blue
            AddItem(new LeatherGloves() { Hue = 1153, Name = "Brine-Stained Gloves" });

            AddItem(new Cutlass() { Hue = 0, Name = "Trader’s Defense" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Trader's Satchel";
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
