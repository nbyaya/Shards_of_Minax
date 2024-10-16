using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class AuraOfCourage : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Aura of Courage", "Valore Virtutis",
            21008,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public AuraOfCourage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Define the radius of the aura effect
                int auraRange = 8; // 8 tile radius

                // Get allies within range
                List<Mobile> allies = new List<Mobile>();
                foreach (Mobile m in Caster.GetMobilesInRange(auraRange))
                {
                    if (m != Caster && m.Alive && m.Player && Caster.CanBeBeneficial(m))
                    {
                        allies.Add(m);
                    }
                }

                // Apply effects to each ally
                foreach (Mobile ally in allies)
                {
                    if (ally != null)
                    {
                        // Apply visual effect and sound
                        ally.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Flashy particles around the waist
                        ally.PlaySound(0x1F7); // Courageous sound effect

                        // Boost attack speed
                        BuffInfo.AddBuff(ally, new BuffInfo(BuffIcon.Bless, 1075622, 1075623, TimeSpan.FromSeconds(30), ally)); // 30 seconds of increased attack speed

                        // Reduce fear effects (placeholder effect, may vary based on your ServUO setup)
                        BuffInfo.RemoveBuff(ally, BuffIcon.Clumsy); // Remove fear-like debuffs if applicable

                        ally.SendMessage("You feel a surge of courage and your attacks quicken!");
                    }
                }

                // Apply effects to the caster
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Flashy particles around the waist
                Caster.PlaySound(0x1F7); // Courageous sound effect

                // Indicate successful cast to caster
                Caster.SendMessage("You cast Aura of Courage, boosting your allies' morale!");

                FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
