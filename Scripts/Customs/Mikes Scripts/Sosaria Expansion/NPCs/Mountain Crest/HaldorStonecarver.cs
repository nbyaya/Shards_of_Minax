using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ExecutionersEchoQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Executioner's Echo"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Haldor Stonecarver*, the Graveyard Warden of Mountain Crest.\n\n" +
                    "Draped in somber garb, his voice is as cold as the winds that sweep the burial grounds.\n\n" +
                    "\"The dead cannot rest. Not while that thing walks.\"\n\n" +
                    "\"The *Glacial Executioner*, once a tormentor in life, now haunts the burial tunnels beneath the Ice Cavern. His phantom axe shatters not just stone, but peace itself. Each swing echoes through the graves, stirring the bones of the innocent.\"\n\n" +
                    "\"I am sworn to the stones, to keep them sealed. But I cannot face him. You must. **Slay the Glacial Executioner**, and still his cursed echo.\"\n\n" +
                    "\"Do this, and take *HeartbreakerSunder*, forged from tombstone and tear alike. A blade fit to silence torment.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the dead forgive us both. Each night they stir more, calling out for an end you would not grant.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You have not yet faced him? The graves quake. His axe will break more than stone if you wait.";
            }
        }

        public override object Complete
        {
            get
            {
                return "He is fallen? Then rest returns, if only for a time.\n\n" +
                       "You’ve done more than slay a wraith—you’ve bound peace to the earth once more.\n\n" +
                       "Take this: *HeartbreakerSunder*. May it remind you of stone’s silence, and the strength it takes to protect it.";
            }
        }

        public ExecutionersEchoQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GlacialExecutioner), "Glacial Executioner", 1));
            AddReward(new BaseReward(typeof(HeartbreakerSunder), 1, "HeartbreakerSunder"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Executioner's Echo'!");
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

    public class HaldorStonecarver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ExecutionersEchoQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner()); // Gravewardens often deal in ceremonial hides and grave shrouds.
        }

        [Constructable]
        public HaldorStonecarver()
            : base("the Graveyard Warden", "Haldor Stonecarver")
        {
        }

        public HaldorStonecarver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1055; // Pale, almost marble-skinned
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Frost white
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1109, Name = "Stonewarden's Shroud" }); // Dark grey, grave-like
            AddItem(new LeatherGloves() { Hue = 1157, Name = "Tombbinder's Grips" }); // Ice-blue hue
            AddItem(new Boots() { Hue = 1102, Name = "Grave-Strider Boots" }); // Shadow black
            AddItem(new HoodedShroudOfShadows() { Hue = 1150, Name = "Helm of Silent Vigil" }); // Pale white

            AddItem(new QuarterStaff() { Hue = 2401, Name = "Gravemark Staff" }); // Weathered stone look

            Backpack backpack = new Backpack();
            backpack.Hue = 2101;
            backpack.Name = "Warden's Satchel";
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
