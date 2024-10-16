using System;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class DiscordantEcho : DiscordanceSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Discordant Echo", "Echo Discortis",
            21004, // GumpID for the spell icon
            9300   // Sound ID for casting sound
        );

        public override SpellCircle Circle => SpellCircle.Third; // Adjusted spell circle as needed
        public override double CastDelay => 0.1;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 25;

        public DiscordantEcho(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You emit a discordant echo, causing havoc to nearby creatures!");

                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F7); // Play a sound effect
                Caster.FixedParticles(0x376A, 1, 62, 0x26BD, EffectLayer.Waist); // Play a visual effect

                List<Mobile> targets = new List<Mobile>();

                // Find all mobiles within a 3-tile radius
                Map map = Caster.Map;
                if (map != null)
                {
                    foreach (Mobile m in Caster.GetMobilesInRange(3))
                    {
                        if (m != Caster && Caster.CanBeHarmful(m, false))
                        {
                            targets.Add(m);
                        }
                    }
                }

                // Apply damage over time effect
                foreach (Mobile target in targets)
                {
                    Caster.DoHarmful(target);
                    Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => ApplyDiscordantDamage(target, 5, 4)); // Apply damage every second for 4 seconds
                }
            }

            FinishSequence();
        }

        private void ApplyDiscordantDamage(Mobile target, int damage, int duration)
        {
            if (target.Alive && !target.Deleted)
            {
                for (int i = 0; i < duration; i++)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(i), () =>
                    {
                        if (target.Alive && !target.Deleted)
                        {
                            target.Damage(damage);
                            target.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Head); // Damage visual effect
                            target.PlaySound(0x1F2); // Damage sound effect
                        }
                    });
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }
    }
}
