using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the invisible saboteur overlord")]
    public class InvisibleSaboteurBoss : InvisibleSaboteur
    {
        [Constructable]
        public InvisibleSaboteurBoss() : base()
        {
            Name = "Invisible Saboteur Overlord";
            Title = "the Supreme Saboteur";

            // Update stats to match or exceed Barracoon or better
            SetStr(425); // Matching Barracoon's upper strength
            SetDex(400); // Enhanced dexterity
            SetInt(400); // Enhanced intelligence

            SetHits(12000); // High health to make it a boss-tier creature
            SetDamage(15, 25); // Enhanced damage range for boss-level difficulty

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Magery, 100.0, 120.0);  // Enhanced skill range
            SetSkill(SkillName.MagicResist, 100.0, 120.0); // Enhanced magic resistance
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Enhanced wrestling

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            // Attach a random ability to the boss
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
            
            // Extra boss behavior could be added here, if desired
        }

        public InvisibleSaboteurBoss(Serial serial) : base(serial)
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
