using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class TrapDetection : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Trap Detection", "In Wis Jux",
            21001,
            9200
        );

        public override SpellCircle Circle => SpellCircle.Fourth;

        public override double CastDelay => 1.5;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 10;

        public TrapDetection(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private TrapDetection m_Owner;

            public InternalTarget(TrapDetection owner) : base(10, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point)
                {
                    m_Owner.Target(point);
                }
                else if (targeted is Item item)
                {
                    m_Owner.RevealTrapOnItem(item);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (CheckSequence())
            {
                SpellHelper.Turn(Caster, new Point3D(p)); // Convert IPoint3D to Point3D
                SpellHelper.GetSurfaceTop(ref p);

                Map map = Caster.Map;

                if (map != null)
                {
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x29); // Play a magical sound
                    Effects.SendLocationParticles(EffectItem.Create(new Point3D(p), map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008); // Flashy particles

                    List<TrapableContainer> traps = new List<TrapableContainer>();

                    foreach (Item item in map.GetItemsInRange(new Point3D(p), 5)) // Scan for traps in a 5-tile radius
                    {
                        TrapableContainer trap = item as TrapableContainer;
                        if (trap != null && !trap.TrapType.Equals(TrapType.None)) // Use Equals for comparison
                        {
                            traps.Add(trap);
                            trap.PublicOverheadMessage(MessageType.Regular, 0x22, false, "Trap Detected!"); // Overhead message on the item

                            // Assuming the correct signature is (Entity target, int itemID, int duration, int speed)

                        }
                    }

                    if (traps.Count == 0)
                    {
                        Caster.SendMessage("No traps detected in the area.");
                    }
                    else
                    {
                        Caster.SendMessage($"{traps.Count} traps detected in the area!");
                    }
                }
            }

            FinishSequence();
        }

        public void RevealTrapOnItem(Item item)
        {
            TrapableContainer trap = item as TrapableContainer;
            if (trap != null && !trap.TrapType.Equals(TrapType.None)) // Use Equals for comparison
            {
                Effects.PlaySound(item.Location, item.Map, 0x29); // Play a sound at the item's location

                // Assuming the correct signature is (Entity target, int itemID, int duration, int speed)

                trap.PublicOverheadMessage(MessageType.Regular, 0x22, false, "Trap Detected!"); // Overhead message
                Caster.SendMessage("Trap detected on the selected item!");
            }
            else
            {
                Caster.SendMessage("The selected item is not trapped.");
            }

            FinishSequence();
        }
    }
}
