using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class NinjasFury : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ninjas Fury", "Shuriken Fujin",
            21004, // GFX of the spell when cast
            9300   // Sound of the spell when cast
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.5; } } // Short cast delay to fit the "fast strikes" description
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public NinjasFury(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private NinjasFury m_Owner;

            public InternalTarget(NinjasFury owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (!from.CanBeHarmful(target))
                        return;

                    from.DoHarmful(target);
                    m_Owner.StartEffect(from, target);
                }
            }
        }

        public void StartEffect(Mobile caster, Mobile target)
        {
            if (CheckSequence())
            {
                caster.SendMessage("You unleash a flurry of strikes on your target!");
                Effects.PlaySound(caster.Location, caster.Map, 0x1F7); // Play initial attack sound

                Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromMilliseconds(200), 5, () =>
                {
                    if (target == null || target.Deleted || !target.Alive || !caster.CanBeHarmful(target))
                        return;

                    int damage = Utility.RandomMinMax(5, 10);
                    caster.DoHarmful(target);
                    AOS.Damage(target, caster, damage, 100, 0, 0, 0, 0);

                    Effects.SendMovingEffect(caster, target, 0xF51, 10, 0, false, false, 0, 0);
                    Effects.SendTargetEffect(target, 0x37B9, 10, 16, 1153, 0); // Flashy effect on each hit
                    Effects.PlaySound(target.Location, target.Map, 0x11C); // Sound for each strike

                    // Chance to stun
                    if (Utility.RandomDouble() < 0.2) // 20% chance to stun
                    {
                        target.Freeze(TimeSpan.FromSeconds(1.5));
                        target.SendMessage("You are stunned by the fury of strikes!");
                        Effects.PlaySound(target.Location, target.Map, 0x204); // Stun sound
                    }
                });
            }

            FinishSequence();
        }
    }
}
