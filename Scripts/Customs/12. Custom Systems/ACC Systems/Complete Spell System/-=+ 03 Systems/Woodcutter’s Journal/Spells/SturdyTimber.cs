using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server;
using Server.Engines.Craft;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class SturdyTimber : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Sturdy Timber", "Saha Ohti",
            21011,
            9311,
            false,
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } } // 1.5 seconds cast delay
        public override double RequiredSkill { get { return 50.0; } } // Required skill level
        public override int RequiredMana { get { return 18; } } // Mana cost

        public SturdyTimber(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Consume reagents and mana
                if (this.Scroll != null)
                    Scroll.Consume();

                Caster.Mana -= RequiredMana;

                // Apply defensive buff
                Caster.SendMessage("You channel the strength of trees, bolstering your defenses!");
                Caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                Caster.PlaySound(0x1ED);

                // Apply a defense buff for 20 seconds
                new DefensiveBuffTimer(Caster).Start();

                // Visual effect of the spell
                Effects.SendLocationParticles(Caster, 0x375A, 10, 15, 5037, 0, 0, 0);
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1ED);
            }

            FinishSequence();
        }

        private class DefensiveBuffTimer : Timer
        {
            private Mobile m_Caster;
            private ResistanceMod m_PhysicalResistanceMod;

            public DefensiveBuffTimer(Mobile caster) : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(20))
            {
                m_Caster = caster;

                // Apply the resistance buff
                m_PhysicalResistanceMod = new ResistanceMod(ResistanceType.Physical, 10); // Increase physical resistance by 10
                m_Caster.AddResistanceMod(m_PhysicalResistanceMod);

                m_Caster.SendMessage("Your defenses have been bolstered!");
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                // Remove the resistance buff after 20 seconds
                m_Caster.SendMessage("The strength of the trees fades away.");
                m_Caster.RemoveResistanceMod(m_PhysicalResistanceMod);

                Stop();
            }
        }
    }
}
