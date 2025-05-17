using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EndTheLamentQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "End the Lament"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Iris Scrollscribe*, Record Keeper of Dawn, standing amidst crumbling scrolls and ink-stained hands.\n\n" +
                    "Her gaze is distant, her fingers trembling as she grips a half-written ledger.\n\n" +
                    "“Each morning, the records are gone. In their place... verses. Terrible, hopeless verses. I never wrote them.”\n\n" +
                    "“They speak of the Matriarch. **The Matriarch of Despair**—her dirges eat memory, leave nothing but grief. She dwells below, in the Doom Dungeon, her song reaching even here.”\n\n" +
                    "“I’ve tried sealing the archives. Burning them. Still, her voice returns. If we don’t end her, the minds of Dawn will unravel.”\n\n" +
                    "**Slay the Matriarch of Despair**, and end this sorrow before our very thoughts fade away.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then pray you do not dream. For she will come, in song, in shadow—and you will forget even your own name.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Her lament still echoes. I cannot even remember what I just wrote. End this before all of Dawn succumbs.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... The silence is strange, yet peaceful. For the first time, my hand is steady.\n\n" +
                       "Take this, *TheUmbralFlame*. A relic from before the darkness. May it light your way, always.";
            }
        }

        public EndTheLamentQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MatriarchOfDespair), "Matriarch of Despair", 1));
            AddReward(new BaseReward(typeof(TheUmbralFlame), 1, "TheUmbralFlame"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'End the Lament'!");
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

    public class IrisScrollscribe : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(EndTheLamentQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe(this));
        }

        [Constructable]
        public IrisScrollscribe()
            : base("the Record Keeper", "Iris Scrollscribe")
        {
        }

        public IrisScrollscribe(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(60, 75, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale
            HairItemID = 0x2049; // Long hair
            HairHue = 2101; // Midnight blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1150, Name = "Moonshadow Gown" }); // Shadowy gray-blue
            AddItem(new HoodedShroudOfShadows() { Hue = 2401, Name = "Scribe's Veil" }); // Deep black
            AddItem(new Sandals() { Hue = 2405, Name = "Ink-Stained Sandals" });
            AddItem(new BodySash() { Hue = 2101, Name = "Memorykeeper's Sash" });

            AddItem(new ScribeSword() { Hue = 1154, Name = "Ledgerblade" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Scrollbinder's Pack";
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
