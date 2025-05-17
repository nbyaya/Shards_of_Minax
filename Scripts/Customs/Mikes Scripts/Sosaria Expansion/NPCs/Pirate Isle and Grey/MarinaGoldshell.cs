using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WraithOfTheDepthsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Wraith of the Depths"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Marina Goldshell*, Pearl Diver of the Pirate Isle coves.\n\n" +
                    "Her hands are calloused from nets and tides, yet her eyes gleam with a mix of salt-spray and sorrow.\n\n" +
                    "“The sea’s cursed me, I swear it. My clam beds? Spoiled. My pearls? Stolen. That wraith… it rose from the depths with golden chains and took them to Exodus.”\n\n" +
                    "“A *Sunken Aurum Wraith*, they say. Drawn to gold, bound to the wrecks. It moans, aye, and those moans… they guide me to sunken treasures—but they cost me.”\n\n" +
                    "“Bring me back my golden pearls. Slay that wraith, before I drown chasing its cursed voice.”\n\n" +
                    "**Slay the Sunken Aurum Wraith** in Exodus Dungeon and return Marina’s lost pearls.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the tides drag me down with the rest of the drowned dreams.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it drifts? Still it sings? I can't sleep. I can't breathe. The pearls—they call to me.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve silenced it? The wraith’s song… it’s gone.\n\n" +
                       "You’ve given me more than pearls. You’ve given me peace.\n\n" +
                       "Take these *Lovely Lilies*, plucked from the sea’s heart. May they bloom in memory of what’s lost—and what’s found.";
            }
        }

        public WraithOfTheDepthsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SunkenAurumWraith), "Sunken Aurum Wraith", 1));
            AddReward(new BaseReward(typeof(LovelyLilies), 1, "Lovely Lilies"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Wraith of the Depths'!");
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

    public class MarinaGoldshell : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WraithOfTheDepthsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBJewel());
        }

        [Constructable]
        public MarinaGoldshell()
            : base("the Pearl Diver", "Marina Goldshell")
        {
        }

        public MarinaGoldshell(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1023; // Sun-kissed tan
            HairItemID = 0x203C; // Long hair
            HairHue = 1153; // Ocean blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1150, Name = "Seasilk Blouse" });
            AddItem(new Skirt() { Hue = 1365, Name = "Pearl-Diver’s Wrap" });
            AddItem(new BodySash() { Hue = 2101, Name = "Tidewoven Sash" });
            AddItem(new Sandals() { Hue = 1175, Name = "Coral-Touched Sandals" });
            AddItem(new FeatheredHat() { Hue = 1170, Name = "Waveshade Hat" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Diver’s Satchel";
            AddItem(backpack);

            AddItem(new FishingPole() { Hue = 2502, Name = "Pearl Hunter's Pole" });
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
