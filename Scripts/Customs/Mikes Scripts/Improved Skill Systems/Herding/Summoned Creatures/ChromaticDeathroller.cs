using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a chromatic deathroller corpse")]
    public class ChromaticDeathroller : BaseCreature
    {
        private DateTime m_NextDeathroll;
        private DateTime m_NextColorShift;
        private int m_CurrentHue;

        [Constructable]
        public ChromaticDeathroller()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a chromatic deathroller";
            Body = 0xCA;
            BaseSoundID = 660;
            m_CurrentHue = 0;
            Hue = m_CurrentHue;

            SetStr(150, 200);
            SetDex(80, 100);
            SetInt(50, 70);

            SetHits(200, 250);
            SetStam(80, 100);
            SetMana(40, 60);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 75.1, 90.0);
            SetSkill(SkillName.Tactics, 80.1, 95.0);
            SetSkill(SkillName.Wrestling, 80.1, 95.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 50;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = -10;

            m_NextDeathroll = DateTime.UtcNow;
            m_NextColorShift = DateTime.UtcNow;
        }

        public ChromaticDeathroller(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 4; } }
        public override int Hides { get { return 20; } }
        public override HideType HideType { get { return HideType.Horned; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextDeathroll)
                {
                    DeathrollAttack();
                }
            }

            if (DateTime.UtcNow >= m_NextColorShift)
            {
                ShiftColor();
            }
        }

        private void DeathrollAttack()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive && InRange(target.Location, 1))
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Deathroll! *");
                PlaySound(0x15A);
                FixedEffect(0x376A, 10, 32);

                int damage = Utility.RandomMinMax(30, 50);
                target.Damage(damage, this);
                target.Stam -= damage;
                target.PlaySound(0x562);

                if (target.Alive)
                {
                    target.Paralyze(TimeSpan.FromSeconds(3));
                }

                m_NextDeathroll = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        private void ShiftColor()
        {
            m_CurrentHue = (m_CurrentHue + 20) % 1001;
            if (m_CurrentHue > 1000 || m_CurrentHue < 2)
                m_CurrentHue = 2;

            Hue = m_CurrentHue;
            m_NextColorShift = DateTime.UtcNow + TimeSpan.FromSeconds(5);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_CurrentHue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_CurrentHue = reader.ReadInt();

            m_NextDeathroll = DateTime.UtcNow;
            m_NextColorShift = DateTime.UtcNow;
        }
    }
}