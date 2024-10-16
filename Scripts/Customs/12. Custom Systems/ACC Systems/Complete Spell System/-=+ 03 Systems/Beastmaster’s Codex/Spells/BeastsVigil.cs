using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class BeastsVigil : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Beast's Vigil", "Vigilo Bestia",
            21004,
            9300
        );
		
        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public BeastsVigil(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F5); // Play a casting sound
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Visual effect on the caster
                
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), ApplyAura); // Apply the aura after 1 second
            }

            FinishSequence();
        }

        private void ApplyAura()
        {
            ArrayList pets = new ArrayList();

            foreach (Mobile m in Caster.GetMobilesInRange(5))
            {
                if (m is BaseCreature && ((BaseCreature)m).Controlled && ((BaseCreature)m).ControlMaster == Caster)
                {
                    pets.Add(m);
                }
            }

            foreach (Mobile pet in pets)
            {
                pet.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Visual effect on the pet
                pet.SendMessage("You feel a protective aura surround you.");

                AuraEffect effect = new AuraEffect(pet, Caster, TimeSpan.FromSeconds(30.0), 3);
                effect.Start();
            }
        }

        private class AuraEffect : Timer
        {
            private Mobile m_Pet;
            private Mobile m_Caster;
            private DateTime m_End;
            private int m_HealAmount;

            public AuraEffect(Mobile pet, Mobile caster, TimeSpan duration, int healAmount) : base(TimeSpan.Zero, TimeSpan.FromSeconds(5.0))
            {
                m_Pet = pet;
                m_Caster = caster;
                m_HealAmount = healAmount;
                m_End = DateTime.Now + duration;
            }

            protected override void OnTick()
            {
                if (m_Pet.Deleted || !m_Pet.Alive || m_Pet.Map != m_Caster.Map || DateTime.Now >= m_End)
                {
                    Stop();
                    return;
                }

                m_Pet.Hits += m_HealAmount; // Heal the pet
                m_Pet.FixedEffect(0x376A, 1, 32); // Visual healing effect
                m_Pet.PlaySound(0x202); // Sound effect for healing
            }
        }
    }
}
