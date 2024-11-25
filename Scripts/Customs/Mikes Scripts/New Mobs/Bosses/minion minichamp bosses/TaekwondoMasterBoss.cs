using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the taekwondo grandmaster")]
    public class TaekwondoMasterBoss : TaekwondoMaster
    {
        [Constructable]
        public TaekwondoMasterBoss() : base()
        {
            Name = "Taekwondo Grandmaster";
            Title = "the Ultimate Taekwondo Master";

            // Update stats to match or exceed Barracoon's boss stats
            SetStr(800); // Increased strength for the boss version
            SetDex(400); // Increased dexterity for the boss version
            SetInt(150); // Kept intelligence the same

            SetHits(6000); // Increased health for the boss version

            SetDamage(30, 40); // Increased damage range for the boss version

            SetResistance(ResistanceType.Physical, 75, 85); // Improved physical resistance
            SetResistance(ResistanceType.Fire, 50, 70); // Improved fire resistance
            SetResistance(ResistanceType.Cold, 50, 70); // Improved cold resistance
            SetResistance(ResistanceType.Poison, 60); // Improved poison resistance
            SetResistance(ResistanceType.Energy, 50, 70); // Improved energy resistance

            SetSkill(SkillName.Anatomy, 110.0, 120.0); // Higher skill levels
            SetSkill(SkillName.MagicResist, 100.0, 120.0); // Higher magic resist
            SetSkill(SkillName.Tactics, 110.0, 120.0); // Higher tactics
            SetSkill(SkillName.Wrestling, 110.0, 120.0); // Higher wrestling skill

            Fame = 20000; // Increased fame
            Karma = -20000; // Negative karma for a stronger boss

            VirtualArmor = 60; // Increased armor for the boss version

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
            // Additional boss logic could be added here
        }

        public TaekwondoMasterBoss(Serial serial) : base(serial)
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
