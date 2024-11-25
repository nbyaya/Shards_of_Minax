using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Deadlord corpse")]
    public class DeadlordBoss : Deadlord
    {
        [Constructable]
        public DeadlordBoss() : base()
        {
            // Boss name and title changes
            Name = "Deadlord Overlord";
            Title = "the Supreme Deadlord";

            // Enhanced stats to match or exceed Deadlord's capabilities
            SetStr(1200); // Increased strength
            SetDex(255);  // Maxed out dexterity
            SetInt(250);  // Maxed out intelligence

            SetHits(12000); // Increased health to boss-level
            SetDamage(35, 50); // Increased damage range

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Increased skills for higher challenge
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 50000; // Higher fame for a boss
            Karma = -50000; // Negative karma for a boss

            VirtualArmor = 120; // Increased armor

            // Attach XmlRandomAbility for extra dynamic behavior
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

            // Additional boss logic can be added here, but for now it uses the original functionality
        }

        public DeadlordBoss(Serial serial) : base(serial)
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
