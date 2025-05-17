using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class StillTheBurningCorpseQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Still the BurningCorpse"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Maelin Frost*, healer of West Montor, standing by her herb-laden table, wiping ash from her hands.\n\n" +
                    "Her eyes, usually calm, now burn with grim determination.\n\n" +
                    "\"I can soothe wounds, cure fevers, even banish some curses—but this?\"\n\n" +
                    "\"Something charred, twisted, and angry has risen from the Gate of Hell. **The BurningCorpse.** Its limbs still smolder, and they wander… they find their way here. Into my clinic. Onto my floors.\"\n\n" +
                    "\"My salves melt, my prayers fade. The dead should rest, not burn eternal. **Put it down** before it sets all West Montor ablaze.\"\n\n" +
                    "**Slay the BurningCorpse**, or this town will know fire like never before.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may you never need my healing. But know this: fire spreads faster than regret.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it burns? Then we are all but kindling.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it… the clinic is still, the air no longer reeks of charred flesh.\n\n" +
                       "Take this. *LawyerBriefcase*. An odd thing, yes—but inside, you’ll find more than parchment. Knowledge, power, and perhaps… protection from the next flame.";
            }
        }

        public StillTheBurningCorpseQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BurningCorpse), "BurningCorpse", 1));
            AddReward(new BaseReward(typeof(LawyerBriefcase), 1, "LawyerBriefcase"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Still the BurningCorpse'!");
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

    public class MaelinFrost : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(StillTheBurningCorpseQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHealer());
        }

        [Constructable]
        public MaelinFrost()
            : base("the Flame-Touched Healer", "Maelin Frost")
        {
        }

        public MaelinFrost(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2047; // Long hair
            HairHue = 1153; // Ashen white
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1154, Name = "Cindershroud Robe" }); // Soft grey-blue
            AddItem(new BodySash() { Hue = 1359, Name = "Ashen Sash" }); // Pale smoke
            AddItem(new Sandals() { Hue = 1109, Name = "Healer's Steps" }); // Dust grey
            AddItem(new SkullCap() { Hue = 1153, Name = "Crown of Soot" }); // Charred grey
            AddItem(new QuarterStaff() { Hue = 1175, Name = "Smokewood Staff" }); // Dark with light runes

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Healer's Pack";
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
