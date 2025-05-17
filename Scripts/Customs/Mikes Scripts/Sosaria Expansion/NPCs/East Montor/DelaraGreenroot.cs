using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class CoppersCurseQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Copper's Curse"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Delara Greenroot*, a renowned herbalist with streaks of copper in her emerald-dyed hair.\n\n" +
                    "She clutches a wilted sprig of night-shade, eyes flashing with urgency.\n\n" +
                    "“You can smell it, can’t you? The scorched earth... the bitterness of burnt roots. It’s the **CopperDragon**, nesting too close to my gardens.”\n\n" +
                    "“Its molten breath has withered half my crop—and worse, I’ve dreamt the beast’s scales hold a compound that could heal even Void-taint.”\n\n" +
                    "“I need you to **slay the CopperDragon**, not just for my herbs, but for the hope that lies in its molten hide.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "If the dragon is left to burn unchecked, East Montor’s gardens will become nothing but ash.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The CopperDragon still breathes? My herbs won't survive another sunrise. Nor will the chance to cure what others deem incurable.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve slain it? Then there’s hope after all...\n\n" +
                       "*Delara gently touches a scale from the beast, lost in thought.*\n\n" +
                       "“Thank you, friend. Take this—*HyruleKnightsShield*. Let it protect you as fiercely as you’ve defended our future.”";
            }
        }

        public CoppersCurseQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CopperDragon), "CopperDragon", 1));
            AddReward(new BaseReward(typeof(HyruleKnightsShield), 1, "HyruleKnightsShield"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Copper's Curse'!");
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

    public class DelaraGreenroot : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(CoppersCurseQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        [Constructable]
        public DelaraGreenroot()
            : base("the Herbalist", "Delara Greenroot")
        {
        }

        public DelaraGreenroot(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 35);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2044; // Long hair
            HairHue = 2208; // Emerald green
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1421, Name = "Verdant Herbalist Robe" }); // Deep forest green
            AddItem(new FlowerGarland() { Hue = 1167, Name = "Copperleaf Crown" }); // Copper-orange
            AddItem(new Sandals() { Hue = 1445, Name = "Earthen Tread" }); // Brown
            AddItem(new BodySash() { Hue = 1272, Name = "Night-Shade Belt" }); // Dark purple
            AddItem(new GnarledStaff() { Hue = 2412, Name = "Rootcaller's Crook" }); // Twisted wood hue
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
