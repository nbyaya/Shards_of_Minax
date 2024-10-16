using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class MaceDance : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mace Dance", "Flury of hits!",
            21004, // Icon ID
            9300   // Cast sound
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public MaceDance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster is PlayerMobile && CheckSequence())
            {
                Caster.SendMessage("You begin to dance with your mace, striking rapidly!");

                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () => PerformMaceDance(Caster));
            }
            FinishSequence();
        }

        private void PerformMaceDance(Mobile caster)
        {
            if (caster == null || caster.Deleted || !caster.Alive)
                return;

            List<Mobile> targets = new List<Mobile>();

            foreach (Mobile m in caster.GetMobilesInRange(1))
            {
                if (m != caster && caster.CanBeHarmful(m))
                {
                    targets.Add(m);
                }
            }

            if (targets.Count > 0)
            {
                caster.MovingEffect(targets[0], 0xF5E, 7, 1, false, false, 0, 0);
                caster.PlaySound(0x1F5);

                int damage = 5;
                double damageMultiplier = 1.0;

                for (int i = 0; i < targets.Count; i++)
                {
                    Mobile target = targets[i];
                    damageMultiplier += 0.2; // Increase damage by 20% per hit
                    int finalDamage = (int)(damage * damageMultiplier);

                    caster.DoHarmful(target);
                    target.Damage(finalDamage, caster);

                    Effects.SendTargetParticles(target, 0x376A, 10, 15, 5013, EffectLayer.Waist);
                    target.PlaySound(0x1E0);
                }

                caster.SendMessage("You unleash a flurry of strikes, each one more powerful than the last!");
            }
            else
            {
                caster.SendMessage("There are no targets in range to strike.");
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0); // Slight delay to balance the rapid strikes
        }
    }
}
