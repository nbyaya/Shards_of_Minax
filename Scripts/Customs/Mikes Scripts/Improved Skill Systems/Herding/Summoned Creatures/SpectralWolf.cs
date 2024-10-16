using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a spectral wolf corpse")]
    public class SpectralWolf : BaseCreature
    {
        private DateTime m_NextPhaseShift;
        private DateTime m_NextHowl;
        private bool m_IsPhased;

        [Constructable]
        public SpectralWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a spectral wolf";
            Body = 0x25;
            BaseSoundID = 0xE5;
            Hue = 1153; // Ghostly blue hue

            SetStr(100, 120);
            SetDex(81, 105);
            SetInt(36, 60);

            SetHits(80, 100);

            SetDamage(11, 17);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 60);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 57.6, 75.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 2500;
            Karma = 0;

            VirtualArmor = 22;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -10;

            m_NextPhaseShift = DateTime.UtcNow;
            m_NextHowl = DateTime.UtcNow;
        }

        public SpectralWolf(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 0; } }
        public override int Hides { get { return 0; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool BleedImmune { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextPhaseShift)
                {
                    PhaseShift();
                }

                if (DateTime.UtcNow >= m_NextHowl)
                {
                    SpectralHowl();
                }
            }
        }

        private void PhaseShift()
        {
            if (!m_IsPhased)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The wolf becomes translucent *");
                PlaySound(0x56);
                FixedEffect(0x37C4, 10, 36);

                m_IsPhased = true;
                Hidden = true;
            }
            else
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The wolf materializes *");
                PlaySound(0x57);
                FixedEffect(0x37C4, 10, 36);

                m_IsPhased = false;
                Hidden = false;
            }

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void SpectralHowl()
        {
            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Player && CanBeHarmful(m))
                {
                    m.SendLocalizedMessage(1010524); // The wolf's howl chills your blood.
                    m.PlaySound(0x56);
                    m.FixedParticles(0x3728, 1, 13, 0x26B8, 0x47D, 0x7, EffectLayer.Head);

                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                }
            }

            m_NextHowl = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextPhaseShift = DateTime.UtcNow;
            m_NextHowl = DateTime.UtcNow;
        }
    }
}