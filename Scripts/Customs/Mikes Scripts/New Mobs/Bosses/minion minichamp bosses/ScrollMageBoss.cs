using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the supreme scroll mage")]
    public class ScrollMageBoss : ScrollMage
    {
        private TimeSpan m_SpellDelay = TimeSpan.FromSeconds(10.0); // Reduced spell delay for more danger
        public DateTime m_NextSpellTime;

        [Constructable]
        public ScrollMageBoss() : base()
        {
            Name = "Supreme Scroll Mage";
            Title = "the Arcane Overlord";

            // Update stats to match or exceed the ScrollMage's capabilities, making it a more dangerous boss
            SetStr(500, 600); // Enhanced strength
            SetDex(120, 160); // Enhanced dexterity
            SetInt(600, 750); // Enhanced intelligence

            SetHits(2500, 3500); // Increased health for a tougher fight

            SetDamage(20, 30); // Increased damage range

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 50);

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 70, 80);

            // Enhanced skills
            SetSkill(SkillName.EvalInt, 120.0, 130.0);
            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 80.0, 90.0);
            SetSkill(SkillName.Wrestling, 80.0, 90.0);

            Fame = 20000; // Increased fame to reflect its boss status
            Karma = -20000; // Boss with negative karma

            VirtualArmor = 60; // Increased armor

            m_NextSpellTime = DateTime.Now + m_SpellDelay;

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
            if (DateTime.Now >= m_NextSpellTime)
            {
                Mobile target = this.Combatant as Mobile;

                if (target != null && target.Map == this.Map && target.InRange(this, 10))
                {
                    CastRandomScrollSpell(target);
                    m_NextSpellTime = DateTime.Now + m_SpellDelay;
                }
            }

            base.OnThink();
        }

        private void CastRandomScrollSpell(Mobile target)
        {
            // You can add specific spell casting logic here
            // For example, it could cast damaging or debuffing spells at the target
        }

        public ScrollMageBoss(Serial serial) : base(serial)
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
