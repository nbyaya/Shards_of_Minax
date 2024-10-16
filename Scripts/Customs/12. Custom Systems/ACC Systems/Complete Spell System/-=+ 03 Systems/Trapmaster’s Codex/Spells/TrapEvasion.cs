using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class TrapEvasion : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Trap Evasion", "Evasion!",
                                                        //SpellCircle.Fourth, 
                                                        21001,
                                                        9200
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public TrapEvasion(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("Your reflexes are heightened, making you more agile to avoid traps!");

                // Apply the trap evasion buff
                Caster.BeginAction(typeof(TrapEvasion));
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerStateCallback(EndEvasion), Caster);

                // Visual and sound effects
                Caster.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist); // Display evasion particles
                Caster.PlaySound(0x1F5); // Play sound effect for agility boost
            }

            FinishSequence();
        }

        private void EndEvasion(object state)
        {
            Mobile caster = (Mobile)state;

            if (caster != null && caster.Alive)
            {
                caster.SendMessage("Your heightened reflexes fade away.");
                caster.EndAction(typeof(TrapEvasion));

                // Ending visual effect
                caster.FixedParticles(0x3735, 1, 30, 9963, EffectLayer.Waist); // Fade-out particles
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
