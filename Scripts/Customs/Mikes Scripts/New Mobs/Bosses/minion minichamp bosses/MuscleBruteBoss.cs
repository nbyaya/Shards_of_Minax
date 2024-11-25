using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the muscle overlord")]
    public class MuscleBruteBoss : MuscleBrute
    {
        [Constructable]
        public MuscleBruteBoss() : base()
        {
            Name = "Muscle Overlord";
            Title = "the Supreme Brute";

            // Update stats to match or exceed Barracoon
            SetStr(1500); // Enhanced strength for the boss
            SetDex(150); // Retain high dexterity
            SetInt(100); // Enhanced intelligence for higher magic resistance

            SetHits(12000); // Higher health for the boss
            SetDamage(25, 40); // Increased damage range for the boss

            SetResistance(ResistanceType.Physical, 90); // Enhanced physical resistance
            SetResistance(ResistanceType.Fire, 50); // Improved fire resistance
            SetResistance(ResistanceType.Cold, 60); // Improved cold resistance
            SetResistance(ResistanceType.Poison, 100); // Max poison resistance
            SetResistance(ResistanceType.Energy, 50); // Improved energy resistance

            SetSkill(SkillName.Tactics, 120.0); // Higher tactics skill
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling skill
            SetSkill(SkillName.MagicResist, 100.0); // Higher magic resist skill

            Fame = 20000; // Increased fame for the boss
            Karma = -20000; // Increased negative karma for the boss

            VirtualArmor = 80; // Higher virtual armor

            // Attach a random ability
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
            // Additional boss logic can be added here, such as special abilities
        }

        public MuscleBruteBoss(Serial serial) : base(serial)
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
