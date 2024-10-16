using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class StalkersStrike : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Stalkers Strike", null,
            21004, // Icon ID
            9300,  // Action ID
            true,  // Requires target
            Reagent.Bloodmoss // Example reagent, can change if needed
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 25; } }

        // Static dictionary to track last tracking update times
        private static Dictionary<PlayerMobile, DateTime> _lastTrackingUpdates = new Dictionary<PlayerMobile, DateTime>();

        public StalkersStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster is PlayerMobile player)
            {
                player.Target = new InternalTarget(this);
            }
        }

        private class InternalTarget : Target
        {
            private StalkersStrike m_Owner;

            public InternalTarget(StalkersStrike owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && m_Owner.Caster.CanBeHarmful(target))
                {
                    m_Owner.Caster.DoHarmful(target);
                    m_Owner.OnTarget(from, target);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void OnTarget(Mobile from, Mobile target)
        {
            if (from is PlayerMobile player && target is Mobile)
            {
                // Check if player has been tracking the target
                bool isTracking = CheckTrackingStatus(player, target);

                if (CheckSequence())
                {
                    from.DoHarmful(target);

                    // Calculate base damage
                    int baseDamage = Utility.RandomMinMax(10, 20);

                    // Additional damage if tracking the target
                    int bonusDamage = isTracking ? Utility.RandomMinMax(15, 30) : 0;

                    // Total damage
                    int totalDamage = baseDamage + bonusDamage;

                    // Apply damage
                    AOS.Damage(target, from, totalDamage, 100, 0, 0, 0, 0);

                    // Visual and sound effects
                    Effects.SendMovingEffect(from, target, 0x36D4, 7, 0, false, false, 0x481, 0);
                    target.PlaySound(0x1F2);
                    target.FixedParticles(0x3728, 10, 15, 9950, EffectLayer.Waist);

                    // Inform player
                    from.SendMessage("You perform a Stalkers Strike, dealing {0} damage!", totalDamage);
                }
            }

            FinishSequence();
        }

        private bool CheckTrackingStatus(PlayerMobile player, Mobile target)
        {
            DateTime lastUpdate;
            bool isTracking = false;

            if (_lastTrackingUpdates.TryGetValue(player, out lastUpdate))
            {
                TimeSpan trackingDuration = DateTime.UtcNow - lastUpdate;
                isTracking = trackingDuration.TotalSeconds <= 10; // Adjust the condition as needed
            }

            // Update the last tracking time
            _lastTrackingUpdates[player] = DateTime.UtcNow;

            return isTracking;
        }
    }
}
