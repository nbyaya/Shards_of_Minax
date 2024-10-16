using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class WildernessStealth : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Wilderness Stealth", "En Snolam Invisibility",
            21005,
            9301,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 25; } }

        public WildernessStealth(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Make the caster hidden (invisible)
                Caster.Hidden = true;
                Caster.SendMessage("You blend into the wilderness, becoming invisible.");
                
                // Play stealth sound and visual effects
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F4); // Play stealth sound
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5021);

                // Set a timer for the stealth duration (30 seconds)
                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
                {
                    if (!Caster.Hidden)
                        return;

                    // Reveal the caster after 30 seconds
                    Caster.Hidden = false;
                    Caster.SendMessage("You become visible again as the effect of Wilderness Stealth fades.");
                    
                    // Play reappear sound and visual effects
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x1F4); // Play reappear sound
                    Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5021);
                });

                // Set a cooldown for the next cast (3 minutes)
                Caster.NextSkillTime = DateTime.UtcNow.Ticks + TimeSpan.FromMinutes(3).Ticks;
            }

            FinishSequence();
        }
    }
}
