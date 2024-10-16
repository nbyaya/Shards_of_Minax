using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class AxeCleave : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Axe Cleave", "Sweeping Axe",
            // SpellCircle.First,
            21009,
            9309,
            false
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 30; } }

        public AxeCleave(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You perform a powerful sweeping attack with your axe!");

                List<Mobile> targets = new List<Mobile>();

                // Define the effect range and direction of the cleave
                Map map = Caster.Map;
                if (map == null)
                    return;

                IPooledEnumerable eable = Caster.GetMobilesInRange(3);
                foreach (Mobile m in eable)
                {
                    if (m != Caster && Caster.CanBeHarmful(m, false) && Caster.InLOS(m) && IsInCleaveArc(m))
                    {
                        targets.Add(m);
                    }
                }
                eable.Free();

                if (targets.Count > 0)
                {
                    foreach (Mobile m in targets)
                    {
                        Caster.DoHarmful(m);
                        m.PlaySound(0x22F); // Sound effect for being hit
                        m.FixedParticles(0x3728, 1, 13, 9915, 1153, 7, EffectLayer.Waist); // Visual effect
                        int damage = (int)(Caster.Skills[SkillName.Swords].Value * 0.5 + Utility.RandomMinMax(10, 20));
                        AOS.Damage(m, Caster, damage, 100, 0, 0, 0, 0); // Physical damage
                    }

                    Caster.PlaySound(0x507); // Sound effect for swinging the axe
                    Caster.FixedParticles(0x3728, 1, 13, 9915, 1153, 7, EffectLayer.Waist); // Sweeping visual effect
                }
                else
                {
                    Caster.SendMessage("There are no enemies in range to cleave.");
                }
            }

            FinishSequence();
        }

        private bool IsInCleaveArc(Mobile target)
        {
            // Define a 90-degree arc in front of the caster
            double arc = 90.0;
            double angle = Math.Abs(Caster.GetDirectionTo(target) - Caster.Direction);
            return (angle <= arc / 2 || angle >= 360 - arc / 2);
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
