using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class CalamitousFrenzy : ProvocationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Calamitous Frenzy", "In Vas Grav Hur",
            // SpellCircle.Fifth,
            21005,
            9500,
            false
        );

        public override SpellCircle Circle { get { return SpellCircle.Fifth; } }
        public override double CastDelay { get { return 3.0; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 30; } }

        public CalamitousFrenzy(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
            else
            {
                Caster.SendMessage("The spell fizzles.");
                FinishSequence();
            }
        }

		public void Target(IPoint3D p)
		{
			if (!Caster.CanSee(p))
			{
				Caster.SendLocalizedMessage(500237); // Target can not be seen.
				return;
			}

			if (!SpellHelper.CheckTown(p, Caster) || !CheckSequence())
			{
				FinishSequence();
				return;
			}

			SpellHelper.Turn(Caster, p);
			SpellHelper.GetSurfaceTop(ref p);

			Point3D loc = new Point3D(p);
			Map map = Caster.Map;

			if (map == null)
				return;

			// Play chaos visuals and sounds
			Effects.PlaySound(loc, map, 0x307); // Chaotic sound
			Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x3709, 1, 30, 997, 3, 9917, 0); // Sparkle effects

			ArrayList list = new ArrayList();

			foreach (Mobile m in map.GetMobilesInRange(loc, 5))
			{
				if (m != Caster && Caster.CanBeHarmful(m, false))
				{
					list.Add(m);
				}
			}

			for (int i = 0; i < list.Count; ++i)
			{
				Mobile m = (Mobile)list[i];
				if (m is BaseCreature && m.Alive && !m.IsDeadBondedPet)
				{
					BaseCreature bc = (BaseCreature)m;

					if (Utility.RandomDouble() < 0.5) // 50% chance to turn enemies against each other
					{
						bc.Combatant = list[Utility.Random(list.Count)] as Mobile;
						m.SendMessage("You are filled with a chaotic frenzy!");
						Effects.SendTargetParticles(m, 0x3709, 1, 30, 997, 3, 9917, EffectLayer.Head, 0);
					}

					// Reduce overall effectiveness
					m.AddStatMod(new StatMod(StatType.Dex, "CalamitousFrenzyDex", -10, TimeSpan.FromSeconds(20)));
					m.AddStatMod(new StatMod(StatType.Int, "CalamitousFrenzyInt", -10, TimeSpan.FromSeconds(20)));
					m.AddStatMod(new StatMod(StatType.Str, "CalamitousFrenzyStr", -10, TimeSpan.FromSeconds(20)));
					m.SendMessage("You feel your strength waning!");
				}
			}

			FinishSequence();
		}


        private class InternalTarget : Target
        {
            private CalamitousFrenzy m_Owner;

            public InternalTarget(CalamitousFrenzy owner) : base(12, true, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                {
                    m_Owner.Target((IPoint3D)targeted);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
