using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FinalWatchQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Final Watch"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Quinlan Shield*, Sentinel Commander of Castle British.\n\n" +
                    "His armor gleams with meticulous care, yet his eyes betray the weight of regret.\n\n" +
                    "“I trained under Sentinel X8—a prototype guardian designed to protect Sosaria’s deepest secrets. But something went wrong. It was never meant to awaken again.”\n\n" +
                    "“Vault 44 has stirred. **Sentinel X8** reactivates, its protocols corrupted. If not stopped, it will lock the vault forever, severing knowledge and trapping anyone within.”\n\n" +
                    "“I bear the override key. But I cannot face him. Not again.”\n\n" +
                    "**Slay VaultSentinelX8** before its reinforcement protocols lock the entire vault.\n\n" +
                    "Return when the Vault’s silence is restored.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then we wait for the lockdown. And pray no one remains inside.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still no word from Vault 44? I fear we’re too late...";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done... X8 has fallen. I can feel the Vault's tension easing.\n\n" +
                       "You’ve severed the final link to a past best forgotten. Take this, *NerdsRage*. It’s a relic from those times—tempered by sorrow, now wielded by hope.";
            }
        }

        public FinalWatchQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(VaultSentinelX8), "VaultSentinelX8", 1));
            AddReward(new BaseReward(typeof(NerdsRage), 1, "NerdsRage"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Final Watch'!");
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

    public class QuinlanShield : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FinalWatchQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWeaponSmith());
        }

        [Constructable]
        public QuinlanShield()
            : base("the Sentinel Commander", "Quinlan Shield")
        {
        }

        public QuinlanShield(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1001; // Pale steel-toned skin
            HairItemID = 0x2048; // Short hair
            HairHue = 1109; // Ash-grey
            FacialHairItemID = 0x204B; // Trimmed beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 2406, Name = "Sentinel's Aegis Chestplate" }); // Dull silver-blue
            AddItem(new PlateArms() { Hue = 2406, Name = "Reinforced Armguards of X8" });
            AddItem(new PlateLegs() { Hue = 2406, Name = "Legacy Greaves" });
            AddItem(new PlateGorget() { Hue = 2406, Name = "Command Gorget" });
            AddItem(new PlateGloves() { Hue = 2406, Name = "Vault-Touched Gauntlets" });
            AddItem(new CloseHelm() { Hue = 2406, Name = "Helm of Final Watch" });
            AddItem(new Cloak() { Hue = 1150, Name = "Sentinel's Cloak of Remembrance" }); // Midnight blue
            AddItem(new Boots() { Hue = 1151, Name = "Steadfast Boots" });

            AddItem(new Broadsword() { Hue = 2500, Name = "Override Blade" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Override Key Satchel";
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
