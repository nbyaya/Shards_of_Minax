using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ObsidiansWrathQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Obsidian’s Wrath"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Petra Coraleye*, a renowned glassblower of Renika.\n\n" +
                    "Her eyes, bright as molten glass, scan your face with desperate hope. She adjusts the delicate tools at her belt and gestures to a cracked black vase, shimmering faintly in the sunlight.\n\n" +
                    "“I tried to mold perfection... but the dragon’s breath won’t let me. It taints the air, warps the fire, and my glass shatters like ice.”\n\n" +
                    "“Deep in the **Mountain Stronghold**, the **ObsidianDragon** slumbers. Its fang holds the balance of fire and stone, needed to shape a flawless obsidian vessel. I've saved a diver from lava once, but I can’t face this beast.”\n\n" +
                    "**Slay the ObsidianDragon**. Bring me its fang, and I'll finally tame the flame.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then my work remains unfinished... and the dragon’s fury reigns unchallenged.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The dragon still lives? My kiln grows colder with each passing day.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... the fang is magnificent, radiating with perfect balance. Now, at last, my obsidian will hold. \n\n" +
                       "**Take this: the DarkKnightsCursedChestplate.** A piece as strong and dark as the glass I dream of. May it shield you from fire and shadow alike.";
            }
        }

        public ObsidiansWrathQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ObsidianDragon), "ObsidianDragon", 1));
            AddReward(new BaseReward(typeof(DarkKnightsCursedChestplate), 1, "DarkKnightsCursedChestplate"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Obsidian’s Wrath'!");
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

    public class PetraCoraleye : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ObsidiansWrathQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        [Constructable]
        public PetraCoraleye()
            : base("the Glassblower", "Petra Coraleye")
        {
        }

        public PetraCoraleye(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1055; // Soft tanned skin
            HairItemID = 0x2048; // Long hair
            HairHue = 1153; // Fiery red
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1175, Name = "Molten Silk Blouse" }); // Deep glowing red
            AddItem(new Skirt() { Hue = 1109, Name = "Ash-Dusted Skirt" }); // Dark ash gray
            AddItem(new BodySash() { Hue = 1172, Name = "Glassblower’s Sash" }); // Shimmering orange
            AddItem(new Sandals() { Hue = 2115, Name = "Cinderwalk Sandals" }); // Black with red highlights
            AddItem(new HalfApron() { Hue = 1154, Name = "Obsidian-Stained Apron" }); // Dark reflective black
            AddItem(new FeatheredHat() { Hue = 1175, Name = "Furnace Plume Hat" }); // Matching molten hue

            AddItem(new SmithHammer() { Hue = 1109, Name = "Glassblower’s Hammer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1170;
            backpack.Name = "Tool Satchel";
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
