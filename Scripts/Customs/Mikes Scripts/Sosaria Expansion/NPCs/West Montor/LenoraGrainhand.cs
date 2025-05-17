using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SnuffTheBlazingBouraQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Snuff the BlazingBoura"; } }

        public override object Description
        {
            get
            {
                return
                    "Lenora Grainhand stands tall amid the rolling pastures of West Montor, her sun-kissed arms crossed and eyes fierce.\n\n" +
                    "“My herd’s milk—ruined! Tastes of ash and fire. The **BlazingBoura** did it, I know it. That cursed beast breathes on the winds from the **Gate of Hell**, tainting the land, spoiling my cows!”\n\n" +
                    "“Three generations we’ve worked these fields, made cheeses that folks across Sosaria crave. But this... this monster scorched my best wheels, and now my stock flees at night, fevered and frightened.”\n\n" +
                    "**Hunt down the BlazingBoura**, and let the fires die with it. Do it for the herd. Do it for West Montor.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then you’ll let the flames take what’s left? I’ll not stand idle. I’ll find someone with the heart to protect what we’ve built.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it burns? My herd grows weaker by the day. If you won't finish it, I must find another...";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The BlazingBoura is no more? By the stars...\n\n" +
                       "You've saved more than just my herd—you've saved a legacy.\n\n" +
                       "Take this: **RadiantCrown**. May it shine as brightly as the fire you snuffed out.";
            }
        }

        public SnuffTheBlazingBouraQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlazingBoura), "BlazingBoura", 1));
            AddReward(new BaseReward(typeof(RadiantCrown), 1, "RadiantCrown"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Snuff the BlazingBoura'!");
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

    public class LenoraGrainhand : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SnuffTheBlazingBouraQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFarmer());
        }

        [Constructable]
        public LenoraGrainhand()
            : base("the Dairy Farmer", "Lenora Grainhand")
        {
        }

        public LenoraGrainhand(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2049; // Braided Hair
            HairHue = 2213; // Wheat-blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2401, Name = "Sunbleached Tunic" }); // Light cream
            AddItem(new LongPants() { Hue = 1813, Name = "Ash-Streaked Trousers" }); // Earthy brown
            AddItem(new HalfApron() { Hue = 2425, Name = "Grainhand's Apron" }); // Golden straw
            AddItem(new Boots() { Hue = 1109, Name = "Muckwalker Boots" }); // Dusty grey
            AddItem(new StrawHat() { Hue = 2213, Name = "Lenora's Hat" }); // Wheat-blonde

            AddItem(new Pitchfork() { Hue = 2502, Name = "Scorched Pitchfork" }); // Slightly burned metal

            Backpack backpack = new Backpack();
            backpack.Hue = 1151;
            backpack.Name = "Dairy Satchel";
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
