using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class Counterattack : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Counterattack", "Re An Ort",
                                                        //SpellCircle.Third,
                                                        21004,
                                                        9300,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public Counterattack(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play initial visual and sound effects
                Caster.FixedParticles(0x3728, 1, 13, 5042, EffectLayer.Waist);
                Caster.PlaySound(0x211);

                // Get all creatures within 2 tiles
                ArrayList targets = new ArrayList();
                foreach (Mobile m in Caster.GetMobilesInRange(2))
                {
                    if (m != Caster && Caster.CanBeHarmful(m) && m is BaseCreature) // Check for creatures and ensure they are harmable
                    {
                        targets.Add(m);
                    }
                }

                // Deal damage and apply visual effects
                foreach (Mobile target in targets)
                {
                    Caster.DoHarmful(target);
                    int damage = Utility.RandomMinMax(20, 40); // Heavy damage range
                    SpellHelper.Damage(this, target, damage, 100, 0, 0, 0, 0); // Pure physical damage

                    // Apply damage visual effects to the target
                    target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                    target.PlaySound(0x1F1);
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
