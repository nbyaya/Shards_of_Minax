using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class WhirlwindAttack : TacticsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Whirlwind Attack", "Spin Maximus",
            21004, // Icon ID
            9300   // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }  // Delay before the ability can be used again
        public override double RequiredSkill { get { return 60.0; } }  // Required skill level to use the ability
        public override int RequiredMana { get { return 40; } }  // Mana cost of the ability

        public WhirlwindAttack(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster == null || Caster.Deleted || !Caster.Alive)
                return;

            // Check if the caster has enough mana and meets the requirements
            if (CheckSequence())
            {
                // Play sound and display visual effects
                Caster.PlaySound(9300); // Sound effect
                Caster.FixedParticles(0x3779, 1, 30, 9960, 0, 0, EffectLayer.Waist); // Visual effect

                // Get all mobiles around the caster within a 2-tile radius
                List<Mobile> targets = new List<Mobile>();
                foreach (Mobile m in Caster.GetMobilesInRange(2))
                {
                    if (m != Caster && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

                // Perform attack on all nearby enemies
                foreach (Mobile target in targets)
                {
                    Caster.DoHarmful(target);

                    // Damage calculation - can be adjusted for balance
                    int damage = Utility.RandomMinMax(10, 20) + (int)(Caster.Skills[SkillName.Swords].Value / 10);
                    target.Damage(damage, Caster);

                    // Chance to knock down the enemy
                    if (Utility.RandomDouble() < 0.25) // 25% chance to knock down
                    {
                        target.Animate(21, 6, 1, true, false, 0); // Knock down animation
                        target.SendMessage("You have been knocked down by the whirlwind attack!");
                        target.Freeze(TimeSpan.FromSeconds(2.0)); // Freeze target for 2 seconds
                    }

                    // Additional visual effect on each target
                    target.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Waist);
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);  // Cooldown before the ability can be used again
        }
    }
}
