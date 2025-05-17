using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RestlessWhispersQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Restless Whispers"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Eamon Shadechaser*, the Spirit Guide of Death Glutch.\n\n" +
                    "Draped in shrouds woven with twilight threads, he clutches a locket that gleams faintly in the gloom.\n\n" +
                    "“I guide spirits from shadow to solace… yet one soul mocks every rite I know.”\n\n" +
                    "“In the crumbled dormitories of **Malidor Witches Academy**, it lingers—a **Wandering Soul**, once a student, now a torment. It whispers to the walls, draws runes in frost upon the glass, and stirs the dreams of those nearby.”\n\n" +
                    "“I found this locket where she died… but it bears no name, only sorrow. Lay her to rest, and perhaps, she will finally speak her truth.”\n\n" +
                    "**Defeat the Wandering Soul** haunting Malidor’s ruins and bring silence to the restless whispers.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the whispers shall continue, and her sorrow shall deepen. But beware—she may find you in your dreams.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You have not yet stilled her cries… the air grows colder with each night she roams free.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The dormitories are quiet at last… Her whispers are gone, and the locket is warm to the touch.\n\n" +
                       "Thank you, kind one. The restless now rest. Take these: **PlantingGloves**—tools of life, given in thanks from one who once knew only death.";
            }
        }

        public RestlessWhispersQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(WanderingSoul), "Wandering Soul", 1));
            AddReward(new BaseReward(typeof(PlantingGloves), 1, "PlantingGloves"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Restless Whispers'!");
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

    public class EamonShadechaser : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RestlessWhispersQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFortuneTeller());
        }

        [Constructable]
        public EamonShadechaser()
            : base("the Spirit Guide", "Eamon Shadechaser")
        {
        }

        public EamonShadechaser(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 90);

            Female = false;
            Body = 0x190; // Male Human
            Race = Race.Human;

            Hue = 0x83EA; // Pale shade
            HairItemID = 0x2047; // Long hair
            HairHue = 1150; // Shadowy gray
            FacialHairItemID = 0x203D; // Long beard
            FacialHairHue = 1150; // Same gray
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1109, Name = "Ebon Shroud of the Lost" });
            AddItem(new LeatherGloves() { Hue = 1150, Name = "Shadewoven Grips" });
            AddItem(new Sandals() { Hue = 1102, Name = "Silent Walkers" });
            AddItem(new Cloak() { Hue = 2101, Name = "Spiritveil Cloak" });
            AddItem(new BodySash() { Hue = 1157, Name = "Lament Sash" });
            AddItem(new GnarledStaff() { Hue = 1153, Name = "Soulroot Staff" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Guide’s Satchel";
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
