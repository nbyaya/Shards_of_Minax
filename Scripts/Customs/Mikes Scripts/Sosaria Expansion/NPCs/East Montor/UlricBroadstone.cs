using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FrogPrinceNoMoreQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Frog Prince No More"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Ulric Broadstone*, the stoic carpenter of East Montor.\n\n" +
                    "He wipes sawdust from his brow, his clothes damp from recent storms, frustration evident in his voice.\n\n" +
                    "“The dam's near to burstin'. The water's murky, foul—and my timber can't hold against the rot.”\n\n" +
                    "“It's that blasted **DreadBullFrog**, sittin' fat atop the dam like he rules the land. Spits acid, he does. Eats my beams and soaks the earth.”\n\n" +
                    "“People say it's cursed, come from the Caves of Drakkon, but I say no beast should lay waste to good work. **Help me kill the brute**, or we'll all be wadin' in filth soon enough.”\n\n" +
                    "“Bring me proof it’s dead, and I’ll see you rewarded proper. The *VestOfTheVeinSeeker*—made from the toughest woods, woven with miner’s thread. It’ll serve you well.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "If you won’t help, then may the river gods have mercy. This dam won’t hold for long.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still croakin'? Then we’re still drownin’. That beast won’t stop till the dam’s dust.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The frog’s no more? \n\n" +
                       "*Ulric clasps your hand, eyes wet with relief.*\n\n" +
                       "“The dam stands, and we can breathe again. Take this—*VestOfTheVeinSeeker*. May it shield you from worse things than frogs.”";
            }
        }

        public FrogPrinceNoMoreQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DreadBullFrog), "DreadBullFrog", 1));
            AddReward(new BaseReward(typeof(VestOfTheVeinSeeker), 1, "VestOfTheVeinSeeker"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Frog Prince No More'!");
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

    public class UlricBroadstone : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FrogPrinceNoMoreQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter());
        }

        [Constructable]
        public UlricBroadstone()
            : base("the Dam Carpenter", "Ulric Broadstone")
        {
        }

        public UlricBroadstone(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 8251; // Shoulder-length wavy
            HairHue = 1107; // Muddy brown
            FacialHairItemID = 8255; // Thick beard
            FacialHairHue = 1107;
        }

        public override void InitOutfit()
        {
            AddItem(new Doublet() { Hue = 1801, Name = "Waterproof Workshirt" }); // Deep blue
            AddItem(new LeatherLegs() { Hue = 2106, Name = "Bog-Hardened Pants" }); // Swamp brown
            AddItem(new HalfApron() { Hue = 2213, Name = "Carpenter's Staincloth" }); // Pine green
            AddItem(new LeatherGloves() { Hue = 2413, Name = "Splinter-Safe Gloves" }); // Bark grey
            AddItem(new Boots() { Hue = 1109, Name = "Floodwalkers" }); // Dark mud

            AddItem(new CarpentersHammer() { Hue = 0, Name = "Ulric's Trusty Hammer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1151; // Dark oak
            backpack.Name = "Woodworker's Pack";
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
