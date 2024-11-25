using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Guernsey Guardian Overlord corpse")]
    public class GuernseyGuardianBoss : GuernseyGuardian
    {
        [Constructable]
        public GuernseyGuardianBoss() : base()
        {
            Name = "Guernsey Guardian Overlord";
            Title = "the Supreme Guardian";

            // Update stats to match or exceed Barracoon (or adjust as needed for balance)
            SetStr(1200); // Stronger than original
            SetDex(255); // Max dexterity to make it agile
            SetInt(250); // Higher intelligence for better spell casting

            SetHits(15000); // Much higher health to reflect boss status
            SetDamage(40, 55); // Increased damage range

            // Improved resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Increase the skills to match the boss tier
            SetSkill(SkillName.Anatomy, 90.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 170.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 30000; // Increased fame for a boss
            Karma = -30000; // Increased karma penalty for a boss

            VirtualArmor = 100; // More virtual armor for better defense

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
            // Additional boss logic could be added here if needed (e.g., special abilities)
        }

        public GuernseyGuardianBoss(Serial serial) : base(serial)
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
