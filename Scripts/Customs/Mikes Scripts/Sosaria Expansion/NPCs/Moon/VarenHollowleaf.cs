using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HornOfTheRiftQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Horn of the Rift"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Varen Hollowleaf*, the moonlit herbalist, tending wilting plants.\n\n" +
                    "“My gardens—nourished by the Moon herself—are under siege. A vile creature, the *Mummy Ram*, crashes through my stone fences, its curse sapping the life from everything it touches.\n\n" +
                    "I fear its horn channels the Void’s power. Will you *vanquish the Mummy Ram* and save my sacred flora?”";
            }
        }

        public override object Refuse { get { return "Then I must pray the moonlight hides me well. I cannot endure another raid."; } }

        public override object Uncomplete { get { return "The Mummy Ram still roams? My plants cry for relief—please, hurry!"; } }

        public override object Complete { get { return "You’ve done it... I feel life returning to the soil. Take this, a relic of quiet strength—use it with care."; } }

        public HornOfTheRiftQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MummyRam), "Mummy Ram", 1));
            AddReward(new BaseReward(typeof(SpinesOfTheQuietRebellion), 1, "SpinesOfTheQuietRebellion"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Horn of the Rift'!");
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

    public class VarenHollowleaf : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HornOfTheRiftQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGlassblower()); 
        }

        [Constructable]
        public VarenHollowleaf()
            : base("the Moonlit Herbalist", "Varen Hollowleaf")
        {
        }

        public VarenHollowleaf(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 65, 85);

            Female = false;
            Body = 0x190; // Male Body
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203B; // Long Hair
            HairHue = 2101; // Moonlit White
            FacialHairItemID = 0x204B; // Goatee
            FacialHairHue = 2101;
        }

        public override void InitOutfit()
        {
            // Unique Outfit for Varen Hollowleaf
            AddItem(new HoodedShroudOfShadows() { Hue = 1150, Name = "Veil of Moon’s Grace" }); // Pale silver
            AddItem(new FancyKilt() { Hue = 2419, Name = "Lunar Herbweave Kilt" }); // Soft green, plant-dyed
            AddItem(new ElvenBoots() { Hue = 2401, Name = "Tread of Quiet Earth" }); // Dusty brown
            AddItem(new LeatherGloves() { Hue = 2413, Name = "Gardener's Restraint" }); // Mossy hue
            AddItem(new QuarterStaff() { Hue = 1153, Name = "Moonbranch Staff" }); // Gnarled with silver inlays
            AddItem(new Cloak() { Hue = 1154, Name = "Herbalist’s Whisper Cloak" }); // Subtle grey-blue

            Backpack backpack = new Backpack();
            backpack.Hue = 33;
            backpack.Name = "Pouch of Sacred Seeds";
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
