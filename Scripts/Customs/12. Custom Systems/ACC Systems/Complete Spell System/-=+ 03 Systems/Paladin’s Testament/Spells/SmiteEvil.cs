using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class SmiteEvil : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Smite Evil", "Ex Fortis Sanctum",
            21007,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public SmiteEvil(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SmiteEvil m_Owner;

            public InternalTarget(SmiteEvil owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (target == from)
                    {
                        from.SendLocalizedMessage(501857); // You can't target yourself.
                    }
                    else if (!target.Alive)
                    {
                        from.SendLocalizedMessage(501857); // Target is not alive.
                    }
                    else if (!SpellHelper.CanRevealCaster(from))
                    {
                        from.SendLocalizedMessage(501857); // You are still hiding.
                    }
                    else if (!m_Owner.CheckHSequence(target))
                    {
                        m_Owner.FinishSequence();
                    }
                    else
                    {
                        SpellHelper.Turn(from, target);

                        if (IsEvil(target))
                        {
                            // Massive damage calculation
                            double damage = Utility.RandomMinMax(75, 100);
                            AOS.Damage(target, from, (int)damage, 100, 0, 0, 0, 0); // 100% physical damage

                            // Visual and sound effects
                            Effects.SendTargetParticles(target, 0x36BD, 20, 10, 5044, EffectLayer.Head);
                            target.PlaySound(0x29A);

                            from.SendMessage("You smite the evil creature with a powerful blow!");
                        }
                        else
                        {
                            from.SendMessage("This spell is only effective against evil-aligned creatures.");
                        }

                        m_Owner.FinishSequence();
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private static bool IsEvil(Mobile target)
        {
            // Ensure that target is a BaseCreature
            if (target is BaseCreature creature)
            {
                // Check if the creature's karma is negative or any other criteria you consider as 'evil'
                if (creature.Karma < 0)
                {
                    return true;
                }

                // Optionally, add more checks based on your criteria
            }

            return false;
        }
    }
}
