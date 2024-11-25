using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the librarian overlord")]
    public class LibrarianCustodianBoss : LibrarianCustodian
    {
        private TimeSpan m_SummonDelay = TimeSpan.FromSeconds(30.0); // time between summons
        public DateTime m_NextSummonTime;

        [Constructable]
        public LibrarianCustodianBoss() : base()
        {
            Name = "Librarian Overlord";
            Title = "the Supreme Custodian";
            Hue = 0x83EC; // Adjusted hue for boss appearance

            // Update stats to match or exceed Barracoon
            SetStr(800, 1200); // Higher strength for a stronger boss
            SetDex(150, 200);  // Higher dexterity
            SetInt(600, 750);  // Higher intelligence

            SetHits(12000);    // High hit points for a boss-tier encounter
            SetDamage(25, 40); // Enhanced damage

            // Set resistance values for a harder challenge
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 22500;      // Higher fame to indicate boss status
            Karma = -22500;    // Negative karma for an evil NPC

            VirtualArmor = 60; // Higher virtual armor for greater defense

            // Attach a random ability for more dynamic behavior
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_NextSummonTime = DateTime.Now + m_SummonDelay; // Reset summon timer
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();
            // Add custom behavior or extra logic here if needed
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

        public LibrarianCustodianBoss(Serial serial) : base(serial)
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
