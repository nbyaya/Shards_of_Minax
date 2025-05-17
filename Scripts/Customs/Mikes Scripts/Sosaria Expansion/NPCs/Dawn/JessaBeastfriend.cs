using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TameTheUntamableQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Tame the Untamable"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Jessa Beastfriend*, her steady hand resting on the muzzle of a restless mastiff.\n\n" +
                    "She sighs, gaze distant, voice laced with frustration and fear.\n\n" +
                    "“They won’t go near the training rings anymore—not since **the Doomed Grizzle** came. I’ve spent years raising these beasts, honing their spirits... but that thing, it *howls*. And the dogs... they shiver like pups in a storm.”\n\n" +
                    "“I tried luring it, trapping it, even calming it—but it’s beyond my voice. Beyond taming. **Its fur snaps like brittle rope under the moonlight**, and its eyes... they see something no creature should.”\n\n" +
                    "“Please, end it. For them. For me. I’ll see you properly rewarded with these *Knucklebinds of the Thorned Path*—once meant for a beastmaster’s hand, now yours if you can slay the untamable.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "I understand... but if it stays, I fear they’ll never trust me—or themselves—again.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It’s still alive? I hear it at night, in my dreams... and they do too. Their howls echo it now.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You did it? Truly? The ring’s silent again... and they’ve returned to me.\n\n" +
                       "Thank you, friend. You’ve done more than slay a beast—you’ve saved their spirits.\n\n" +
                       "Take these *Knucklebinds of the Thorned Path*. May they serve you as well as you’ve served us.";
            }
        }

        public TameTheUntamableQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DoomedGrizzle), "the Doomed Grizzle", 1));
            AddReward(new BaseReward(typeof(KnucklebindsOfTheThornedPath), 1, "Knucklebinds of the Thorned Path"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Tame the Untamable'!");
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

    public class JessaBeastfriend : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(TameTheUntamableQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        [Constructable]
        public JessaBeastfriend()
            : base("the Beastfriend", "Jessa Beastfriend")
        {
        }

        public JessaBeastfriend(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 2101; // Chestnut Brown
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 2406, Name = "Beastkeeper's Vest" }); // Earth-brown
            AddItem(new LeatherLegs() { Hue = 2105, Name = "Trail-Worn Trousers" });
            AddItem(new LeatherGloves() { Hue = 1819, Name = "Bridle-Grip Mitts" });
            AddItem(new Bandana() { Hue = 2418, Name = "Beastfriend's Bandana" }); // Dusty Red
            AddItem(new Boots() { Hue = 2101, Name = "Mudstompers" });
            AddItem(new BodySash() { Hue = 1153, Name = "Thorned Path Sash" }); // Subtle Green-Gray, symbolizing nature & resilience

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Trainer's Pack";
            AddItem(backpack);

            AddItem(new ShepherdsCrook() { Hue = 1157, Name = "Beastcaller's Crook" }); // Slightly shimmering blue
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
