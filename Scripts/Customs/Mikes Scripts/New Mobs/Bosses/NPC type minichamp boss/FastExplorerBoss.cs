using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Fast Explorer Overlord")]
    public class FastExplorerBoss : FastExplorer
    {
        [Constructable]
        public FastExplorerBoss() : base()
        {
            Name = "Fast Explorer Overlord";
            Title = "the Supreme Adventurer";

            // Update stats to match or exceed the original
            SetStr(425); // Strength for a boss-level opponent
            SetDex(200); // Increased dexterity for better speed and agility
            SetInt(100); // Increased intelligence for more strategic abilities

            SetHits(12000); // Matching Barracoon's health for a boss
            SetDamage(35, 50); // Increased damage for a stronger boss

            SetResistance(ResistanceType.Physical, 70);
            SetResistance(ResistanceType.Fire, 60);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 70);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80; // Increased armor for better protection

            ActiveSpeed = 0.01;   // Increased speed for a boss-like quick movement
            PassiveSpeed = 0.02;  // Faster movement

            // Attach a random ability for added boss complexity
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Add a more fitting horse or mount if necessary
            AddHorse();
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Boss-specific AI or behavior can be added here
        }

        public FastExplorerBoss(Serial serial) : base(serial)
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
