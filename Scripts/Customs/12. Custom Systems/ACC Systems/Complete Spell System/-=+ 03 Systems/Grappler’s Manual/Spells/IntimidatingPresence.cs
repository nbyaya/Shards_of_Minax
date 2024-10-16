using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class IntimidatingPresence : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Intimidating Presence", "Unleash Fear!",
            //SpellCircle.Second,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 15; } }

        public IntimidatingPresence(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Get all creatures within 5 tiles
                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m is BaseCreature && m.Alive && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

                if (targets.Count > 0)
                {
                    foreach (Mobile target in targets)
                    {
                        Caster.DoHarmful(target);

                        // Apply lesser poison
                        target.ApplyPoison(Caster, Poison.Lesser);

                        // Flashy visual effect and sound
                        target.FixedParticles(0x374A, 10, 30, 5052, EffectLayer.Waist);
                        target.PlaySound(0x1FE);
                    }

                    // Central flashy visual effect and sound
                    Caster.FixedParticles(0x375A, 10, 30, 5052, EffectLayer.Waist);
                    Caster.PlaySound(0x21);

                    Caster.SendMessage("You unleash an intimidating presence, poisoning your enemies!");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
