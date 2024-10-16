using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class CrypticStrike : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Cryptic Strike", "Vis Libri",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public CrypticStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckHSequence(target))
            {
                SpellHelper.Turn(Caster, target);

                // Calculate damage as the difference between target's current hits and max hits
                int damage = target.HitsMax - target.Hits;

                // Ensure the damage is at least 1
                damage = Math.Max(1, damage);

                // Apply damage to target
                SpellHelper.Damage(this, target, damage, 0, 100, 0, 0, 0);

                // Visual and sound effects
                Effects.SendTargetParticles(target, 0x36BD, 20, 10, 5044, EffectLayer.Head);
                target.PlaySound(0x208);

                // Additional flashy effect
                Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 30, 10, 0x47F, 0);
                target.PlaySound(0x1FE);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private CrypticStrike m_Owner;

            public InternalTarget(CrypticStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
