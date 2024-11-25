using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the locksmith boss")]
    public class LocksmithBoss : Locksmith
    {
        [Constructable]
        public LocksmithBoss() : base()
        {
            Name = "Locksmith Overlord";
            Title = "the Master Locksmith";

            // Update stats to match or exceed Barracoon (or better if applicable)
            SetStr(600); // Better strength than original
            SetDex(450); // Better dexterity than original
            SetInt(300); // Max intelligence

            SetHits(12000); // Boss-tier health
            SetDamage(16, 24); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 60, 80); // Enhanced resistance
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Lockpicking, 120.0, 140.0); // Higher lockpicking skill
            SetSkill(SkillName.Stealth, 100.0, 120.0);    // Enhanced stealth
            SetSkill(SkillName.Hiding, 100.0, 120.0);     // Enhanced hiding
            SetSkill(SkillName.Tactics, 95.0, 115.0);     // Better tactics
            SetSkill(SkillName.Wrestling, 80.0, 100.0);   // Enhanced wrestling

            Fame = 10000;  // Enhanced fame
            Karma = -10000; // Boss-level karma

            VirtualArmor = 50; // Enhanced virtual armor

            // Attach the random ability
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

        public LocksmithBoss(Serial serial) : base(serial)
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
