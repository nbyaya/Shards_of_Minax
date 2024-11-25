using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the battle dressmaker overlord")]
    public class BattleDressmakerBoss : BattleDressmaker
    {
        private TimeSpan m_EnhanceDelay = TimeSpan.FromSeconds(15.0); // time between enhancements
        public DateTime m_NextEnhanceTime;

        [Constructable]
        public BattleDressmakerBoss() : base()
        {
            Name = "Battle Dressmaker Overlord";
            Title = "the Supreme Dressmaker";

            // Update stats to match or exceed a boss-level entity
            SetStr(800); // Increased strength to match boss-tier stats
            SetDex(200); // Increased dexterity to match boss-tier stats
            SetInt(300); // Higher intelligence for the boss

            SetHits(12000); // Boss-level health
            SetDamage(25, 35); // Enhanced damage for boss-level challenge

            SetResistance(ResistanceType.Physical, 75); // Higher resistances
            SetResistance(ResistanceType.Fire, 70);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 60);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 100.0); // Higher magic resist for bosses
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics skill
            SetSkill(SkillName.Wrestling, 100.0); // Higher wrestling skill

            Fame = 22500; // Boss-level fame
            Karma = -22500; // Boss-level karma

            VirtualArmor = 70; // Increased virtual armor

            // Attach the XmlRandomAbility to provide extra random features
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_NextEnhanceTime = DateTime.Now + m_EnhanceDelay;
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
            // Add additional logic if needed for the boss
        }

        public BattleDressmakerBoss(Serial serial) : base(serial)
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
