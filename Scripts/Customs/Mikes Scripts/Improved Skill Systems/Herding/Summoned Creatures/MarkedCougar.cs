using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a marked cougar corpse")]
    public class MarkedCougar : BaseCreature
    {
        private DateTime m_NextPredatorMark;
        private Mobile m_MarkedTarget;

        [Constructable]
        public MarkedCougar()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a marked cougar";
            Body = 63;
            BaseSoundID = 0x73;
            Hue = 1266; // Unique hue for the special cougar

            this.SetStr(200);
            this.SetDex(110);
            this.SetInt(150);

            this.SetDamage(14, 21);

            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Poison, 100);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 50, 60);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 70, 80);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.EvalInt, 90.1, 100.0);
            this.SetSkill(SkillName.Meditation, 90.1, 100.0);
            this.SetSkill(SkillName.Magery, 90.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 90.1, 100.0);
            this.SetSkill(SkillName.Tactics, 100.0);
            this.SetSkill(SkillName.Wrestling, 98.1, 99.0);

            this.VirtualArmor = 58;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_NextPredatorMark = DateTime.UtcNow;
        }

        public MarkedCougar(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 12; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public override void OnThink()
		{
			base.OnThink();

			if (Combatant != null && DateTime.UtcNow >= m_NextPredatorMark)
			{
				// Ensure Combatant is a Mobile before passing to ApplyPredatorMark
				if (Combatant is Mobile)
				{
					ApplyPredatorMark((Mobile)Combatant);
					m_NextPredatorMark = DateTime.UtcNow + TimeSpan.FromMinutes(1);
				}
			}

			if (m_MarkedTarget != null && m_MarkedTarget.Alive)
			{
				// Track and increase damage to the marked target
				TrackMarkedTarget();
				IncreaseDamageToMarkedTarget();
			}
		}


        private void ApplyPredatorMark(Mobile target)
        {
            m_MarkedTarget = target;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Predator's Mark! *");
            PlaySound(0x1E9);
            target.FixedEffect(0x37B9, 10, 20);
        }

        private void TrackMarkedTarget()
        {
            if (m_MarkedTarget != null && m_MarkedTarget.Alive)
            {
                // Implementation of tracking logic
                if (this.CanSee(m_MarkedTarget))
                {
                    this.Combatant = m_MarkedTarget;
                }
            }
        }

        private void IncreaseDamageToMarkedTarget()
        {
            if (Combatant == m_MarkedTarget)
            {
                this.SetDamage(12, 18); // Increase damage when attacking marked target
            }
            else
            {
                this.SetDamage(8, 14); // Reset damage if not attacking marked target
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextPredatorMark);
            writer.Write(m_MarkedTarget);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextPredatorMark = reader.ReadDateTime();
            m_MarkedTarget = reader.ReadMobile();
        }
    }
}
