using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a goliath birdeater boss corpse")]
    public class GoliathBirdeaterBoss : GoliathBirdeater
    {
        [Constructable]
        public GoliathBirdeaterBoss() : base()
        {
            Name = "Goliath Birdeater";
            Title = "the Terrifying Overlord";
            
            // Update stats to match or exceed Barracoon's tier
            SetStr(1200); // Upper strength from Barracoon's stats
            SetDex(255); // Maximum dexterity
            SetInt(250); // Intelligence from original Goliath Birdeater

            SetHits(12000); // Increased health for boss
            SetDamage(40, 60); // Increased damage for boss-tier

            // Resistances adjusted to be tougher than the original Goliath Birdeater
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 85, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Adjusted skills for boss-tier difficulty
            SetSkill(SkillName.MagicResist, 150.0); // Increased magic resist
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling skill

            Fame = 25000; // Increased fame to match boss tier
            Karma = -25000; // Karma reduced for a villainous boss

            VirtualArmor = 100; // Increased virtual armor to make it tougher

            // Attach a random ability for added challenge
            XmlAttach.AttachTo(this, new XmlRandomAbility());
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
            // Additional boss-specific logic can be added here, like special behavior based on health, etc.
        }

        public GoliathBirdeaterBoss(Serial serial) : base(serial)
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
