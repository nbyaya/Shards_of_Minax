using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class JacksNoMoreQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Jack’s No More"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Mirella Dewpetal*, Fawn’s beloved herbalist, amidst rows of glowing herbs, their leaves shimmering under the moonlight.\n\n" +
                    "She looks up, her violet eyes reflecting both warmth and concern.\n\n" +
                    "“You’ve seen it, haven’t you? The way the nightshade glows when *he* passes—the Glimmerjack. His pelt… it’s unlike anything else, soaked in luminescence that nourishes or destroys.”\n\n" +
                    "“I’ve used its essence in healing, but now… the creature’s grown bold, venturing too close, warping the land’s balance. I need you to end its reign. **Bring me the pelt**, and I can craft salves to restore what’s been lost.”\n\n" +
                    "**Slay the Glimmerjack**, and reclaim peace for our groves.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the glow remains unchallenged. I only hope our lands survive its touch a little longer.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still the Glimmerjack haunts us? I can feel its glow creeping closer in the roots and branches. Please, bring me that pelt.";
            }
        }

        public override object Complete
        {
            get
            {
                return "Ahh… the pelt. Even now, it hums with wild energy. You’ve done a great service to Fawn, and to the earth itself.\n\n" +
                       "Please, take these *Boots of Command*. Walk with purpose, as you have walked through shadow.";
            }
        }

        public JacksNoMoreQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Glimmerjack), "Glimmerjack", 1));
            AddReward(new BaseReward(typeof(BootsOfCommand), 1, "Boots of Command"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Jack’s No More'!");
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

    public class MirellaDewpetal : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(JacksNoMoreQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        [Constructable]
        public MirellaDewpetal()
            : base("the Herbalist", "Mirella Dewpetal")
        {
        }

        public MirellaDewpetal(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 25);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 33770; // Soft, earthy skin tone
            HairItemID = 0x2046; // Long hair
            HairHue = 1154; // Silver-blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1289, Name = "Moonshade Gown" }); // Soft lavender
            AddItem(new FlowerGarland() { Hue = 1359, Name = "Glimmerpetal Wreath" }); // Glimmering blue
            AddItem(new Sandals() { Hue = 2105, Name = "Twilight Sandals" }); // Deep violet
            AddItem(new Cloak() { Hue = 1365, Name = "Mistcloak of Fawn" }); // Gentle teal

            AddItem(new QuarterStaff() { Hue = 1153, Name = "Rootbinder Staff" }); // Custom staff for flavor

            Backpack backpack = new Backpack();
            backpack.Hue = 1175; // Pale green
            backpack.Name = "Herbalist's Satchel";
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
