using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class RicochetArrow : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ricochet Arrow", "Ricoch",
            //SpellCircle.Second,
            21005,
            9301
        );

        public override SpellCircle Circle => SpellCircle.Second;

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 30.0;
        public override int RequiredMana => 25;

        public RicochetArrow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile target)
        {
            if (target == null || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (SpellHelper.CheckTown(target.Location, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, target);

                Effects.SendMovingEffect(Caster, target, 0xF42, 10, 0, false, false, 0, 0); // Arrow graphic
                Caster.PlaySound(0x5D2); // Arrow release sound

                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    if (target.Alive && !target.Deleted && Caster.CanBeHarmful(target))
                    {
                        AOS.Damage(target, Caster, Utility.RandomMinMax(20, 40), 100, 0, 0, 0, 0); // Main target damage
                        Caster.PlaySound(0x5D3); // Hit sound
                        Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044, 0, 0, 0); // Explosion effect

                        // AOE Damage
                        foreach (Mobile mob in target.GetMobilesInRange(2))
                        {
                            if (mob != target && Caster.CanBeHarmful(mob))
                            {
                                Caster.DoHarmful(mob);
                                AOS.Damage(mob, Caster, Utility.RandomMinMax(10, 20), 100, 0, 0, 0, 0); // AOE damage
                                mob.PlaySound(0x5D1); // Secondary hit sound
                                Effects.SendLocationParticles(EffectItem.Create(mob.Location, mob.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044, 0, 0, 0); // Smaller explosion effect
                            }
                        }
                    }
                });
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private RicochetArrow m_Owner;

            public InternalTarget(RicochetArrow owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    m_Owner.Target(target);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
