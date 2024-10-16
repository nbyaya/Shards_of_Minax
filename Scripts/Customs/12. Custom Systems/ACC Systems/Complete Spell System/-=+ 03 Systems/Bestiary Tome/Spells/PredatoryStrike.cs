using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class PredatoryStrike : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Predatory Strike", "Ferox Atrox",
                                                        //SpellCircle.Second,
                                                        21005,
                                                        9400,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 22; } }

        public PredatoryStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PredatoryStrike m_Owner;

            public InternalTarget(PredatoryStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && from != null && target != null)
                {
                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        
                        // Calculate damage based on 20% of caster's max hits
                        int damage = (int)(from.HitsMax * 0.2);

                        // Deal damage to the target
                        SpellHelper.Damage(TimeSpan.Zero, target, from, damage);

                        // Visual and sound effects
                        Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 30, 10, 1150, 0);
                        target.PlaySound(0x209);
                    }
                }

                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
