using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WraithOfTheDeepQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Wraith of the Deep"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Elenora Veilwalker*, Spirit Hunter of Mountain Crest.\n\n" +
                    "She moves with a calm, deliberate grace, her garments shimmering faintly with a spectral sheen. " +
                    "In her hands, she clutches an obsidian wand, etched with runes that pulse softly.\n\n" +
                    "“The *Glacial Wraith*—it haunts the hidden alcoves of the Ice Cavern, beyond the reach of flame or light.”\n\n" +
                    "“I’ve tried every rite, every ward. It laughs at them all.”\n\n" +
                    "“Only silver or blessed steel can end it. Will you take up this task, and banish what I cannot?”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I must continue alone... but know this, each night it grows stronger. And colder.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lingers? The cold seeps deeper into the land. My rites falter... I can feel it watching.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The cavern breathes easier. I can feel it.\n\n" +
                       "You’ve not only freed the land—you’ve given me back my purpose.\n\n" +
                       "Take this: *HeavyAnchor*. May it root you firm, even in the face of darkness.";
            }
        }

        public WraithOfTheDeepQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GlacialWraith), "Glacial Wraith", 1));
            AddReward(new BaseReward(typeof(HeavyAnchor), 1, "HeavyAnchor"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Wraith of the Deep'!");
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

    public class ElenoraVeilwalker : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WraithOfTheDeepQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTailor());
        }

        [Constructable]
        public ElenoraVeilwalker()
            : base("the Spirit Hunter", "Elenora Veilwalker")
        {
        }

        public ElenoraVeilwalker(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 80, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale
            HairItemID = 0x2049; // Long Hair
            HairHue = 2101; // Silver-Blue
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1153, Name = "Veilwoven Robe" }); // Frosted Azure
            AddItem(new Cloak() { Hue = 1154, Name = "Cloak of the Silent Chill" }); // Pale Ice
            AddItem(new LeatherGloves() { Hue = 2101, Name = "Spiritbinders" }); // Frostgray
            AddItem(new Boots() { Hue = 2406, Name = "Wanderer's Steps" }); // Stone-gray
            AddItem(new WizardsHat() { Hue = 1153, Name = "Hood of Echoes" }); // Matching robe

            AddItem(new GnarledStaff() { Hue = 2101, Name = "Obsidian Wand of Sealing" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Hunter's Satchel";
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
