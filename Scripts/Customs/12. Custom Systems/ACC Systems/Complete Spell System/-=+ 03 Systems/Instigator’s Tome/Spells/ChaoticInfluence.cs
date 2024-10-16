using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class ChaoticInfluence : ProvocationSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Chaotic Influence", "In Or An Lor",
            21005,
            9400
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 20;
        public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(2.0);

        private static readonly TimeSpan Duration = TimeSpan.FromSeconds(30.0); // Duration of the aura

        public ChaoticInfluence(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You invoke the chaotic energies around you.");

                // Visual and sound effects for the caster
                Caster.FixedParticles(0x375A, 10, 30, 5052, EffectLayer.Waist);
                Caster.PlaySound(0x1FC);

                // Create the aura effect
                new ChaoticInfluenceAura(Caster, Duration).Start();

                FinishSequence();
            }
        }

        private class ChaoticInfluenceAura : Timer
        {
            private readonly Mobile m_Caster;
            private readonly DateTime m_End;
            private static readonly Dictionary<Mobile, ChaoticInfluenceAura> m_Auras = new Dictionary<Mobile, ChaoticInfluenceAura>();

            public ChaoticInfluenceAura(Mobile caster, TimeSpan duration) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
                m_Caster = caster;
                m_End = DateTime.UtcNow + duration;
                m_Auras[m_Caster] = this;

                // Set the debuff
                foreach (Mobile m in m_Caster.GetMobilesInRange(5))
                {
                    if (m != m_Caster && m is BaseCreature && m.Alive)
                    {
                        m.SendMessage("You feel a chaotic influence disrupting your spellcasting.");
                        // Here you should apply your debuff logic (e.g., using a custom property or flag)
                        (m as BaseCreature)?.ApplyChaoticInfluenceDebuff();
                    }
                }
            }

            protected override void OnTick()
            {
                if (m_Caster == null || m_Caster.Deleted || !m_Caster.Alive || DateTime.UtcNow >= m_End)
                {
                    Stop();
                    m_Auras.Remove(m_Caster);
                    return;
                }

                // Periodic visual and sound effects
                m_Caster.FixedParticles(0x375A, 10, 15, 5052, EffectLayer.Waist);
                m_Caster.PlaySound(0x1FD);

                // Chance for spell misfire
                foreach (Mobile m in m_Caster.GetMobilesInRange(5))
                {
                    if (m != m_Caster && m is BaseCreature && m.Alive)
                    {
                        if (Utility.RandomDouble() < 0.2) // 20% chance of spell misfire
                        {
                            m.SendMessage("The chaotic aura causes your spell to misfire!");
                            m.FixedParticles(0x374A, 10, 15, 5052, EffectLayer.Head);
                            m.PlaySound(0x1F8);
                            // Additional logic for misfire effect (e.g., spell fails or unintended effect)
                        }
                    }
                }
            }
        }
    }
}

public static class BaseCreatureExtensions
{
    public static void ApplyChaoticInfluenceDebuff(this BaseCreature creature)
    {
        // Implement the logic for the debuff effect
        // Example: Decrease the creature's spell effectiveness by some percentage
    }
}
