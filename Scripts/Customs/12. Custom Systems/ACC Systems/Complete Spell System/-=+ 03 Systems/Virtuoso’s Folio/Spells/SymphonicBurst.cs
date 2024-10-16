using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class SymphonicBurst : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Symphonic Burst", "In Soundius",
            // SpellCircle.Fourth, (Placeholder for the circle, adjust as necessary)
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } } // 2 seconds cast delay
        public override double RequiredSkill { get { return 50.0; } } // Requires 50 skill points
        public override int RequiredMana { get { return 40; } } // Consumes 40 mana

        private const int DamageRadius = 5; // 5 tile radius
        private const int BaseDamage = 30; // Base damage value

        public SymphonicBurst(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Get the point where the caster is located
                Point3D loc = Caster.Location;
                Map map = Caster.Map;

                if (map == null)
                    return;

                // Create a list to hold affected mobiles
                List<Mobile> targets = new List<Mobile>();

                // Find all mobiles within the damage radius
                foreach (Mobile m in Caster.GetMobilesInRange(DamageRadius))
                {
                    if (Caster.CanBeHarmful(m, false) && Caster != m)
                    {
                        targets.Add(m);
                    }
                }

                // If we have targets, execute the AoE effect
                if (targets.Count > 0)
                {
                    // Play visual and sound effects
                    Effects.PlaySound(loc, map, 0x5C0); // Sound of a loud burst
                    Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x3728, 10, 30, 5052, 0, 0, 0); // Sound wave visual effect

                    foreach (Mobile target in targets)
                    {
                        Caster.DoHarmful(target);
                        int damage = BaseDamage + Utility.RandomMinMax(5, 10); // Base damage with a random modifier

                        // Deal damage to each target
                        SpellHelper.Damage(this, target, damage);

                        // Additional visual effect on each target
                        target.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Head);
                        target.PlaySound(0x5C0);
                    }
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0); // Adjust the cast delay as needed
        }
    }
}
