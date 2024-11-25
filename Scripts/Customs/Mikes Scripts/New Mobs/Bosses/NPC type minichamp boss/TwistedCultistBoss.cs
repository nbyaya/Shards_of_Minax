using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the twisted cultist overlord")]
    public class TwistedCultistBoss : TwistedCultist
    {
        [Constructable]
        public TwistedCultistBoss() : base()
        {
            Name = "Twisted Cultist Overlord";
            Title = "the Supreme Cultist";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Upper range for strength
            SetDex(255); // Upper range for dexterity
            SetInt(250); // Upper range for intelligence

            SetHits(12000); // Enhanced health for boss
            SetDamage(20, 40); // Increased damage range for boss

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 85); // Higher resistances for boss
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 50.0, 100.0); // Increased skills for the boss
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 22500; // Higher fame for boss
            Karma = -22500; // More negative karma

            VirtualArmor = 80; // Enhanced armor for the boss

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

        public TwistedCultistBoss(Serial serial) : base(serial)
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
