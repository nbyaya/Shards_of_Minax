using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class VanishingBlow : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Vanishing Blow", "In Vis Mort",
            21016,
            9215
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 30; } }

        public VanishingBlow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private VanishingBlow m_Owner;

            public InternalTarget(VanishingBlow owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (!from.CanBeHarmful(target))
                    {
                        from.SendLocalizedMessage(1001083); // You cannot perform that action.
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        int damage = (int)(from.Skills[SkillName.Swords].Value * 0.4);
                        target.Damage(damage, from);

                        Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5022);
                        Effects.PlaySound(target.Location, target.Map, 0x1FE);

                        // Invisibility effect
                        from.Hidden = true;
                        from.SendMessage("You vanish into thin air!");

                        // Move away in a random direction
                        Point3D newLocation = from.GetDistanceToSqrt(target.Location) < 2 ? from.Location : target.Location;
                        from.MoveToWorld(newLocation, from.Map);
                        Effects.PlaySound(from.Location, from.Map, 0x209);

                        Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => { from.Hidden = false; from.SendMessage("You are visible again."); });
                    }

                    m_Owner.FinishSequence();
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

    }
}
