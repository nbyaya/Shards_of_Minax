using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ShadowsInTheVeinsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Shadows in the Veins"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Talia Ashfell*, the stern Ore Inspector of Death Glutch.\n\n" +
                    "Her clothes bear the dust of the forges, and her eyes are sharp with worry. A curious, arcane shimmer flickers at the edge of her pickaxe, as if the very ore she inspects has begun to change.\n\n" +
                    "“The veins beneath Malidor’s Academy are spoiled... tainted.”\n\n" +
                    "“My readings show *magical corruption* in the ore that feeds our forges—it’s weakening the steel, and with it, my standing here.”\n\n" +
                    "“Years ago, my mentor went below to track the source. Never returned. Now the miners whisper of a **beast**, burrowing through the cursed stone—feeding on magic.”\n\n" +
                    "“Slay this **Arcane Antlion**. Restore the veins, and I’ll see you’re well rewarded.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then watch your step around the forges, stranger. Weak metal snaps easy, and so do reputations.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still the corruption spreads? The veins grow thin, and the forges colder. If you fear the creature, say so.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It's done? The veins hum with life again... cleaner, stronger. You’ve done what none dared since my mentor fell.\n\n" +
                       "**Here**—*HlaaluTrader’s Cuffs*. Crafted for fair hands, but earned by blood. Wear them knowing the veins owe you now.";
            }
        }

        public ShadowsInTheVeinsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ArcaneAntlion), "Arcane Antlion", 1));
            AddReward(new BaseReward(typeof(HlaaluTradersCuffs), 1, "HlaaluTrader’s Cuffs"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Shadows in the Veins'!");
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

    public class TaliaAshfell : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ShadowsInTheVeinsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiner());
        }

        [Constructable]
        public TaliaAshfell()
            : base("the Ore Inspector", "Talia Ashfell")
        {
        }

        public TaliaAshfell(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 75, 45);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Long Hair
            HairHue = 1107; // Ashen Gray
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 2406, Name = "Ashvein Tunic" }); // Dark ash-grey
            AddItem(new LeatherLegs() { Hue = 2301, Name = "Stonebound Leggings" }); // Faded charcoal
            AddItem(new LeatherGloves() { Hue = 1109, Name = "Ore-Touched Gloves" }); // Dusty gray
            AddItem(new Bandana() { Hue = 1153, Name = "Vein-Seeker's Bandana" }); // Deep violet, symbolizing her pursuit
            AddItem(new Boots() { Hue = 1811, Name = "Forgewalkers" }); // Soot-black

            AddItem(new Pickaxe() { Hue = 2115, Name = "Ashfell’s Measure" }); // Slight purple hue, hinting at magical residue

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Inspector’s Kit";
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
