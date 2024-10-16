using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;
using Server;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class ProtectiveShield : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Protective Shield", "An Umri",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public ProtectiveShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ProtectiveShield m_Owner;

            public InternalTarget(ProtectiveShield owner) : base(12, false, TargetFlags.Beneficial)
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

        public void Target(Mobile target)
        {
            if (CheckBSequence(target))
            {
                if (target.BeginAction(typeof(ProtectiveShield)))
                {
                    target.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist); // Visual Effect
                    target.PlaySound(0x1F7); // Sound Effect
                    target.SendMessage("You are enveloped in a protective shield!");

                    double shieldAmount = 0.2; // 20% damage absorption
                    TimeSpan duration = TimeSpan.FromSeconds(30);

                    BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Bless, 1075848, duration, target));

                    ShieldTimer timer = new ShieldTimer(target, shieldAmount, duration);
                    timer.Start();
                }
                else
                {
                    Caster.SendMessage("That target is already protected by a shield.");
                }
            }

            FinishSequence();
        }

        private class ShieldTimer : Timer
        {
            private Mobile m_Target;
            private double m_ShieldAmount;

            public ShieldTimer(Mobile target, double shieldAmount, TimeSpan duration) : base(duration)
            {
                m_Target = target;
                m_ShieldAmount = shieldAmount;
            }

            protected override void OnTick()
            {
                m_Target.EndAction(typeof(ProtectiveShield));
                m_Target.SendMessage("The protective shield has worn off.");
            }
        }
    }

    public class DamageModifier
    {
        public static void ApplyShieldDamageReduction(Mobile target, double shieldAmount)
        {
            // Implement damage reduction logic here
            // Example: Store shieldAmount in a property or use it to modify incoming damage
        }
    }
}
