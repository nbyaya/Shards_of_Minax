using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class VenomousSurgeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Venomous Surge"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Garrick Poisonbane*, assistant herbalist of Death Glutch, pacing nervously outside his shop.\n\n" +
                    "His apron is stained with a deep green sap, and his eyes dart frequently toward a bubbling cauldron beside him.\n\n" +
                    "“You feel that? In the air? It's thick... poisoned.”\n\n" +
                    "“The waters feeding into our supply, they’ve gone bad. Something foul from **Malidor Witches Academy**—a *ToxinElemental*, they say. I’m brewing an antidote, but it won’t matter if that thing keeps tainting the stream.”\n\n" +
                    "“I’m no warrior. I mix herbs, I cure coughs. But this—this is bigger. I need someone strong, someone brave. **Slay the ToxinElemental** before we all breathe our last.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then pray you can hold your breath... for this poison, it won’t wait.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still alive? Barely. The elemental still festers in that place, spreading death. I can't hold it back much longer.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve slain it? The air... it’s lighter already. You’ve done more than save me—you’ve saved Death Glutch.\n\n" +
                       "**Take this: BohoChicSundress.** Woven with care, now safe from taint. May it bring light to your steps, free of poison’s grasp.";
            }
        }

        public VenomousSurgeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ToxinElemental), "ToxinElemental", 1));
            AddReward(new BaseReward(typeof(BohoChicSundress), 1, "BohoChicSundress"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Venomous Surge'!");
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

    public class GarrickPoisonbane : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(VenomousSurgeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        [Constructable]
        public GarrickPoisonbane()
            : base("the Herbalist’s Assistant", "Garrick Poisonbane")
        {
        }

        public GarrickPoisonbane(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 30);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 2208; // Moss-green tint
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 2101, Name = "Vinewoven Shroud" }); // Dark forest green
            AddItem(new LeatherGloves() { Hue = 2117, Name = "Sap-Stained Gloves" });
            AddItem(new LeatherLegs() { Hue = 1820, Name = "Barkskin Leggings" });
            AddItem(new HalfApron() { Hue = 2113, Name = "Herbalist’s Apron" });
            AddItem(new Sandals() { Hue = 2106, Name = "Mirewalk Sandals" });

            AddItem(new GnarledStaff() { Hue = 2101, Name = "Rootbound Rod" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Herb Satchel";
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
