using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class DisruptiveResonance : DiscordanceSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disruptive Resonance", "Dis Ort Bloo",
            // SpellCircle.Fifth,
            21004,
            9300
        );

        public override SpellCircle Circle { get { return SpellCircle.Fifth; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 25; } }

        public DisruptiveResonance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.RevealingAction();

                int numberOfStrikes = Utility.RandomMinMax(3, 6); // Random number of lightning strikes
                for (int i = 0; i < numberOfStrikes; i++)
                {
                    Point3D strikeLocation = GetRandomLocationWithinRange(Caster.Location, 8);
                    Map map = Caster.Map;

                    if (map != null)
                    {
                        // Visual effect and sound for the lightning strike
                        Effects.SendLocationParticles(EffectItem.Create(strikeLocation, map, EffectItem.DefaultDuration), 0x379F, 10, 30, 5052);
                        Effects.PlaySound(strikeLocation, map, 0x29);

                        // Find mobiles at the location of the strike
                        IPooledEnumerable mobilesAtLocation = map.GetMobilesInRange(strikeLocation, 0);

                        foreach (Mobile mobile in mobilesAtLocation)
                        {
                            if (mobile != Caster && Caster.CanBeHarmful(mobile, false))
                            {
                                Caster.DoHarmful(mobile);
                                int damage = Utility.RandomMinMax(15, 30); // Random damage for each strike
                                AOS.Damage(mobile, Caster, damage, 0, 0, 0, 0, 100); // Pure energy damage
                            }
                        }

                        mobilesAtLocation.Free();
                    }
                }
            }

            FinishSequence();
        }

        private Point3D GetRandomLocationWithinRange(Point3D center, int range)
        {
            int xOffset = Utility.RandomMinMax(-range, range);
            int yOffset = Utility.RandomMinMax(-range, range);
            int zOffset = 0; // Keep the Z level consistent; adjust if you want varying heights

            return new Point3D(center.X + xOffset, center.Y + yOffset, center.Z + zOffset);
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5); // Cooldown for casting the spell
        }
    }
}
