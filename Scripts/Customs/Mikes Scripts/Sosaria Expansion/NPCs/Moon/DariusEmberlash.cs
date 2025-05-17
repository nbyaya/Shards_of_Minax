using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EmbersOfTheDeltaQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Embers of the Delta"; } }

        public override object Description
        {
            get
            {
                return
                    "Darius Emberlash, the fire-weaving bard of Moon, gazes solemnly into the bonfire.\n\n" +
                    "“Each flame tells a tale, but one now screams. The *Flame of the Nile* dances with malice, haunting our sacred bonfire nights.\n\n" +
                    "It leaps, reignites, defies our songs. You must extinguish this spirit's blaze, or it will consume more than fire.\n\n" +
                    "**Seek and destroy the Flame of the Nile**, and bring peace to our nights.”";
            }
        }

        public override object Refuse { get { return "The flames will not rest. Beware, for they may reach further with each passing night."; } }

        public override object Uncomplete { get { return "The Flame still flickers? It grows bold, feeding on our fears—do not let it endure."; } }

        public override object Complete { get { return "Silence... and only the gentle crackle of true fire remains. You have freed us from its grasp. Take these sandals, woven from petals that once survived the desert heat."; } }

        public EmbersOfTheDeltaQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FlameOfTheNile), "FlameOfTheNile", 1));
            AddReward(new BaseReward(typeof(PetalstrideSandals), 1, "Petalstride Sandals"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Embers of the Delta'!");
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

    public class DariusEmberlash : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(EmbersOfTheDeltaQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBJewel()); 
        }

        [Constructable]
        public DariusEmberlash()
            : base("the Caste Bard", "Darius Emberlash")
        {
        }

        public DariusEmberlash(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 80, 40);

            Female = false;
            Body = 0x190; // Male Body
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1358; // Fiery Red
            FacialHairItemID = 0x2041; // Short Beard
            FacialHairHue = 1358;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1356, Name = "Blazing Ember Shirt" }); // Deep crimson hue
            AddItem(new ElvenPants() { Hue = 1355, Name = "Sand-Touched Trousers" }); // Burnt amber tone
            AddItem(new BodySash() { Hue = 1359, Name = "Bard’s Flame Sash" }); // Vibrant flame hue
            AddItem(new Cloak() { Hue = 1151, Name = "Cloak of Desert Whispers" }); // Pale gold
            AddItem(new Sandals() { Hue = 1360, Name = "Ashen Step Sandals" }); // Smoky grey
            AddItem(new Scepter() { Hue = 1365, Name = "Torchsong Scepter" }); // Ember-glow staff
            Backpack backpack = new Backpack();
            backpack.Hue = 45;
            backpack.Name = "Scroll Satchel";
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
