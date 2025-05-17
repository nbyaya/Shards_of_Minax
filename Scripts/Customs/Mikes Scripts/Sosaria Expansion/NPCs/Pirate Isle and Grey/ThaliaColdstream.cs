using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DuskwroughtHuntQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Duskwrought Hunt"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Thalia Coldstream*, Grey’s enigmatic archivist.\n\n" +
                    "Draped in twilight silks, her eyes glint with the knowledge of forbidden texts.\n\n" +
                    "“Have you seen them, traveler? The shapes beyond the candlelight?”\n\n" +
                    "“There is a beast—a **Duskwrought Stalker**—drawn to the flicker of flame and ink. It has taken those who venture into moonlight, inspired by whispers from my tomes.”\n\n" +
                    "“The creature haunts the ruins of **Exodus**, its form born from shadow and regret. I fear it comes for me next.”\n\n" +
                    "**Slay the Duskwrought Stalker**, and I shall grant you the gloves of the Hexweavers, spun in secret for those who defy the dark.”";
            }
        }

        public override object Refuse
        {
            get { return "Then beware the night. It may not be light you see beyond the windows, but its eyes."; }
        }

        public override object Uncomplete
        {
            get { return "The beast still lives. Its presence clouds the ink, dims the flame. I cannot write—I cannot breathe until it’s gone."; }
        }

        public override object Complete
        {
            get
            {
                return "You have stilled the dark, if only for a time...\n\n" +
                       "These *Hexweavers Mystical Gloves* are yours—woven from the silk of shadows that once danced in the edges of my visions.\n\n" +
                       "May they shield you from the darkness that yet lingers.";
            }
        }

        public DuskwroughtHuntQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DuskwroughtStalker), "Duskwrought Stalker", 1));
            AddReward(new BaseReward(typeof(HexweaversMysticalGloves), 1, "Hexweavers Mystical Gloves"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Duskwrought Hunt'!");
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

    public class ThaliaColdstream : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DuskwroughtHuntQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe(this));
        }

        [Constructable]
        public ThaliaColdstream()
            : base("the Archivist of Shadows", "Thalia Coldstream")
        {
        }

        public ThaliaColdstream(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 60);
            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;
            Hue = 1150; // Pale, shadow-touched

            HairItemID = 0x203C; // Long Hair
            HairHue = 1109; // Ashen Gray
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 2424, Name = "Moonshroud Gown" }); // Dark Indigo
            AddItem(new Cloak() { Hue = 2101, Name = "Veil of Forgotten Pages" }); // Shadow Gray
            AddItem(new Sandals() { Hue = 2301, Name = "Silent Step Sandals" }); // Midnight Blue
            AddItem(new FeatheredHat() { Hue = 1109, Name = "Inkfeather Cap" }); // Ashen Gray

            Backpack backpack = new Backpack();
            backpack.Hue = 1153; // Ink-black
            backpack.Name = "Archivist's Satchel";
            AddItem(backpack);

            AddItem(new SpellWeaversWand() { Hue = 2507, Name = "Wand of Flickering Insight" }); // Violet shimmer
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
