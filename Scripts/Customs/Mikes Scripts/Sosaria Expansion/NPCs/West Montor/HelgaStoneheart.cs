using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RiseAgainstHellDragonQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Rise Against the HellDragon"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Helga Stoneheart*, the indomitable Guard Captain of West Montor.\n\n" +
                    "Her armor scorched and her eyes unwavering, she leans heavily on a blade with strange symbols etched into the steel.\n\n" +
                    "“You’ve heard the cries, haven’t you? The screams in the night? That’s no ordinary beast... that’s the **HellDragon**.”\n\n" +
                    "“Half my garrison’s gone, turned to ash or worse. That monster has risen from the Gate of Hell, and it means to melt our walls and raze our homes.”\n\n" +
                    "“I’ve fought it. Wounded it, with this blade forged near the Pyramid’s cursed edge. But I need you to finish it.”\n\n" +
                    "**Slay the HellDragon** before it turns West Montor to slag.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the flames spare you, because they’ll show me no mercy.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it breathes? I hear its wings even now, stoking the fires at our gates.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done what I could not. The beast is dead, and for now, our walls hold.\n\n" +
                       "**Take this axe**—a symbol of those who fell, and a weapon for those who rise.\n\n" +
                       "The *AlamoDefendersAxe*. Wield it well.";
            }
        }

        public RiseAgainstHellDragonQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(HellDragon), "HellDragon", 1));
            AddReward(new BaseReward(typeof(AlamoDefendersAxe), 1, "AlamoDefendersAxe"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Rise Against the HellDragon'!");
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

    public class HelgaStoneheart : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RiseAgainstHellDragonQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSwordWeapon()); // She is a sword-wielding guard captain
        }

        [Constructable]
        public HelgaStoneheart()
            : base("the Guard Captain", "Helga Stoneheart")
        {
        }

        public HelgaStoneheart(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Ash blonde
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 2101, Name = "Stoneheart's Aegis" }); // Deep iron
            AddItem(new PlateLegs() { Hue = 2101, Name = "Molten-Ward Greaves" });
            AddItem(new PlateArms() { Hue = 2101, Name = "Scorch-Bound Vambraces" });
            AddItem(new PlateGloves() { Hue = 2101, Name = "Ashen Gauntlets" });
            AddItem(new CloseHelm() { Hue = 2115, Name = "Captain’s Inferno Helm" }); // Slightly burnt hue
            AddItem(new BodySash() { Hue = 2075, Name = "Ember-Touched Sash" });
            AddItem(new Cloak() { Hue = 1358, Name = "Banner of West Montor" }); // Dark crimson cloak
            AddItem(new Boots() { Hue = 1109, Name = "Charred March Boots" });

            AddItem(new Broadsword() { Hue = 2117, Name = "Pyramid-Forged Blade" }); // Unusual dark steel hue
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
