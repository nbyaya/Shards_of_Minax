using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BeetleBaneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Beetle Bane"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Neris Flintwarden*, the stalwart Watchtower Keeper of East Montor.\n\n" +
                    "Her leathers are singed, eyes sharp beneath soot-streaked brows, and a scent of smoke clings to her like a second skin.\n\n" +
                    "“You smell that? That’s the stench of *failure*. My fires are dying, and with them, our sight over the wilds.”\n\n" +
                    "“A **DrakonBeetle** has made its home in the chimneys above. Sealed 'em tight with slag and shell. The beast’s carapace—hard as dragonbone—turns blades like rain off stone.”\n\n" +
                    "“Our towers stand blind now. If the fires don’t burn, we won’t see what’s coming out of Drakkon’s caves.”\n\n" +
                    "**Exterminate the DrakonBeetle** before East Montor falls into darkness. Bring me peace—and I’ll part with a relic of flame.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then hope you’re quick with a blade in the dark, friend. The beetle waits, and our fires won’t light themselves.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still alive, that beetle? I can feel the cold creeping in, one shuttered flame at a time.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The fires... they burn bright again.\n\n" +
                       "You’ve done more than slay a pest. You’ve saved East Montor’s eyes—and mine.\n\n" +
                       "**Take this: the FireRelic**. It’s forged in heat, meant for those who walk where others dare not.";
            }
        }

        public BeetleBaneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DrakonBeetle), "DrakonBeetle", 1));
            AddReward(new BaseReward(typeof(FireRelic), 1, "FireRelic"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Beetle Bane'!");
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

    public class NerisFlintwarden : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BeetleBaneQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWeaponSmith()); // Watchtower Keeper uses WeaponSmith stock
        }

        [Constructable]
        public NerisFlintwarden()
            : base("the Watchtower Keeper", "Neris Flintwarden")
        {
        }

        public NerisFlintwarden(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1001; // Lightly tanned
            HairItemID = 8255; // Long hair
            HairHue = 1109; // Ash-gray
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2425, Name = "Emberforged Vest" }); // Charcoal gray
            AddItem(new StuddedLegs() { Hue = 2401, Name = "Soot-Stained Leggings" }); // Deep coal
            AddItem(new LeatherGloves() { Hue = 2306, Name = "Firekeeper’s Mitts" }); // Smoldering red
            AddItem(new LeatherGorget() { Hue = 2306, Name = "Ashen Gorget" }); // Smoldering red
            AddItem(new LeatherCap() { Hue = 2411, Name = "Watchfire Helm" }); // Burnished copper
            AddItem(new Boots() { Hue = 1102, Name = "Cinder-Tread Boots" }); // Blackened leather

            AddItem(new WarFork() { Hue = 2413, Name = "Blazepike" }); // Orange-tinged weapon

            Backpack backpack = new Backpack();
            backpack.Hue = 1157;
            backpack.Name = "Flintwarden’s Pack";
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
