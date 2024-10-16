using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class WoodlandCamouflage : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Woodland Camouflage", "Vas Ort Sanct",
            21002,
            9302,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 12; } }

        public WoodlandCamouflage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private WoodlandCamouflage m_Owner;

            public InternalTarget(WoodlandCamouflage owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (target is PlayerMobile && m_Owner.CheckSequence())
                    {
                        m_Owner.ApplyEffect(target);
                    }
                    else
                    {
                        from.SendMessage("You can only cast this on yourself or other players.");
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void ApplyEffect(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive)
                return;

            target.SendMessage("You blend into the environment, reducing your visibility.");
            Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
            Effects.PlaySound(target.Location, target.Map, 0x1F8);

            target.Hidden = true;
            target.SendLocalizedMessage(500200); // You are hidden.

            Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
            {
                target.Hidden = false;
                target.SendMessage("You are no longer camouflaged.");
                Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x373A, 10, 15, 5008);
                Effects.PlaySound(target.Location, target.Map, 0x1F9);
            });

            FinishSequence();
        }
    }
}
