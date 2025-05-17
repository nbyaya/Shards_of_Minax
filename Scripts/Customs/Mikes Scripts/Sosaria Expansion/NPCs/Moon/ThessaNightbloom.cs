using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HornOfNightQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Horn of Night"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Thessa Nightbloom*, a serene figure cloaked in the colors of midnight.\n\n" +
                    "She’s seated beneath an overhanging bloom, glowing faintly under starlight, as she crushes silvery petals between her fingers.\n\n" +
                    "“Do you smell it?” she asks, dreamily. “Moonblossoms. They only bloom when the moon wanes. And something stirs them this season. Something cruel.”\n\n" +
                    "“A unicorn—yes, a creature of majesty. But this one, *twisted*. It was once a guardian of sacred flora. Now it drains life into vials of crystal... corrupted by the Pharaoh’s old bond.”\n\n" +
                    "“The grove must breathe freely again. The moonblossoms are crying, and I fear their sorrow will taint the soil for generations.”\n\n" +
                    "**Slay the Pharaoh’s Unicorn**, and let the grove dream once more.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“Then the petals shall wither, and the land will mourn alone.”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“You feel it too, don’t you? The cold touch on the back of your mind... it grows stronger. The unicorn still draws life.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "Thessa rises silently as moonlight floods the grove.\n\n" +
                       "“You’ve done it... the unicorn’s lament has ceased. I heard it go still in the wind.”\n\n" +
                       "She presses a cool vial into your hand.\n\n" +
                       "*‘Soulleech’—a crystal forged from grief and mercy.*\n\n" +
                       "“Let it drink for you, so you may never be drained again.”";
            }
        }

        public HornOfNightQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PharaohsUnicorn), "Pharaoh’s Unicorn", 1));
            AddReward(new BaseReward(typeof(Soulleech), 1, "Soulleech"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Horn of Night'!");
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

    public class ThessaNightbloom : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HornOfNightQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBArchitect()); 
        }

        [Constructable]
        public ThessaNightbloom()
            : base("the Nocturnal Botanist", "Thessa Nightbloom")
        {
        }

        public ThessaNightbloom(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(65, 70, 100);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Deep moon-silver
        }

        public override void InitOutfit()
        {
            AddItem(new FemaleElvenRobe() { Hue = 1310, Name = "Moonpetal Robe" }); // Soft violet
            AddItem(new WoodlandBelt() { Hue = 1109, Name = "Nightbloom Sash" }); // Ash grey
            AddItem(new Sandals() { Hue = 1152, Name = "Desert Bloom Walkers" }); // Light blue
            AddItem(new HoodedShroudOfShadows() { Hue = 1175, Name = "Lunarshade Hood" }); // Night blue
            AddItem(new FlowerGarland() { Hue = 1151, Name = "Bloomlight Wreath" }); // Pale starlight
            AddItem(new MagicWand() { Hue = 1260, Name = "Vial-Wand of Saplight" }); // Moon-crystal wand
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
