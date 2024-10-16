using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class DeathBlossom : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Death Blossom", "Bladus Tornadus",
            21004, 9300 // Animation and Sound IDs
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override int RequiredMana { get { return 20; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }

        public DeathBlossom(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play spinning animation and sound
                Caster.Animate(31, 5, 1, true, false, 0); // Spinning animation
                Caster.PlaySound(0x511); // Spinning sound

                // Get all nearby enemies within a 3-tile radius
                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(3))
                {
                    if (m != Caster && Caster.CanBeHarmful(m) && Caster.InLOS(m))
                        targets.Add(m);
                }

                if (targets.Count > 0)
                {
                    foreach (Mobile target in targets)
                    {
                        Caster.DoHarmful(target);

                        int damage = Utility.RandomMinMax(20, 40);
                        AOS.Damage(target, Caster, damage, 100, 0, 0, 0, 0); // Physical damage

                        target.SendMessage("You are hit by the deadly spinning attack!");
                        target.PlaySound(0x1B9); // Blood effect sound

                        // Apply bleeding effect
                        target.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Waist);
                        BleedEffect(target, Caster, 5, 10); // Apply bleeding effect for 5-10 seconds
                    }

                    // Flashy visual effect at the caster's location
                    Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x3709, 30, 10, 0, 0);
                }
            }

            FinishSequence();
        }

        private void BleedEffect(Mobile target, Mobile caster, int minDuration, int maxDuration)
        {
            int duration = Utility.RandomMinMax(minDuration, maxDuration);
            Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
            {
                if (target != null && !target.Deleted && target.Alive)
                {
                    target.Damage(5, caster); // Deal 5 damage per tick
                    target.SendMessage("You are bleeding!");
                    target.PlaySound(0x19C); // Bleeding sound effect
                    target.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Waist);
                }
            });
        }
    }
}
