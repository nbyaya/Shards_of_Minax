using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DanceOfDeathQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Dance of Death"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Myra Gourdance*, Cultural Chronicler of Devil Guard.\n\n" +
                    "She adjusts a quill behind her ear, her eyes dark with concern.\n\n" +
                    "“Have you ever seen a *MineDance*, traveler? It's an old rite—joyous, grounded in rhythm and stone. But now... something twisted mocks it.”\n\n" +
                    "“The **MineDancer** haunts the lower tunnels, luring our miners with familiar steps, only to trap them in eternal silence. I chronicled every dance, every tradition... but I never wrote of this.”\n\n" +
                    "“Slay the MineDancer before it turns our heritage into a weapon.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware the mines, friend. Even a familiar tune can lead to doom.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The mines still echo with that *false rhythm*. Our people fear the dance, and with it, our past fades.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You've silenced the MineDancer? Then you've saved more than lives—you’ve preserved our soul.\n\n" +
                       "Take these: *TracklessWyrmbinders*. Crafted to tread paths unseen—use them to walk your own way, free of deceitful echoes.";
            }
        }

        public DanceOfDeathQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MineDancer), "MineDancer", 1));
            AddReward(new BaseReward(typeof(TracklessWyrmbinders), 1, "TracklessWyrmbinders"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Dance of Death'!");
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

    public class MyraGourdance : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DanceOfDeathQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBVagabond()); // Closest match for a cultural chronicler
        }

        [Constructable]
        public MyraGourdance()
            : base("the Cultural Chronicler", "Myra Gourdance")
        {
        }

        public MyraGourdance(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Pale miner’s hue
            HairItemID = 0x2046; // Long Hair
            HairHue = 1150; // Silver-grey
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 2101, Name = "Ceremonial Shroud of Echoes" }); // Deep purple-blue
            AddItem(new LeatherSkirt() { Hue = 1842, Name = "Archivist's Guard" }); // Earthy brown
            AddItem(new LeatherGloves() { Hue = 2115, Name = "Inkstained Gloves" }); // Deep violet
            AddItem(new Sandals() { Hue = 1175, Name = "Stone-Worn Sandals" }); // Ashen grey

            AddItem(new ScribeSword() { Hue = 2401, Name = "Quillblade" }); // Decorative, symbolic weapon
            Backpack backpack = new Backpack();
            backpack.Hue = 1134;
            backpack.Name = "Chronicler's Satchel";
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
