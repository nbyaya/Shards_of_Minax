using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ReapersScytheQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Reaper's Scythe"; } }

        public override object Description
        {
            get
            {
                return
                    "Roland Ironmantle, a master armorer of Grey, sharpens a twisted blade, its edge dull with age but not intent.\n\n" +
                    "His forge flickers in rhythm with a darker pulse—of fear, of history.\n\n" +
                    "“The SkullfangReaper’s shadow is upon us again. Its visits taint our fields, blackening the crops and chilling the soil. I’ve forged wards strong enough to repel its touch, but they’re useless if the beast still breathes.”\n\n" +
                    "“Go to the ruins of Exodus. Find this harbinger and **slay the SkullfangReaper**. Bring me proof—its bone sickle. Only then can the blight be lifted.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the Reaper reap us all. I’ll forge no more for those who flee from darkness.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it stalks our fields? My wards falter, and every blade I make grows colder.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The sickle... still dripping with its curse. You’ve done well.\n\n" +
                       "Take this, **RatsNest**, crafted from the darkest iron and warded against shadows. You’ll need it when next the darkness calls.";
            }
        }

        public ReapersScytheQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SkullfangReaper), "SkullfangReaper", 1));
            AddReward(new BaseReward(typeof(RatsNest), 1, "RatsNest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Reaper's Scythe'!");
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

    public class RolandIronmantle : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ReapersScytheQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith());
        }

        [Constructable]
        public RolandIronmantle()
            : base("the Wardsmith", "Roland Ironmantle")
        {
        }

        public RolandIronmantle(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(95, 100, 75);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Weathered, storm-tanned
            HairItemID = Race.RandomHair(this);
            HairHue = 1109; // Soot-black
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 2101, Name = "Warded Iron Mantle" }); // Dark steel
            AddItem(new StuddedLegs() { Hue = 2105, Name = "Scourge-Bound Greaves" });
            AddItem(new PlateGloves() { Hue = 2117, Name = "Ember-Forged Gauntlets" });
            AddItem(new HalfApron() { Hue = 1154, Name = "Smokebound Apron" }); // Ash-grey
            AddItem(new LeatherCap() { Hue = 1102, Name = "Forge-Helm of Grey" });
            AddItem(new Boots() { Hue = 1824, Name = "Tideworn Boots" });

            AddItem(new ExecutionersAxe() { Hue = 2118, Name = "Anvil-Breaker" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Ward-Forged Pack";
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
