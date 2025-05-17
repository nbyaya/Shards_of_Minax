using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DominionsFallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Dominion’s Fall"; } }

        public override object Description
        {
            get
            {
                return
                    "Guildmaster *Alric Stonehelm* stands at the dock’s edge, arms crossed over a breastplate dulled by salt and time. His voice is low, almost swallowed by the waves:\n\n" +
                    "“You heard of the *Forsaken Dominion*? Damn thing’s more shadow than man. Miners say its banners fly over the shafts now, spectral and foul. They won’t go near 'em. And without those veins, Grey dies. Simple as that.”\n\n" +
                    "“I’m not one to beg, but I can’t let this stand. Someone has to put that beast down before the mine’s spirit is lost.”\n\n" +
                    "**Slay the Forsaken Dominion**. Bring peace to our dead, and strength to the living.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Aye. Maybe another will step up. But the mine’s lifeblood runs dry, and with it, so does Grey.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Dominion still haunts us? Miners won't last another week. Each night they see more banners... more eyes in the dark.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done, then. The Dominion falls, and with it, our fears.\n\n" +
                       "You’ve not just slain a beast—you’ve saved this town’s soul.\n\n" +
                       "Take *David’s Sling*. May it serve you well, as you’ve served us.";
            }
        }

        public DominionsFallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ForsakenDominion), "Forsaken Dominion", 1));
            AddReward(new BaseReward(typeof(DavidsSling), 1, "David’s Sling"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Dominion’s Fall'!");
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

    public class AlricStonehelm : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DominionsFallQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiner());
        }

        [Constructable]
        public AlricStonehelm()
            : base("the Guildmaster", "Alric Stonehelm")
        {
        }

        public AlricStonehelm(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1107; // Dark slate
            FacialHairItemID = 0x204B; // Full beard
            FacialHairHue = 1107;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 2401, Name = "Salt-Hardened Chestplate" }); // Sea-worn steel
            AddItem(new StuddedArms() { Hue = 2405, Name = "Brine-Soaked Vambraces" });
            AddItem(new StuddedLegs() { Hue = 1816, Name = "Dockside Guard’s Greaves" });
            AddItem(new LeatherGloves() { Hue = 1811, Name = "Ironbound Gloves" });
            AddItem(new Bascinet() { Hue = 1109, Name = "Guildmaster’s Helm" });

            AddItem(new Boots() { Hue = 1812, Name = "Stone-Tread Boots" });
            AddItem(new Cloak() { Hue = 1102, Name = "Tidecloak of Grey" });

            AddItem(new WarHammer() { Hue = 2406, Name = "Mine-Keeper’s Hammer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Guildmaster’s Pack";
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
