using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GuardiansFallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Guardian’s Fall"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Felicity Starwatch*, the Herald of Renika, her sea-blue cloak billowing in the coastal wind.\n\n" +
                    "Her eyes, sharp and reflective like the ocean at dusk, narrow as she speaks of forgotten oaths.\n\n" +
                    "“Beneath our lighthouse lies a gate—sealed long before Renika’s founding, guarding the path to my homeland’s lost scrolls. The key to our history, to our truths, rests behind that stone.”\n\n" +
                    "“But the **GraniteGuardian**, relic of the Archons, still stands watch. A sentinel unbroken by time, barring the way with stone and ancient power.”\n\n" +
                    "“I announced Renika’s rise from that lighthouse. I’ve waited centuries to reclaim what lies below. Will you help me see it done?”\n\n" +
                    "**Slay the GraniteGuardian** in the Mountain Stronghold and free the gateway. The past must rise again.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the past will remain silent, and my people’s stories will remain dust.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The GraniteGuardian still endures? The stones remain silent, and so do I.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It is done? The seal is broken, and the stones sing once more. My ancestors’ voices echo through the gate.\n\n" +
                       "You’ve restored more than a path—you’ve restored hope.\n\n" +
                       "Take this *AnimalBox*, a humble token from our shores. May it serve you as you have served Renika.";
            }
        }

        public GuardiansFallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GraniteGuardian), "GraniteGuardian", 1));
            AddReward(new BaseReward(typeof(AnimalBox), 1, "AnimalBox"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Guardian’s Fall'!");
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

    public class FelicityStarwatch : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(GuardiansFallQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMapmaker()); // She’s a Herald tied to lost knowledge, maps, and scrolls.
        }

        [Constructable]
        public FelicityStarwatch()
            : base("the Herald", "Felicity Starwatch")
        {
        }

        public FelicityStarwatch(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Light coastal skin tone
            HairItemID = 0x2047; // Long hair
            HairHue = 1153; // Deep sea-blue
            FacialHairItemID = 0; // None
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1150, Name = "Starwoven Gown" }); // Deep night blue
            AddItem(new Cloak() { Hue = 1289, Name = "Tidecaller’s Cloak" }); // Ocean teal
            AddItem(new BodySash() { Hue = 1367, Name = "Herald’s Sash" }); // Silver-white
            AddItem(new Sandals() { Hue = 1153, Name = "Shorestriders" }); // Deep sea-blue
            AddItem(new FeatheredHat() { Hue = 1154, Name = "Ceremonial Crest" }); // Soft pearl hue

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Starwatch Satchel";
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
