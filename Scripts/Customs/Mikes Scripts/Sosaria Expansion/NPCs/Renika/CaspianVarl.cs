using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ElitistsEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Elitist’s End"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Caspian Varl*, the Master Blacksmith of Renika, his face darkened with soot and worry.\n\n" +
                    "He grips a glowing ingot, veins of darkness pulsing within.\n\n" +
                    "“This ore... it’s cursed. Came from a convoy ambushed in the high passes. The MountainElite took our best miners, left us with poisoned metal.”\n\n" +
                    "“I forge blades for Renika’s guard. I need pure ore, not this corrupted trash. But we can't cleanse it—not while *he* lives.”\n\n" +
                    "“The *MountainElite*. A warrior of stone and wrath, guarding the Stronghold where our convoy fell.”\n\n" +
                    "“Bring me his head. Only then can I purify the crate and restore the forge’s honor.”\n\n" +
                    "**Slay the MountainElite warrior** in the Mountain Stronghold and let this forge breathe clean again.";
            }
        }

        public override object Refuse
        {
            get { return "Then take care, traveler. The taint in this ore seeps deeper each day."; }
        }

        public override object Uncomplete
        {
            get { return "Still he stands? The forge grows colder, and with it, Renika’s hope fades."; }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it.\n\n" +
                       "The MountainElite’s fall lifts the curse from our forge. I can feel it—the metal sings again.\n\n" +
                       "Take this helm: *DaedricWarHelm*. Forged in fire, now blessed by your courage.";
            }
        }

        public ElitistsEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MountainElite), "MountainElite", 1));
            AddReward(new BaseReward(typeof(DaedricWarHelm), 1, "DaedricWarHelm"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Elitist’s End'!");
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

    public class CaspianVarl : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ElitistsEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith());
        }

        [Constructable]
        public CaspianVarl()
            : base("the Master Blacksmith", "Caspian Varl")
        {
        }

        public CaspianVarl(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 95, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // A weathered tan
            HairItemID = 8253; // Short messy hair
            HairHue = 1150; // Deep iron-gray
            FacialHairItemID = 8267; // Full beard
            FacialHairHue = 1150; // Iron-gray
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedDo() { Hue = 2309, Name = "Forge-Kissed Cuirass" }); // Burnished iron tone
            AddItem(new StuddedLegs() { Hue = 2401, Name = "Ember-Touched Greaves" });
            AddItem(new StuddedGloves() { Hue = 2412, Name = "Smith's Iron Grips" });
            AddItem(new LeatherGorget() { Hue = 1824, Name = "Ashen Neckguard" });
            AddItem(new HalfApron() { Hue = 1109, Name = "Renikan Forge Apron" });
            AddItem(new Boots() { Hue = 1813, Name = "Molten-Steel Boots" });

            AddItem(new SmithSmasher() { Hue = 1150, Name = "Ore-Cleaver Hammer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1108;
            backpack.Name = "Smith's Toolpack";
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
