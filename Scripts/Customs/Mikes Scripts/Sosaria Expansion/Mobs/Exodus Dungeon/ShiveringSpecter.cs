using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("the remains of a shivering specter")]
    public class ShiveringSpecter : BaseCreature, IAuraCreature
    {
        private DateTime m_NextWail;
        private DateTime m_NextPhaseShift;
        private DateTime m_NextFrostCurse;

        [Constructable]
        public ShiveringSpecter()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Shivering Specter";
            Body = 0x590; // Uses the same model as Frost Mite
            Hue = 0x47E;  // Icy spectral blue
            Female = false;

            BaseSoundID = 0x4E8;

            SetStr(900, 1100);
            SetDex(180, 210);
            SetInt(500, 600);

            SetHits(1000, 1400);
            SetMana(1200);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Cold, 90);
            SetDamageType(ResistanceType.Energy, 10);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 95, 100);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 110.0, 130.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 85.0, 100.0);
            SetSkill(SkillName.Focus, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 85;

            m_NextWail = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(15, 30));
            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 20));
            m_NextFrostCurse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 40));
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextWail)
                    WailOfLostSouls();

                if (DateTime.UtcNow >= m_NextPhaseShift)
                    PhaseShift();

                if (DateTime.UtcNow >= m_NextFrostCurse)
                    CastFrostCurse();
            }
        }

        private void WailOfLostSouls()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Shivering Specter unleashes a soul-wail!*");
            PlaySound(0x4E5);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 30), 0, 100, 0, 0, 0);

                    if (m is Mobile target)
                    {
                        target.Mana -= Utility.Random(10, 30);
                        target.SendMessage("You feel your life essence drain as the specter screams!");
                    }
                }
            }

            m_NextWail = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 40));
        }

		private void PhaseShift()
		{
			PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The specter fades from this plane...*");
			PlaySound(0x5C6);

			Hidden = true;
			Blessed = true;

			Timer.DelayCall(TimeSpan.FromSeconds(4), () =>
			{
				Hidden = false;
				Blessed = false;

				Effects.SendLocationParticles(this, 0x3728, 10, 15, Hue, 0, 5040, 0);
				PublicOverheadMessage(MessageType.Regular, 0x480, true, "*...and returns, more frigid than before.*");
				PlaySound(0x4E6);
			});

			m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(60);
		}


        private void CastFrostCurse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*A spectral chill creeps into your bones...*");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.SendMessage("You are frozen in fear by the specter's curse!");
                }
            }

            m_NextFrostCurse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(30, 60));
        }

        public void AuraEffect(Mobile m)
        {
            m.FixedParticles(0x374A, 10, 30, 5052, Hue, 0, EffectLayer.Waist);
            m.PlaySound(0x5C6);

            m.SendMessage("The freezing aura of the Shivering Specter saps your vitality!");
            m.Stam -= Utility.Random(5, 10);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 6);

        }

        public override int GetAngerSound() => 0x4E8;
        public override int GetIdleSound() => 0x4E7;
        public override int GetAttackSound() => 0x4E6;
        public override int GetHurtSound() => 0x4E9;
        public override int GetDeathSound() => 0x4E5;

        public ShiveringSpecter(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
