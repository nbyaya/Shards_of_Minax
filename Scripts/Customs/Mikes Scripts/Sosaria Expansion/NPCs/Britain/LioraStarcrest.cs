using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SteelStallionQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Steel Stallion"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Liora Starcrest*, Knight Trainer of Castle British, her armor bearing hues of deep iron and sunlit bronze.\n\n" +
                    "She sharpens a spike-studded horseshoe, her eyes narrowing at the echo of distant hoofbeats.\n\n" +
                    "“We’ve trained warriors in these halls for centuries. Yet now… something ancient has breached our grounds. *A stallion of steel and shadow,* galloping through Vault 44, trampling dummies as if they were leaves.”\n\n" +
                    "“My squires found hoofprints through the reinforced gates—*gates meant to withstand siege*. I fear this creature is more than rogue machinery—it’s a relic of the Planar Imperium, unbound and raging.”\n\n" +
                    "“Take these forged spikes, meant to pierce its impervious hide. Slay the **ImperiumSteed** before it turns our training sanctum into ruin.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I shall keep forging. But steel alone may not hold it back for long...";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The steed still runs? My squires dare not enter the vault now. They fear its charge in every shadow.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So the beast falls, and our grounds breathe easy once more.\n\n" +
                       "You’ve proven your strength beyond measure. Take this: *Veinspike*—its edge honed for those who face monsters without flinching.";
            }
        }

        public SteelStallionQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ImperiumSteed), "ImperiumSteed", 1));
            AddReward(new BaseReward(typeof(Veinspike), 1, "Veinspike"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Steel Stallion'!");
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

    public class LioraStarcrest : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SteelStallionQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSwordWeapon());
        }

        [Constructable]
        public LioraStarcrest()
            : base("the Knight Trainer", "Liora Starcrest")
        {
        }

        public LioraStarcrest(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 80);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Metallic silver-blue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateHelm() { Hue = 2401, Name = "Sunforged Helm" });
            AddItem(new PlateChest() { Hue = 2425, Name = "Starcrest Plate" });
            AddItem(new PlateArms() { Hue = 2425, Name = "Spiked Armguards" });
            AddItem(new PlateGloves() { Hue = 2405, Name = "Tempered Grip" });
            AddItem(new PlateLegs() { Hue = 2425, Name = "Knight’s March Greaves" });
            AddItem(new Cloak() { Hue = 1150, Name = "Banner of the Vault" });
            AddItem(new Boots() { Hue = 1109, Name = "Forge-Touched Boots" });
            AddItem(new Broadsword() { Hue = 1157, Name = "Trainer’s Edge" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Trainer's Satchel";
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
