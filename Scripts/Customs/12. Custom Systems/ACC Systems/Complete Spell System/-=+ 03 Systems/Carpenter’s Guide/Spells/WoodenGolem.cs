using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class WoodenGolem : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Wooden Golem", "Come Wood!",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 40; } }

        public WoodenGolem(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    BaseCreature woodenGolem = new WoodGolem();

                    SpellHelper.Summon(woodenGolem, Caster, 0x215, TimeSpan.FromSeconds(5.0 * Caster.Skills[CastSkill].Value), false, false);

                    // Add some visual and sound effects
                    Effects.SendLocationEffect(woodenGolem.Location, woodenGolem.Map, 0x376A, 10, 1, 1153, 0); // Explosion effect
                    Effects.PlaySound(woodenGolem.Location, woodenGolem.Map, 0x1FB); // Summon sound
                }
                catch
                {
                    Caster.SendMessage("The summoning failed.");
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
