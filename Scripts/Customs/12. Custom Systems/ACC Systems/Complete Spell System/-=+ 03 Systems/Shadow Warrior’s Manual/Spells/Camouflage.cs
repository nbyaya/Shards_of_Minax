using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class Camouflage : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Camouflage", "In Vis", // In Vis means "Become Invisible" in Ultima Online language
            21004,
            9300,
            Reagent.Nightshade // Optional reagent if needed
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public Camouflage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!Caster.CanBeginAction(typeof(Camouflage)))
            {
                Caster.SendMessage("You are already camouflaged.");
                return;
            }

            if (CheckSequence())
            {
                Caster.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist); // Visual effect
                Caster.PlaySound(0x659); // Sound effect

                Caster.Hidden = true; // Make the player invisible
                Caster.CantWalk = false;

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerStateCallback(EndEffect), Caster);

                Caster.BeginAction(typeof(Camouflage));
            }

            FinishSequence();
        }

        private void EndEffect(object state)
        {
            Mobile caster = (Mobile)state;

            if (caster == null || caster.Deleted)
                return;

            caster.EndAction(typeof(Camouflage));
            caster.Hidden = false; // End invisibility
            caster.SendMessage("You are no longer camouflaged.");
            caster.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist); // Visual effect
            caster.PlaySound(0x658); // Sound effect
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
