using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss frost wapiti corpse")]
    public class BossFrostWapiti : FrostWapiti
    {
        [Constructable]
        public BossFrostWapiti()
            : base() // Calls the base class constructor (FrostWapiti)
        {
            Name = "The Frost Wapiti";
            Title = "the Colossus";
            Hue = 1152; // Unique boss hue (light blue)

            // Enhance stats to make this a stronger version of FrostWapiti
            SetStr(1500); // Stronger strength than the original
            SetDex(250);  // Increased dexterity
            SetInt(350);  // Increased intelligence

            SetHits(15000); // Boss-level health
            SetDamage(40, 50); // Increased damage output

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 110.0, 130.0);
            SetSkill(SkillName.Wrestling, 110.0, 130.0);
            SetSkill(SkillName.Magery, 95.0, 100.0);

            Fame = 35000; // Increased fame
            Karma = -35000; // Increased karma (negative for evil boss)

            VirtualArmor = 100; // Enhanced armor

            Tamable = false; // Boss is not tamable
            ControlSlots = 0; // Not tamable, so no control slots
            MinTameSkill = 0; // Not tamable

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Initialize additional loot drops (including MaxxiaScrolls)
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Drop 5 MaxxiaScrolls
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Retain base loot

            this.Say("I will crush you with the cold of the abyss!");
            PackGold(1000, 2000); // Enhanced gold drops
            PackItem(new IronIngot(Utility.RandomMinMax(50, 100))); // More ingots for a boss-level challenge
            this.AddLoot(LootPack.FilthyRich, 2); // Rich loot pack for the boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original behavior for abilities like FrostBreath, IcicleCharge, etc.
        }

        public BossFrostWapiti(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
