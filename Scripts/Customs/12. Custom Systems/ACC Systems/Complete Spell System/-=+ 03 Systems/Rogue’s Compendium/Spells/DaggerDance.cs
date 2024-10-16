using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Misc;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class DaggerDance : StealingSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Dagger Dance", "Rapidus Incisio",
            // SpellCircle.Eighth,
            21004,
            9300
        );

        public override SpellCircle Circle => SpellCircle.Eighth;
        public override double CastDelay => 0.1;
        public override double RequiredSkill => 70.0;
        public override int RequiredMana => 10;

        public DaggerDance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x1FB); // Play a flashy attack sound
                Caster.FixedParticles(0x376A, 1, 62, 9943, 1153, 7, EffectLayer.Waist); // Play a swirl of daggers around the caster

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(2))
                {
                    if (m != Caster && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

                if (targets.Count > 0)
                {
                    foreach (Mobile target in targets)
                    {
                        Caster.DoHarmful(target);
                        int damage = Utility.RandomMinMax(10, 30);
                        target.Damage(damage, Caster);

                        target.PlaySound(0x145); // Play a hit sound on the target
                        target.FixedParticles(0x3779, 1, 15, 9943, 67, 7, EffectLayer.Head); // Display a blood splatter effect on the target

                        // Chance to disarm the target
                        if (Utility.RandomDouble() < 0.25) // 25% chance to disarm
                        {
                            Disarm(target);
                        }
                    }
                }
            }

            FinishSequence();
        }

        private void Disarm(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive)
                return;

            Item weapon = target.FindItemOnLayer(Layer.OneHanded) ?? target.FindItemOnLayer(Layer.TwoHanded);

            if (weapon != null)
            {
                target.SendMessage("You have been disarmed!");
                Caster.SendMessage("You disarmed your opponent!");

                target.AddToBackpack(weapon); // Move the weapon to the target's backpack
                target.PlaySound(0x3B9); // Play a disarm sound effect
                target.FixedEffect(0x37B9, 10, 16); // Display a disarm visual effect
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0); // Adjusted for a rapid attack style
        }
    }
}
