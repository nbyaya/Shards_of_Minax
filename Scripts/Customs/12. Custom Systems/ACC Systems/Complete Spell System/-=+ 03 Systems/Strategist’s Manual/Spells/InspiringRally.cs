using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class InspiringRally : TacticsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Inspiring Rally", "Rally!",
            21004,
            9300
        );

        public override SpellCircle Circle => SpellCircle.Second;
        public override double CastDelay => 0.2;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 25;

        public InspiringRally(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                Effects.PlaySound(Caster.Location, Caster.Map, 0x2D6); // Play battle cry sound effect
                Caster.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Head); // Emit particles to signify the rally

                List<Mobile> allies = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m != Caster && m.Player && m.Alive && m.AccessLevel == AccessLevel.Player)
                    {
                        allies.Add(m);
                    }
                }

                foreach (Mobile ally in allies)
                {
                    ally.SendMessage("You feel inspired by the rallying cry!");
                    ally.FixedEffect(0x373A, 10, 15); // Display a visual effect on each ally
                    ally.PlaySound(0x5C3); // Play a sound effect for each ally

                    // Temporarily boost stats using StatMods
                    ally.AddStatMod(new StatMod(StatType.Str, "InspiringRallyDamageMin", 5, TimeSpan.FromSeconds(30)));
                    ally.AddStatMod(new StatMod(StatType.Str, "InspiringRallyDamageMax", 5, TimeSpan.FromSeconds(30)));
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private InspiringRally m_Owner;

            public InternalTarget(InspiringRally owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                    m_Owner.Target((IPoint3D)targeted);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
