using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class ElementalSurge : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Elemental Surge", "Channelling the forces of fire, ice, or lightning",
            // The spell circle number can be adjusted if needed
            266,
            9040
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; } // Adjust circle if necessary
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 25; } }

        public ElementalSurge(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Determine the elemental type
                int elementType = Utility.Random(3);

                // Define effect parameters
                int itemID;
                int soundID;
                EffectType effect;

                switch (elementType)
                {
                    case 0: // Fire
                        itemID = 0x1F4; // Fire effect
                        soundID = 0x15E; // Fireball sound
                        effect = EffectType.Fire;
                        break;

                    case 1: // Ice
                        itemID = 0x1F7; // Ice effect
                        soundID = 0x2D4; // Ice spell sound
                        effect = EffectType.Ice;
                        break;

                    case 2: // Lightning
                        itemID = 0x2A8; // Lightning effect
                        soundID = 0x29B; // Lightning sound
                        effect = EffectType.Lightning;
                        break;

                    default:
                        itemID = 0x1F4;
                        soundID = 0x15E;
                        effect = EffectType.Fire;
                        break;
                }

                // Create the elemental surge effect
                CreateElementalSurgeEffect(effect, soundID, itemID);

                // Apply damage to all enemies in the area
                ApplyDamageToEnemies();

                FinishSequence();
            }
        }

        private void CreateElementalSurgeEffect(EffectType effect, int soundID, int itemID)
        {
            // Play the sound effect
            Effects.PlaySound(Caster.Location, Caster.Map, soundID);

            // Create visual effect based on the chosen element
            int duration = 5; // Duration in seconds
            Timer.DelayCall(TimeSpan.FromSeconds(0.1), () =>
            {
                // Corrected effect parameters (reduced to 4 parameters)
                Effects.SendLocationEffect(Caster.Location, Caster.Map, itemID, 30);
            });
        }

        private void ApplyDamageToEnemies()
        {
            List<Mobile> enemies = new List<Mobile>();

            foreach (Mobile m in Caster.GetMobilesInRange(5))
            {
                if (m.Alive && !m.Equals(Caster)) // Check if mobile is not the caster
                {
                    enemies.Add(m);
                }
            }

            foreach (Mobile enemy in enemies)
            {
                int damage = Utility.Random(10, 20); // Random damage between 10 and 20
                AOS.Damage(enemy, Caster, damage, 0, 0, 0, 0, 0);
            }
        }

        private enum EffectType
        {
            Fire,
            Ice,
            Lightning
        }
    }
}
