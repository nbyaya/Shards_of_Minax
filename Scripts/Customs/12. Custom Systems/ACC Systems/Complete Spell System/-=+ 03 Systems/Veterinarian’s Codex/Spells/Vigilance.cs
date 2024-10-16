using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class Vigilance : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Vigilance", "Vigilia",
            21005,
            9301,
            false,
            Reagent.MandrakeRoot,
            Reagent.Ginseng
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public Vigilance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Effect duration is based on caster's skill level, with a minimum of 10 seconds and a maximum of 60 seconds
                double duration = 10 + (Caster.Skills[CastSkill].Value * 0.5);
                if (duration > 60)
                    duration = 60;

                Caster.SendMessage("You enhance the creature's awareness with vigilance.");

                // Apply the effect to the caster
                Caster.FixedParticles(0x375A, 10, 15, 5017, EffectLayer.Waist);
                Caster.PlaySound(0x1ED);
                Caster.AddStatMod(new StatMod(StatType.Dex, "VigilanceDex", 10, TimeSpan.FromSeconds(duration)));
                Caster.AddStatMod(new StatMod(StatType.Int, "VigilanceInt", 5, TimeSpan.FromSeconds(duration)));

                // Increase detection capabilities
                Caster.Hidden = false; // Makes sure the caster is visible
                Caster.CantWalk = false; // Allows movement if previously restricted

                // Vigilance Buff: Improves detection of stealth and hidden enemies
                VigilanceEffect vigEffect = new VigilanceEffect(Caster, TimeSpan.FromSeconds(duration));
                vigEffect.Start();

                FinishSequence();
            }
        }

        private class VigilanceEffect : Timer
        {
            private Mobile m_Caster;
            private DateTime m_EndTime;

            public VigilanceEffect(Mobile caster, TimeSpan duration) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
                m_Caster = caster;
                m_EndTime = DateTime.Now + duration;
            }

            protected override void OnTick()
            {
                if (DateTime.Now >= m_EndTime || m_Caster == null || m_Caster.Deleted || !m_Caster.Alive)
                {
                    Stop();
                    return;
                }

                // Periodic effect: Enhanced awareness visual and sound effect
                m_Caster.FixedParticles(0x375A, 10, 15, 5017, EffectLayer.Waist);
                m_Caster.PlaySound(0x1ED);

                // Improved detection mechanic
                foreach (Mobile m in m_Caster.GetMobilesInRange(10))
                {
                    if (m.Hidden && m is PlayerMobile && m.Alive && m != m_Caster)
                    {
                        // Chance to detect hidden players
                        if (Utility.RandomDouble() < 0.1 + (m_Caster.Skills[SkillName.DetectHidden].Value * 0.01))
                        {
                            m.RevealingAction();
                            m.SendMessage("You have been revealed by a vigilant creature!");
                        }
                    }
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
