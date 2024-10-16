using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class QuenchBlade : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Quench Blade", "Frigus Ensis",
            21017,
            9313
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; } // Adjust based on your desired circle
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 25; } }

        public QuenchBlade(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private QuenchBlade m_Owner;

            public InternalTarget(QuenchBlade owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is BaseWeapon weapon)
                {
                    m_Owner.ApplyEffect(weapon);
                }
                else
                {
                    from.SendMessage("You must target a weapon.");
                }
            }
        }

        public void ApplyEffect(BaseWeapon weapon)
        {
            if (!CheckSequence())
                return;

            if (weapon == null || weapon.RootParent != Caster)
            {
                Caster.SendMessage("The weapon must be in your possession to enchant.");
                return;
            }

            Caster.SendMessage("You imbue your weapon with the chilling properties of quenching water.");
            Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x3709, 1, 15, 1153, 2, 9962, 0);
            Effects.PlaySound(Caster.Location, Caster.Map, 0x208);

            // Use a custom effect class or mechanism
            new QuenchBladeEffect(weapon, TimeSpan.FromSeconds(30), 10, 15); // Adjust duration and damage as desired

            FinishSequence();
        }

        private class QuenchBladeEffect : Timer
        {
            private BaseWeapon m_Weapon;
            private int m_ColdDamageBonus;
            private int m_SlowDuration;

            public QuenchBladeEffect(BaseWeapon weapon, TimeSpan duration, int coldDamageBonus, int slowDuration) : base(duration)
            {
                m_Weapon = weapon;
                m_ColdDamageBonus = coldDamageBonus;
                m_SlowDuration = slowDuration;

                // You can use a Dictionary or another approach to associate this effect with the weapon if needed
                Start();
            }

            protected override void OnTick()
            {
                if (m_Weapon != null)
                {
                    // Implement removal of the effect if needed
                    if (m_Weapon.RootParent is Mobile mobile)
                    {
                        mobile.SendMessage("The chilling effect on your weapon fades.");
                    }
                }
            }

            public void OnHit(Mobile target)
            {
                if (target != null && target.Alive)
                {
                    AOS.Damage(target, m_Weapon.RootParent as Mobile, m_ColdDamageBonus, 0, 0, 100, 0, 0); // Cold damage

                    target.SendMessage("You feel a chilling sensation as the blade strikes you.");
                    target.FixedParticles(0x376A, 1, 14, 1153, 2, 9962, 0);
                    target.PlaySound(0x204);

                    // Apply slowing effect
                    target.Paralyze(TimeSpan.FromSeconds(m_SlowDuration));
                }
            }
        }
    }
}
