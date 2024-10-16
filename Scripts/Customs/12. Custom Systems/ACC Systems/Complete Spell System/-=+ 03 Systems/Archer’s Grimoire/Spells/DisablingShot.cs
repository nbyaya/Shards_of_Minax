using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class DisablingShot : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disabling Shot", "Volito Artus",
            // SpellCircle.Sixth,
            21005,
            9301,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 25; } }

        public DisablingShot(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private DisablingShot m_Owner;

            public InternalTarget(DisablingShot owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (!from.CanBeHarmful(target))
                        return;

                    if (m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        
                        // Apply visual and sound effects
                        Effects.SendLocationEffect(target.Location, target.Map, 0x36BD, 20, 10, 0, 0);
                        Effects.PlaySound(target.Location, target.Map, 0x208);

                        // Temporarily reduce target's Dexterity by 90%
                        int dexReduction = (int)(target.Dex * 0.9);
                        int originalDex = target.Dex;
                        target.Dex -= dexReduction;

                        // Timed effect to restore Dexterity
                        Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                        {
                            if (target.Alive && !target.Deleted)
                            {
                                target.Dex = originalDex;
                                target.SendMessage("Your dexterity has returned to normal.");
                            }
                        });

                        target.SendMessage("You feel your dexterity being greatly diminished!");
                    }
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
