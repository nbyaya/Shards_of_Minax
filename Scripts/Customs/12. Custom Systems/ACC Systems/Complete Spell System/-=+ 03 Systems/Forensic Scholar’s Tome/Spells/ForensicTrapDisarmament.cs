using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class ForensicTrapDisarmament : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Forensic Trap Disarmament", "Revlo!",
            21004, // GumpID
            9300   // ButtonID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public ForensicTrapDisarmament(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply effects to reveal hidden creatures
                RevealHiddenCreatures(Caster.Location, Caster.Map, 5);
                
                // Play visual and sound effects
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1FB); // Sound effect for spell
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5013); // Flashy visual effect

                // End the spell casting sequence
                FinishSequence();
            }
        }

        private void RevealHiddenCreatures(Point3D center, Map map, int radius)
        {
            List<Mobile> hiddenCreatures = new List<Mobile>();

            // Find all hidden creatures within the radius
            foreach (Mobile m in map.GetMobilesInRange(center, radius))
            {
                if (m.Hidden && m.Alive && m is BaseCreature)
                {
                    hiddenCreatures.Add(m);
                }
            }

            // Reveal each hidden creature
            foreach (Mobile creature in hiddenCreatures)
            {
                creature.RevealingAction();
                creature.SendMessage("You have been revealed by Forensic Trap Disarmament!");
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
