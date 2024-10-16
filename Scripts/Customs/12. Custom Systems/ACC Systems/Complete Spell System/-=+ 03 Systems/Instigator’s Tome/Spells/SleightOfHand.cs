using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Engines;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class SleightOfHand : ProvocationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Sleight of Hand", "Zyra Kryto",
            21005,
            9400,
            false
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public SleightOfHand(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SleightOfHand m_Owner;

            public InternalTarget(SleightOfHand owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    // Attempt to steal an item from the target
                    m_Owner.StealItem(from, target);
                }
                else if (targeted is IPoint3D point)
                {
                    // Place a trap or distraction at the location
                    m_Owner.PlaceTrapOrDistraction(from, point);
                }

                m_Owner.FinishSequence();
            }
        }

        public void StealItem(Mobile caster, Mobile target)
        {
            if (!caster.CanSee(target))
            {
                caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (!target.Alive || target.IsDeadBondedPet)
            {
                caster.SendLocalizedMessage(1061907); // You can't steal from the dead!
                return;
            }

            // Steal logic
            Item stolenItem = target.Backpack.FindItemByType(typeof(Item)); // Basic item type search
            if (stolenItem != null)
            {
                caster.Backpack.DropItem(stolenItem);
                target.SendMessage("An item has been stolen from you!");
                caster.SendMessage("You have successfully stolen an item!");

                // Visual and sound effects
                Effects.PlaySound(caster.Location, caster.Map, 0x1F4); // Sound effect
                caster.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Waist); // Visual effect
            }
            else
            {
                caster.SendMessage("No suitable items could be stolen.");
            }
        }

        public void PlaceTrapOrDistraction(Mobile caster, IPoint3D point)
        {
            Point3D loc = new Point3D(point);

            if (!caster.CanSee(point))
            {
                caster.SendLocalizedMessage(500237); // Target can not be seen.
                return;
            }

            if (!SpellHelper.CheckTown(loc, caster) || !caster.InRange(loc, 12))
            {
                caster.SendMessage("You cannot place a trap or distraction there.");
                return;
            }

            Item trapOrDistraction;

            if (Utility.RandomBool()) // 50% chance to place a trap or distraction
            {
                trapOrDistraction = new CaltropTrap();
                caster.SendMessage("You have placed a caltrop trap.");
            }
            else
            {
                trapOrDistraction = new NoiseMaker();
                caster.SendMessage("You have placed a noise-making distraction.");
            }

            trapOrDistraction.MoveToWorld(loc, caster.Map);

            // Visual and sound effects
            Effects.PlaySound(loc, caster.Map, 0x1F4); // Sound effect
            Effects.SendLocationParticles(EffectItem.Create(loc, caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5025); // Visual effect
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }

        // Inner classes for traps and distractions
        private class CaltropTrap : Item
        {
            public CaltropTrap() : base(0x10B2) // Trap item ID
            {
                Movable = false;
                Name = "Caltrop Trap";
                Hue = 1150;
                Timer.DelayCall(TimeSpan.FromSeconds(0), CheckForStep);
            }

            private void CheckForStep()
            {
                // Register a movement listener to detect when a mobile steps on the trap
                new TrapDetectionTimer(this).Start();
            }

            private class TrapDetectionTimer : Timer
            {
                private CaltropTrap m_Trap;

                public TrapDetectionTimer(CaltropTrap trap) : base(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1))
                {
                    m_Trap = trap;
                }

                protected override void OnTick()
                {
                    foreach (Mobile mobile in m_Trap.GetMobilesInRange(1))
                    {
                        if (mobile.Player)
                        {
                            mobile.Damage(10); // Damage upon stepping on the trap
                            mobile.SendMessage("You step on a caltrop trap and take damage!");
                            Effects.PlaySound(mobile.Location, m_Trap.Map, 0x1F4); // Sound effect
                            m_Trap.Delete(); // Remove the trap after it triggers
                            Stop();
                            return;
                        }
                    }
                }
            }
        }

        private class NoiseMaker : Item
        {
            public NoiseMaker() : base(0x1E2D) // Noise item ID
            {
                Movable = false;
                Name = "Noise Maker";
                Hue = 1150;
                Timer.DelayCall(TimeSpan.FromSeconds(10), Delete); // Deletes after 10 seconds
            }

            public override void OnDoubleClick(Mobile from)
            {
                Effects.PlaySound(Location, Map, 0x2A7); // Sound effect
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5023); // Visual effect
                from.SendMessage("The noise maker creates a loud distraction!");
            }
        }
    }
}
