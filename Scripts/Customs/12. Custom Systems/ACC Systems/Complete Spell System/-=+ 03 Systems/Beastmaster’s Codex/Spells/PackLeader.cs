using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class PackLeader : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Pack Leader", "Uus Ylem",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public PackLeader(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Increase the max followers temporarily
                Caster.FollowersMax += 2;
                Caster.SendMessage("You feel the power of the pack within you!");

                // Play sound and show effects
                Effects.PlaySound(Caster.Location, Caster.Map, 0x2CE); // A wolf howl sound
                Caster.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist); // Glowing particle effect around the caster

                // Timer to revert the max followers back to normal after a duration
                Timer.DelayCall(TimeSpan.FromSeconds(30), () => 
                {
                    Caster.FollowersMax -= 2;
                    Caster.SendMessage("The power of the pack fades away.");
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
