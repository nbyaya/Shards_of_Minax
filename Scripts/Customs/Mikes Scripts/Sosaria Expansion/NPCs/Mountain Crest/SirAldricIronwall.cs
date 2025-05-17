using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class KnightsLastStandQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Knight's Last Stand"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Sir Aldric Ironwall*, a retired knight clad in weathered armor that still holds a gleam of nobility.\n\n" +
                    "His voice is low, but unwavering:\n\n" +
                    "“I once held the watchtower in the Ice Cavern… before the curse claimed it, and all who stood with me.”\n\n" +
                    "“One of my brothers, now bound in frost and rage, patrols still. His armor echoes through the empty halls like battered bells.”\n\n" +
                    "“Slay the **Frostbound Knight**. Free his soul, and perhaps… grant me peace.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then he marches still, frozen in time. If you hear the bells, turn back, lest you share his fate.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still the bells toll? His watch is unbroken, and the cold grows ever closer.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The bells are silent...\n\n" +
                       "Thank you, brave soul. You’ve given rest to a brother, and honor to a fading line of knights.\n\n" +
                       "Take this: a **Distillation Flask**—may it serve you in taming the arcane, as you have tamed the frost.";
            }
        }

        public KnightsLastStandQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FrostboundKnight), "Frostbound Knight", 1));
            AddReward(new BaseReward(typeof(DistillationFlask), 1, "Distillation Flask"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Knight's Last Stand'!");
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

    public class SirAldricIronwall : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(KnightsLastStandQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHelmetArmor());
        }

        [Constructable]
        public SirAldricIronwall()
            : base("the Retired Knight", "Sir Aldric Ironwall")
        {
        }

        public SirAldricIronwall(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 90, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Pale, frost-kissed tone
            HairItemID = 0x2049; // Long hair
            HairHue = 1150; // Icy silver
            FacialHairItemID = 0x204C; // Full beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 1150, Name = "Knight's Cuirass of Remembrance" }); // Frosted Steel
            AddItem(new PlateLegs() { Hue = 1150, Name = "Frost-Touched Greaves" });
            AddItem(new PlateArms() { Hue = 1150, Name = "Silent Vigil Vambraces" });
            AddItem(new PlateGloves() { Hue = 1150, Name = "Gauntlets of the Last Oath" });
            AddItem(new WingedHelm() { Hue = 1150, Name = "Ironwall’s Crest" });
            AddItem(new Cloak() { Hue = 1153, Name = "Cloak of Eternal Vigil" }); // Light blue, nearly translucent
            AddItem(new Boots() { Hue = 2101, Name = "Knight’s March Boots" }); // Frosty leather tone

            AddItem(new Longsword() { Hue = 2405, Name = "Honor’s Edge" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Knight's Pack";
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
