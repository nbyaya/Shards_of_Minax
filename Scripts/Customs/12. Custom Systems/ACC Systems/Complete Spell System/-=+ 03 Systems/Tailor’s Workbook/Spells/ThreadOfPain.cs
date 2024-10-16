using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;
using Server.Engines.Craft;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class ThreadOfPain : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Thread of Pain", "Weave of Anguish",
                                                        21001,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public ThreadOfPain(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ThreadOfPain m_Owner;

            public InternalTarget(ThreadOfPain owner) : base(12, false, TargetFlags.Harmful)
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
                    from.SendLocalizedMessage(500619); // That is not a weapon!
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void ApplyEffect(BaseWeapon weapon)
        {
            if (!CheckSequence())
                return;

            if (Caster.Mana < RequiredMana)
            {
                Caster.SendMessage("You do not have enough mana to perform this spell.");
                return;
            }

            Caster.Mana -= RequiredMana;

            weapon.WeaponAttributes.HitLeechHits = 10; // Example: Apply an effect to the weapon

            Effects.PlaySound(Caster.Location, Caster.Map, 0x20F); // Play sound effect

            Caster.SendMessage("Your weapon is imbued with the Thread of Pain!");

            Timer.DelayCall(TimeSpan.FromSeconds(30), () => RemoveEffect(weapon)); // Effect lasts 30 seconds

            FinishSequence();
        }

        private void RemoveEffect(BaseWeapon weapon)
        {
            // Reset the weapon's attributes back to normal
            weapon.WeaponAttributes.HitLeechHits = 0;
            Caster.SendMessage("The magical thread fades from your weapon.");
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }

    public class ThreadOfPainEffect
    {
        private Mobile m_Caster;
        private BaseWeapon m_Weapon;
        private Timer m_Timer;

        public ThreadOfPainEffect(Mobile caster, BaseWeapon weapon)
        {
            m_Caster = caster;
            m_Weapon = weapon;

            m_Timer = new ThreadOfPainTimer(this);
            m_Timer.Start();
        }

        public void OnHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damage)
        {
            if (Utility.RandomDouble() < 0.25) // 25% chance to trigger effect
            {
                int extraDamage = Utility.RandomMinMax(5, 10);
                defender.Damage(extraDamage, attacker);

                Effects.SendMovingEffect(attacker, defender, 0x36E4, 10, 0, false, false, 0, 0);
                defender.FixedParticles(0x3779, 1, 15, 9941, 32, 0, EffectLayer.Waist); // Visual effect
                defender.PlaySound(0x1F1); // Sound effect

                if (defender is BaseCreature creature)
                {
                    creature.Freeze(TimeSpan.FromSeconds(2.0)); // Slow effect
                }
            }
        }

        private class ThreadOfPainTimer : Timer
        {
            private ThreadOfPainEffect m_Effect;

            public ThreadOfPainTimer(ThreadOfPainEffect effect) : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            {
                m_Effect = effect;
            }

            protected override void OnTick()
            {
                if (m_Effect == null || m_Effect.m_Weapon.Deleted)
                {
                    Stop();
                    return;
                }

                // You can add checks here to see if the weapon is used and apply effects
                // This is a placeholder for the actual effect-checking logic
            }
        }
    }
}
