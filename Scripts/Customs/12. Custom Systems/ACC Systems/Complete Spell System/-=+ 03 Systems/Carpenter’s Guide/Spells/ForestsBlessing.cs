using System;
using System.Collections;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class ForestsBlessing : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Forest’s Blessing", "Blesyn Sylvanus",
            21004, // Icon ID
            9300   // Cast Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 3.0; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 35; } }

        public ForestsBlessing(Mobile caster, Item scroll) : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x373A, 10, 15, 5018, 1109, 7, EffectLayer.Waist); // Healing visual effect
                Caster.PlaySound(0x1F7); // Healing sound effect

                ArrayList targets = new ArrayList();

                foreach (Mobile m in Caster.GetMobilesInRange(5)) // Find all mobiles in 5 tiles radius
                {
                    if (m is PlayerMobile && Caster.CanBeBeneficial(m)) // Only heal player characters and beneficial targets
                        targets.Add(m);
                }

                int healAmount = Utility.RandomMinMax(40, 60); // Large healing amount

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    Caster.DoBeneficial(m);
                    m.Heal(healAmount); // Heal the target
                    m.FixedParticles(0x376A, 9, 32, 5005, 1109, 7, EffectLayer.Waist); // Additional visual effect on healed targets
                    m.PlaySound(0x202); // Sound effect on healed targets
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
