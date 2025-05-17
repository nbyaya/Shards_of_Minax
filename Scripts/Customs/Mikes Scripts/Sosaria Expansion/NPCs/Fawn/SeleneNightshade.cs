using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SoulboundQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Soulbound"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Selene Nightshade*, the beekeeper of Fawn, her eyes distant yet sharp, her fingers stained with honey and ash.\n\n" +
                    "She speaks softly, yet the urgency in her voice trembles through the air.\n\n" +
                    "“The hives... they don’t just buzz anymore—they *whisper*. A shadow falls over them whenever the Graventh nears, its touch not only kills, it *binds*... enslaves their minds.”\n\n" +
                    "“Once, it found me. I felt its hunger, its grip like frost on my soul. I barely escaped, and this—” she shows a singed cloak, “—is all that saved me.”\n\n" +
                    "“Destroy the Graventh. Free the hives. Or we will all be soulbound to its will.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then tread carefully near the hives, traveler. If you hear the whispers... run.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Graventh still stirs? The hives grow restless. I fear I will not hear silence again unless it dies.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The whispers... they fade. Thank you, brave soul.\n\n" +
                       "Take this: *MosesStaff*. Let it guide you as you’ve guided me—and the hives—back to peace.";
            }
        }

        public SoulboundQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Graventh), "Graventh", 1));
            AddReward(new BaseReward(typeof(MosesStaff), 1, "MosesStaff"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Soulbound'!");
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

    public class SeleneNightshade : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SoulboundQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBeekeeper());
        }

        [Constructable]
        public SeleneNightshade()
            : base("the Haunted Beekeeper", "Selene Nightshade")
        {
        }

        public SeleneNightshade(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Pale
            HairItemID = 0x203B; // Long Hair
            HairHue = 1150; // Dark blue-black
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 2105, Name = "Ashen Honey Robe" }); // Pale gold, like old wax
            AddItem(new Cloak() { Hue = 1109, Name = "Burnt Cloak of the Hive" }); // Smoky grey-black, scorched edges
            AddItem(new Sandals() { Hue = 1153, Name = "Beekeeper’s Soles" }); // Deep forest green
            AddItem(new FlowerGarland() { Hue = 1358, Name = "Crown of Whispering Blossoms" }); // Light lilac
            AddItem(new LeatherGloves() { Hue = 2101, Name = "Pollen-Stained Gloves" }); // Soft honey hue
            AddItem(new QuarterStaff() { Hue = 1102, Name = "Hivekeeper’s Staff" }); // Dark wood, carved bees along its length

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Herbalist's Satchel";
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
