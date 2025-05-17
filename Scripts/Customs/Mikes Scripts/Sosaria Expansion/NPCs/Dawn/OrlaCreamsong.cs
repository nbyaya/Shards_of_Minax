using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MilkAndMaliceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Milk and Malice"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Orla Creamsong*, the renowned dairy farmer of Dawn.\n\n" +
                    "She wipes her brow, the scent of fresh butter and hay clinging to her homespun apron.\n\n" +
                    "“You’ll forgive the smell, friend. We’ve had no time to rest, let alone bathe. These blasted *Cult Brigands*—they’ve been raiding my milkmen! Chanting like mad devils at the very gates of Doom Dungeon. They’ve stolen my finest cheeses, threatened my folk, and now they demand some ‘dark tithe.’”\n\n" +
                    "“You look like you’ve the spine to set this right. **Slay the brigand leader** who haunts the entrance to Doom. Bring peace back to my farmhands, and let the cream flow free again!”\n\n" +
                    "“They say he chants in tongues not meant for mortal ears… but steel silences all tongues in time.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Suit yourself. But don’t come crying when your bread’s dry and your stew’s thin for lack of butter.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still he skulks about? My milkmen dare not leave the village. I fear the brigands will come to Dawn next.";
            }
        }

        public override object Complete
        {
            get
            {
                return "Blessed be the churn and the cow! You've done it!\n\n" +
                       "No more will we cower in fear of losing our craft to dark hands. You’ve not just slain a brigand—you’ve defended the lifeblood of our community.\n\n" +
                       "Take these *Loomsoled Wanderers*. May they carry you as steady as a milkman on the morning run.";
            }
        }

        public MilkAndMaliceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CultBrigand), "Cult Brigand", 1));
            AddReward(new BaseReward(typeof(LoomsoledWanderers), 1, "Loomsoled Wanderers"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Milk and Malice'!");
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

    public class OrlaCreamsong : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MilkAndMaliceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFarmer());
        }

        [Constructable]
        public OrlaCreamsong()
            : base("the Dairy Farmer", "Orla Creamsong")
        {
        }

        public OrlaCreamsong(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 65, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2047; // Long hair
            HairHue = 1153; // Creamy Blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2412, Name = "Buttercream Blouse" }); // Soft cream
            AddItem(new LongPants() { Hue = 2424, Name = "Churner's Trousers" }); // Earth brown
            AddItem(new HalfApron() { Hue = 2418, Name = "Milkstained Apron" }); // Linen white
            AddItem(new StrawHat() { Hue = 2101, Name = "Dawnlight Hat" }); // Light tan
            AddItem(new Boots() { Hue = 2106, Name = "Farmstead Boots" }); // Muddy leather

            Backpack backpack = new Backpack();
            backpack.Hue = 1107;
            backpack.Name = "Cheesemaker's Satchel";
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
