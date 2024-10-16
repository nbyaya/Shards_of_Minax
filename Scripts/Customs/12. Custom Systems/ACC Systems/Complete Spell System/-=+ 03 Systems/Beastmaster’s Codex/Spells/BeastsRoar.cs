using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class BeastsRoar : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Beast's Roar", "ROAR!",
                                                        21004,
                                                        9300,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public BeastsRoar(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Visual and sound effects for casting the spell
                Caster.FixedParticles(0x3728, 1, 13, 5042, EffectLayer.Waist);
                Caster.PlaySound(0x16B);

                // Find all followers within 5 tiles
                List<Mobile> followers = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m is BaseCreature creature && creature.ControlMaster == Caster)
                    {
                        followers.Add(m);
                    }
                }

                // Apply the strength buff to all followers
                foreach (Mobile follower in followers)
                {
                    BaseCreature creature = follower as BaseCreature;
                    if (creature != null)
                    {
                        int originalStrength = creature.Str;
                        int bonusStrength = (int)(originalStrength * 0.2);
                        
                        // Apply buff
                        creature.Str += bonusStrength;
                        creature.SendMessage("You feel a surge of strength!");

                        // Visual effect on followers
                        creature.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Waist);
                        creature.PlaySound(0x213);

                        // Timer to remove the buff after 30 seconds
                        Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                        {
                            if (creature != null && !creature.Deleted && creature.Alive)
                            {
                                creature.Str -= bonusStrength;
                                creature.SendMessage("The effect of Beast's Roar has worn off.");
                            }
                        });
                    }
                }
            }

            FinishSequence();
        }
    }
}
