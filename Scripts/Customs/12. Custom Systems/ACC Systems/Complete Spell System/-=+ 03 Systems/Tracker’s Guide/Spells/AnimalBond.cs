using System;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class AnimalBond : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Animal Bond", "In Aera Bestia",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public AnimalBond(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private AnimalBond m_Owner;

            public InternalTarget(AnimalBond owner) : base(10, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                {
                    IPoint3D p = (IPoint3D)targeted;
                    Point3D point = new Point3D(p); // Convert IPoint3D to Point3D
                    
                    if (!m_Owner.Caster.CanSee(point))
                    {
                        m_Owner.Caster.SendLocalizedMessage(500237); // Target can not be seen.
                    }
                    else if (SpellHelper.CheckTown(point, m_Owner.Caster) && m_Owner.CheckSequence())
                    {
                        SpellHelper.Turn(m_Owner.Caster, point);

                        // Visual effect
                        Effects.SendLocationParticles(EffectItem.Create(point, from.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5023);
                        Effects.PlaySound(point, from.Map, 0x1E5);

                        m_Owner.RevealCreaturesAndPlayers(from, point);
                    }
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private void RevealCreaturesAndPlayers(Mobile caster, Point3D location)
        {
            Map map = caster.Map;
            if (map == null)
                return;

            List<Mobile> nearbyEntities = new List<Mobile>();

            // Range of detection
            int range = 10;

            // Gather nearby mobiles (players and creatures)
            foreach (Mobile m in map.GetMobilesInRange(location, range))
            {
                if (m != caster && (m.Player || m is BaseCreature))
                {
                    nearbyEntities.Add(m);
                }
            }

            if (nearbyEntities.Count > 0)
            {
                // Display information about nearby creatures and players
                caster.SendMessage("You sense the presence of the following nearby entities:");

                foreach (Mobile m in nearbyEntities)
                {
                    string type = m.Player ? "Player" : "Creature";
                    caster.SendMessage($"{type} detected: {m.Name} at ({m.X}, {m.Y}, {m.Z})");
                    
                    // Visual and sound effects for detected entities
                    m.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                    m.PlaySound(0x1EB);
                }
            }
            else
            {
                caster.SendMessage("No nearby creatures or players detected.");
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
