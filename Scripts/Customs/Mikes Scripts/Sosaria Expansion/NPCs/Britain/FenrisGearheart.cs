using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EngineShutdownQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Engine Shutdown"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Fenris Gearheart*, the castle’s machinist, his attire gleaming with strange metals, the scent of oil heavy in the air.\n\n" +
                    "He taps a strange, humming device on his wrist, eyes narrowing as gears within whir. His voice carries urgency, tinged with technical jargon.\n\n" +
                    "“The Vault’s reactor—**Preservation Vault 44**—is syncing to an unstable rhythm. The **TauEngine** is driving the entire grid to a meltdown. It wasn’t supposed to awaken.”\n\n" +
                    "“I heard it. The hum—it called through the cooling vents I designed. The Engine *thinks*. And now it’s dreaming of fire.”\n\n" +
                    "**Slay the TauEngine**, or it will overload and turn the Vault—and perhaps Castle British—into molten ruin.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "If the TauEngine completes its cycle, there’ll be no turning back. It *will* breach the Vault.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still active? Then time’s against us. Those vents will only hold so long—its heat’s already warping the grid.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You did it? The hum... it’s gone.\n\n" +
                       "**The Vault stabilizes, and the castle breathes again.**\n\n" +
                       "Take this, *HideOfTheHuntersPact*. Fashioned from the Vault’s finest synthhide—it’s stronger now, thanks to you.";
            }
        }

        public EngineShutdownQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(TauEngine), "TauEngine", 1));
            AddReward(new BaseReward(typeof(HideOfTheHuntersPact), 1, "HideOfTheHuntersPact"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Engine Shutdown'!");
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

    public class FenrisGearheart : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(EngineShutdownQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTinker(this)); // Closest to machinist/inventor profession
        }

        [Constructable]
        public FenrisGearheart()
            : base("the Machinist", "Fenris Gearheart")
        {
        }

        public FenrisGearheart(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 80, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Slightly pale, reflective of a life spent indoors
            HairItemID = 0x203C; // Short Hair
            HairHue = 1151; // Metallic silver
            FacialHairItemID = 0x2040; // Short Beard
            FacialHairHue = 1151;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 2418, Name = "Machinist's Vestplate" }); // Brass hue
            AddItem(new StuddedLegs() { Hue = 2305, Name = "Cogwoven Trousers" }); // Grease-dark
            AddItem(new LeatherGloves() { Hue = 2219, Name = "Tinker’s Gauntlets" }); // Burnt copper
            AddItem(new FeatheredHat() { Hue = 2117, Name = "Gear-Studded Cap" }); // Bronze-rimmed
            AddItem(new Boots() { Hue = 1109, Name = "Oil-Slick Boots" }); // Shiny black
            AddItem(new ToolKit() { Name = "Fenris' Portable Ventschematic", Hue = 1154 }); // A prop for his workshop plans

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Machinist's Gearpack";
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
