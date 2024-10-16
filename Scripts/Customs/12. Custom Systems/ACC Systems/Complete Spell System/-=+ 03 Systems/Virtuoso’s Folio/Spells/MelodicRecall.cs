using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class MelodicRecall : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Melodic Recall", "Tunis Rexal",
            // No specific incantation needed for this spell
            21004, // Animation ID for casting
            9300   // Sound ID for casting
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; } // Adjusted as per system need
        }

        public override double CastDelay { get { return 0.2; } } // Short delay for casting
        public override double RequiredSkill { get { return 60.0; } } // Moderate skill requirement
        public override int RequiredMana { get { return 70; } } // High mana cost as described

        private Point3D m_MarkedLocation; // Store the marked location for recall

        public MelodicRecall(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
            // Initialize with a marked location. This can be set previously through another ability or command.
            m_MarkedLocation = caster.Location; // Set to the caster's current location initially
        }

        public override void OnCast()
        {
            if (m_MarkedLocation == Point3D.Zero) // If no location has been marked
            {
                Caster.SendMessage("You have not marked a location to recall to.");
                FinishSequence();
                return;
            }

            if (CheckSequence()) // Check if the spell can be cast
            {
                // Consume mana and scroll if present
                if (Scroll != null)
                    Scroll.Consume();

                Caster.Mana -= RequiredMana;

                // Play teleport effect at current location
                Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x3728, 10, 1, 1153, 3);
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1FE);

                // Move the caster to the marked location
                Caster.MoveToWorld(m_MarkedLocation, Caster.Map);

                // Play teleport arrival effect
                Effects.SendLocationEffect(m_MarkedLocation, Caster.Map, 0x3728, 10, 1, 1153, 3);
                Effects.PlaySound(m_MarkedLocation, Caster.Map, 0x1FE);

                // Add a special effect at the arrival point for flair
                Caster.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                Caster.PlaySound(0x5C3); // Magical sound on arrival
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0); // 2 seconds delay before casting
        }

        public void MarkLocation(Point3D location, Map map)
        {
            m_MarkedLocation = location;
        }
    }
}
