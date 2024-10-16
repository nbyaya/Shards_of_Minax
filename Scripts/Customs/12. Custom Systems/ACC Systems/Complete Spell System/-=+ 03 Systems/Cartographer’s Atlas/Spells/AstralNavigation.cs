using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class AstralNavigation : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Astral Navigation", "Vas Rel Por",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 50; } }

        private static TimeSpan m_Duration = TimeSpan.FromMinutes(5.0);
        private static TimeSpan m_Cooldown = TimeSpan.FromHours(1.0);
        private DateTime m_EndTime;
        private bool m_IsActive;

        public AstralNavigation(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (m_IsActive)
            {
                Caster.SendMessage("Astral Navigation is already active.");
                return;
            }

            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
                m_EndTime = DateTime.UtcNow + m_Duration;
                m_IsActive = true;
                Caster.SendMessage("You feel the power of the astral plane flow through you.");
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F7); // Magic sound effect
                Effects.SendLocationParticles(
                    EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration),
                    0x375A, 10, 15, 5015, 0, 0, 0xFFFF); // Astral effect (layer value updated)
            }
            else
            {
                FinishSequence();
            }
        }

        private void EndEffect()
        {
            m_IsActive = false;
            Caster.SendMessage("The power of astral navigation fades away.");
        }

        // Removed override keyword since OnTick() may not exist in the base class
        public void OnTick()
        {
            if (m_IsActive && DateTime.UtcNow >= m_EndTime)
            {
                EndEffect();
            }
        }

        private class InternalTarget : Target
        {
            private AstralNavigation m_Owner;

            public InternalTarget(AstralNavigation owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D p)
                {
                    Point3D target = new Point3D(p);

                    if (SpellHelper.CheckTown(target, from) && m_Owner.CheckSequence())
                    {
                        SpellHelper.Turn(from, p);
                        SpellHelper.GetSurfaceTop(ref p);

                        from.MoveToWorld(new Point3D(p), from.Map);

                        Effects.PlaySound(from.Location, from.Map, 0x1FE); // Teleport sound
                        Effects.SendLocationParticles(
                            EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration),
                            0x3728, 10, 10, 2023, 0, 0, 0xFFFF); // Flash effect (layer value updated)
                    }
                    else
                    {
                        from.SendMessage("You cannot teleport to that location.");
                    }

                    m_Owner.FinishSequence();
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }

        // Removed override keyword since GetCooldown() may not exist in the base class
        public TimeSpan GetCooldown()
        {
            return m_Cooldown;
        }
    }
}
