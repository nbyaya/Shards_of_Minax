using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a void fiend corpse")]
    public class SummonedVoidFiend : BaseCreature
    {
        private Mobile m_Caster;
        private Timer m_VoidPulseTimer;

        [Constructable]
        public SummonedVoidFiend()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            Name = "Void Fiend";
            Body = 314; // Shadowy daemon body
            BaseSoundID = 0x165;
            Hue = 1175; // Dark void shade


            double scale = 0;

            SetStr((int)(300 * scale), (int)(400 * scale));
            SetDex((int)(80 * scale), (int)(100 * scale));
            SetInt((int)(400 * scale), (int)(500 * scale));

            SetHits((int)(400 * scale), (int)(600 * scale));
            SetDamage((int)(15 * scale), (int)(25 * scale));

            SetDamageType(ResistanceType.Energy, 60);
            SetDamageType(ResistanceType.Physical, 40);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 55);
            SetResistance(ResistanceType.Cold, 40, 55);
            SetResistance(ResistanceType.Poison, 100); // Fully immune
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 75;

            ControlSlots = 4;

            m_VoidPulseTimer = new VoidPulseTimer(this);
            m_VoidPulseTimer.Start();
        }

		public override void OnThink()
		{
			base.OnThink();

			// Eye of the Void: Chance to blind
			if (Combatant != null && Utility.RandomDouble() < 0.05)
			{
				Mobile target = Combatant as Mobile;

				if (target != null && target.Alive && CanBeHarmful(target))
				{
					target.SendMessage(38, "You are blinded by the Eye of the Void!");

					// Apply temporary stat debuffs to simulate blindness
					target.AddStatMod(new StatMod(StatType.Dex, "VoidBlindDex", -10, TimeSpan.FromSeconds(5)));
					target.AddStatMod(new StatMod(StatType.Int, "VoidBlindInt", -10, TimeSpan.FromSeconds(5)));

					// Optional visual/auditory effect
					target.FixedParticles(0x374A, 10, 15, 5032, 1175, 3, EffectLayer.Head);
					target.PlaySound(0x1E3);
				}
			}
		}


        public override bool AutoDispel => false;
        public override bool CanRummageCorpses => false;
        public override bool CanFly => true;

        public override Poison PoisonImmune => Poison.Lethal;

        public override double DispelDifficulty => 125.0;
        public override double DispelFocus => 50.0;

        public override void OnDelete()
        {
            base.OnDelete();
            m_VoidPulseTimer?.Stop();
        }

        public SummonedVoidFiend(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        private class VoidPulseTimer : Timer
        {
            private readonly SummonedVoidFiend m_Fiend;

            public VoidPulseTimer(SummonedVoidFiend fiend)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(6))
            {
                m_Fiend = fiend;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Fiend.Deleted || !m_Fiend.Alive)
                {
                    Stop();
                    return;
                }

                foreach (Mobile m in m_Fiend.GetMobilesInRange(3))
                {
                    if (m != m_Fiend && m_Fiend.CanBeHarmful(m) && m.Alive && m_KarmaCheck(m))
                    {
                        m_Fiend.DoHarmful(m);
                        AOS.Damage(m, m_Fiend, Utility.RandomMinMax(20, 35), 0, 0, 0, 0, 100); // Pure energy
                        m.FixedParticles(0x3709, 10, 30, 5052, 1175, 3, EffectLayer.Head);
                        m.PlaySound(0x208);
                    }
                }
            }

            private bool m_KarmaCheck(Mobile m)
            {
                return m.Karma >= 0 || (m.Player && m != m_Fiend.m_Caster);
            }
        }
    }
}
