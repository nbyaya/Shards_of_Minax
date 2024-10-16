using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class ShadowSever : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shadow Sever", "Vas Ort",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; } // Choose an appropriate circle
        }

        public override double CastDelay { get { return 0.2; } } // Quick cast
        public override double RequiredSkill { get { return 50.0; } } // Required skill level
        public override int RequiredMana { get { return 25; } } // Mana cost

        public ShadowSever(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ShadowSever m_Owner;

            public InternalTarget(ShadowSever owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        SpellHelper.Turn(from, target);

                        // Apply visual and sound effects
                        Effects.SendLocationEffect(target.Location, target.Map, 0x36BD, 20, 10, 1153, 0);
                        Effects.PlaySound(target.Location, target.Map, 0x208);

                        // Prevent hiding or going invisible for 10 seconds
                        target.AddStatMod(new StatMod(StatType.Dex, "ShadowSeverDex", -10, TimeSpan.FromSeconds(10)));

                        // Apply bleeding effect if the target was attempting to hide
                        if (target.Hidden || target.Combatant == null)
                        {
                            target.Hidden = false;
                            target.SendMessage("You have been revealed by the Shadow Sever!");
                            target.ApplyPoison(from, Poison.Lesser);

                            // Bleeding damage over time (5 damage every 2 seconds for 10 seconds)
                            Timer.DelayCall(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2), 5, () =>
                            {
                                if (target.Alive && !target.Deleted)
                                {
                                    target.Damage(5);
                                    target.FixedEffect(0x374A, 1, 32);
                                }
                            });
                        }
                    }
                }

                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
