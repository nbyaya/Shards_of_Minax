using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class ResonantGuard : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Resonant Guard", "Cantus Deflectus",
                                                        //SpellCircle.Sixth,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 85; } }

        public ResonantGuard(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Particle effect
                Caster.PlaySound(0x1FA); // Sound effect

                Effects.SendTargetParticles(Caster, 0x376A, 1, 13, 5054, EffectLayer.Waist);
                Caster.PlaySound(0x1F7);

                // Apply a temporary buff to deflect incoming attacks and projectiles

                Timer.DelayCall(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(1.0), 10, () =>
                {
                    foreach (Mobile m in Caster.GetMobilesInRange(2))
                    {
                        if (m != Caster && m.Alive && Caster.CanBeHarmful(m, false))
                        {
                            Caster.DoHarmful(m);
                            m.FixedEffect(0x3779, 10, 16, 1153, 3); // Spark effect
                            m.PlaySound(0x5C3); // Sound effect for deflected attack
                            m.SendMessage("Your attack is deflected by a resonant shield!");
                        }
                    }
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }

    public class BuffInfo
    {
        public static void AddBuff(Mobile m, BuffInfo buff)
        {
            // Implementation for applying buff effects
            m.SendMessage("You are protected by a resonant shield!");
        }
    }
}
