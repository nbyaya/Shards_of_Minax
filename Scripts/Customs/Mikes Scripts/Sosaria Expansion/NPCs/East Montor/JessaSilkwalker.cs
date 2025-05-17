using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BoundByFireQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bound by Fire"; } }

        public override object Description
        {
            get
            {
                return
                    "You are approached by *Jessa Silkwalker*, a merchant draped in shimmering fabrics, her expression a mixture of grace and concern.\n\n" +
                    "She fingers a scrap of scorched silk, eyes narrowed.\n\n" +
                    "“My caravans... gone. Ambushed on the old trade road by drakes—twisted things, bearing the mark of some ancient flame-bound empire.”\n\n" +
                    "“The **Dragonbound Overseer** commands them, lording over the lesser drakes from the depths of the **Caves of Drakkon**.”\n\n" +
                    "“I’ve lost not only goods, but good people. I want vengeance, yes—but more, I need those roads safe again. For trade, for life.”\n\n" +
                    "**Slay the Dragonbound Overseer**. Let the fire it serves be quenched in its blood.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the flames consume more than silk. But know, those fires will one day burn unchecked.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Overseer still commands? My dreams are haunted by the crackling of silk set alight.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The flame is doused, then? Bless you, truly.\n\n" +
                       "Here, take this: **BardOfErinsMuffler**. It’s more than silk—it’s woven with threads blessed by the wandering bards of old. Let it warm you, as you’ve protected what’s dear to me.";
            }
        }

        public BoundByFireQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DragonboundOverseer), "Dragonbound Overseer", 1));
            AddReward(new BaseReward(typeof(BardOfErinsMuffler), 1, "BardOfErinsMuffler"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Bound by Fire'!");
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

    public class JessaSilkwalker : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BoundByFireQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTailor());
        }

        [Constructable]
        public JessaSilkwalker()
            : base("the Silk Merchant", "Jessa Silkwalker")
        {
        }

        public JessaSilkwalker(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(60, 60, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Deep violet
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 2117, Name = "Firelight Gown" }); // Fiery orange-red
            AddItem(new Cloak() { Hue = 2950, Name = "Drake's Bane Cloak" }); // Smoldering ember
            AddItem(new ThighBoots() { Hue = 2406, Name = "Silken Boots" }); // Golden thread
            AddItem(new BodySash() { Hue = 1166, Name = "Trade Winds Sash" }); // Deep sea blue
            AddItem(new FeatheredHat() { Hue = 1150, Name = "Ashen Plume Hat" }); // Ash-gray

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Merchant's Pack";
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
