using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class ForensicHealing : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Forensic Healing", "Realus Healus",
            21004, // Sound ID
            9300   // Effect ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override int RequiredMana { get { return 30; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }

        public ForensicHealing(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ForensicHealing m_Spell;

            public InternalTarget(ForensicHealing spell) : base(12, false, TargetFlags.Beneficial)
            {
                m_Spell = spell;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (!from.CanSee(target))
                    {
                        from.SendLocalizedMessage(500237); // Target cannot be seen.
                    }
                    else if (m_Spell.CheckSequence())
                    {
                        SpellHelper.Turn(from, target);
                        
                        // Visual effects
                        target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                        target.PlaySound(0x213); // Sound effect

                        // Heal 50% of target's maximum health
                        int toHeal = (int)(target.HitsMax * 0.5);
                        target.Hits += toHeal;

                        // Another visual effect for a healing glow
                        Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5030);
                        Effects.PlaySound(target.Location, target.Map, 0x1F7); // Healing sound effect
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target cannot be seen.
                }

                m_Spell.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Spell.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
