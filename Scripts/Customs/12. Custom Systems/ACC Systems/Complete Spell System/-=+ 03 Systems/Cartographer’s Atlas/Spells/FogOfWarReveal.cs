using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class FogOfWarReveal : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Fog of War Reveal", "Maximus Revealo",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; } // Adjust the circle to fit the skill system balance
        }

        public override double CastDelay { get { return 0.2; } } // Adjust the delay as needed
        public override double RequiredSkill { get { return 80.0; } } // Adjust the required skill level as needed
        public override int RequiredMana { get { return 50; } } // Adjust the mana cost as needed

        // Define Cooldown here if CartographySpell does not have it
        public TimeSpan Cooldown { get { return TimeSpan.FromMinutes(30); } } // Set the cooldown time to 30 minutes

        public FogOfWarReveal(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            // Start the targeting process
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            Point3D loc = new Point3D(p);

            // Check if the caster can see the location
            SpellHelper.Turn(Caster, p);

            // Play sound and visual effects at the target location
            Effects.PlaySound(loc, Caster.Map, 0x64C); // Play a dramatic sound
            Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x375A, 1, 30, 1153, 7, 9502, 0); // Play visual effect

            // Reveal all hidden creatures within 10 tiles
            foreach (Mobile m in Caster.GetMobilesInRange(10))
            {
                if (m.Hidden && Caster.CanBeHarmful(m, false))
                {
                    m.RevealingAction();
                    m.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Head); // Reveal effect
                    m.PlaySound(0x3C4); // Sound effect for revealing
                }
            }

            FinishSequence(); // Finish the sequence
        }

        private class InternalTarget : Target
        {
            private FogOfWarReveal m_Owner;

            public InternalTarget(FogOfWarReveal owner) : base(10, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D p)
                    m_Owner.Target(p);
                else
                    from.SendMessage("You must target a location.");
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0); // Adjust the cast delay
        }
    }
}
