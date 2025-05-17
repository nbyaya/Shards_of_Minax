using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class PurgeAshWraithQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Purge the AshWraith"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Elara Hearthwood*, the town’s herbalist, tending to withering herbs by a soot-streaked window.\n\n" +
                    "Her hands are stained with ash, her eyes clouded with worry.\n\n" +
                    "“The forest edge burns, but not with fire—it’s the Wraith’s breath, seeping through soil and soul.”\n\n" +
                    "“Ash drifts into my garden now, poisons the roots. My mentor once banished such a spirit, long ago. I was too young to learn the rite.”\n\n" +
                    "**Slay the AshWraith**, cleanse the air, and let the forest breathe again. My craft cannot survive its corrosive touch.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I must prepare for loss. Without those herbs, not just I—but all West Montor—will suffer.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Wraith still lingers? The ash thickens, and I fear even the wildflowers begin to weep.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The air... it's lighter. The garden can breathe once more.\n\n" +
                       "You've done more than destroy a menace—you've preserved **life**.\n\n" +
                       "Take this: *Exotic Whistle*. It may summon aid when next you face the wild flame.";
            }
        }

        public PurgeAshWraithQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AshWraith), "AshWraith", 1));
            AddReward(new BaseReward(typeof(ExoticWhistle), 1, "Exotic Whistle"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Purge the AshWraith'!");
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

    public class ElaraHearthwood : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(PurgeAshWraithQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCook());
        }

        [Constructable]
        public ElaraHearthwood()
            : base("the Herbalist", "Elara Hearthwood")
        {
        }

        public ElaraHearthwood(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 90, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1148; // Fiery Auburn
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1358, Name = "Emberleaf Robe" }); // Warm autumn orange
            AddItem(new Sandals() { Hue = 2403, Name = "Ash-Walkers" }); // Charcoal grey
            AddItem(new FlowerGarland() { Hue = 1260, Name = "Withered Bloom Crown" }); // Faded green
            AddItem(new HalfApron() { Hue = 2101, Name = "Herbalist’s Pouch Apron" }); // Earthy brown
            AddItem(new GnarledStaff() { Hue = 1109, Name = "Druid’s Memory" }); // Ashen staff

            Backpack backpack = new Backpack();
            backpack.Hue = 1157;
            backpack.Name = "Apothecary's Satchel";
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
