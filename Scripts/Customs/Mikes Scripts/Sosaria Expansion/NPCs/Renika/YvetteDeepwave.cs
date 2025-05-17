using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SlugOfSilenceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Slug of Silence"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Yvette Deepwave*, renowned diver and pioneer of Renika’s pearl trade.\n\n" +
                    "Her wetsuit gleams faintly, like starlight on ocean waves. A coral pendant rests against her chest, pulsing faintly.\n\n" +
                    "“Our springs are dying. The pearls... they're fading, dissolving. I've dived these waters since I could swim, but never have I seen this.”\n\n" +
                    "“A trail of slime, thicker than silt, spreads from the mountains. It poisons everything it touches.”\n\n" +
                    "“They speak of a beast... the **CragSlug**, a horror from the Stronghold, dragging rot into our world.”\n\n" +
                    "“I need it dead. Or we all drown in silence.”\n\n" +
                    "**Slay the CragSlug** before Renika’s springs are forever fouled.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "The springs won’t last much longer. Neither will my pearls.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The CragSlug still feeds? The waters choke more each day. I beg you, end this.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it. The CragSlug’s trail will fade, and perhaps our springs can breathe again.\n\n" +
                       "Take this—**GuillotineBladeDagger**. I crafted it for precision in the depths. Let it cut your way as sharply as you’ve freed our waters.";
            }
        }

        public SlugOfSilenceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CragSlug), "CragSlug", 1));
            AddReward(new BaseReward(typeof(GuillotineBladeDagger), 1, "GuillotineBladeDagger"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Slug of Silence'!");
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

    public class YvetteDeepwave : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SlugOfSilenceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman()); // Closest vendor type to her diver background
        }

        [Constructable]
        public YvetteDeepwave()
            : base("the Diver", "Yvette Deepwave")
        {
        }

        public YvetteDeepwave(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Ocean-blue
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherBustierArms() { Hue = 1369, Name = "Pearl-Weave Vest" }); // Iridescent sheen
            AddItem(new LeatherShorts() { Hue = 1372, Name = "Tide-Bound Shorts" }); // Deep sea green
            AddItem(new Sandals() { Hue = 1370, Name = "Reef-Striders" }); // Coral tint
            AddItem(new Cloak() { Hue = 1153, Name = "Wavecaller’s Cloak" }); // Deep azure
            AddItem(new BodySash() { Hue = 1367, Name = "Diver’s Sash" }); // Light teal
            AddItem(new GoldBracelet() { Hue = 0, Name = "Bracelet of the Deep" }); // Decorative lore item

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Diver’s Pack";
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
