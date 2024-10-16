using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class CartographersCurse : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Cartographer’s Curse", "Curse of the Map",
            //SpellCircle.Sixth, // If using spell circles, uncomment and specify the circle.
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; } // Adjust the circle as needed
        }

        public override double CastDelay { get { return 0.2; } } // Casting delay
        public override double RequiredSkill { get { return 70.0; } } // Required skill level to cast
        public override int RequiredMana { get { return 50; } } // Mana cost to cast

        public CartographersCurse(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private CartographersCurse m_Owner;

            public InternalTarget(CartographersCurse owner) : base(10, false, TargetFlags.Harmful)
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
                    m_Owner.ApplyCurse(target);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void ApplyCurse(Mobile target)
        {
            // Effect on main target
            target.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
            target.PlaySound(0x1FB); // Play a curse-like sound
            target.SendMessage("You have been cursed with the Cartographer’s Curse!");

            // Apply damage over time effect to the main target
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(5.0), 5, () => ApplyDamage(target, 10)); // 10 damage every second for 5 seconds

            // Effect on surrounding targets within 1 tile
            List<Mobile> targets = new List<Mobile>();

            foreach (Mobile mob in target.GetMobilesInRange(1))
            {
                if (mob != target && mob != Caster && Caster.CanBeHarmful(mob))
                {
                    targets.Add(mob);
                }
            }

            foreach (Mobile mob in targets)
            {
                Caster.DoHarmful(mob);
                mob.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                mob.PlaySound(0x1FB);
                mob.SendMessage("You have been caught in the Cartographer’s Curse!");
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(5.0), 5, () => ApplyDamage(mob, 10)); // Same DoT effect on nearby mobs
            }

            FinishSequence();
        }

        private void ApplyDamage(Mobile target, int amount)
        {
            if (target.Alive && !target.Deleted)
            {
                target.Damage(amount);
                target.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }

        public TimeSpan GetCooldown()
        {
            return TimeSpan.FromHours(1); // 1 hour cooldown
        }
    }
}
