using System;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an earthquake ettin boss corpse")]
    public class EarthquakeEttinBoss : EarthquakeEttin
    {
        [Constructable]
        public EarthquakeEttinBoss() : base()
        {
            Name = "Earthquake Ettin Boss";
            Title = "the Earthquake Overlord";
            Hue = 1563; // Earthy brown hue

            // Update stats to match or exceed Barracoon or better
            SetStr(1200, 1500);  // Higher strength for a boss
            SetDex(255, 300);     // Higher dexterity for agility
            SetInt(250, 350);     // Higher intelligence for more tactical use

            SetHits(12000);       // Higher health than the original
            SetDamage(35, 50);    // Increased damage range

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 110.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 80.0);

            Fame = 32000;
            Karma = -32000;

            VirtualArmor = 100;

            Tamable = false;
            ControlSlots = 0; // Not tamable as a boss

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Add 5 MaxxiaScrolls in addition to the regular loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional logic for the boss could be placed here
        }

        public EarthquakeEttinBoss(Serial serial) : base(serial)
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
