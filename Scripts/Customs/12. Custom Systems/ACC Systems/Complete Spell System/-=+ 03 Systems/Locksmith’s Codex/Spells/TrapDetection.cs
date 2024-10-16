using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items; // Ensure this is included for item-related operations
using Server.Engines.Harvest; // Include this if BaseTrap is part of this namespace

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class TrapDetection : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Trap Detection", "Revela Trappo",
            21001,
            9301,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 10; } }

        public TrapDetection(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
                Caster.SendMessage("Target an area to detect traps.");
            }
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                Map map = Caster.Map;

                if (map != null)
                {
                    IPooledEnumerable eable = map.GetItemsInRange(new Point3D(p), 5);
                    List<Item> traps = new List<Item>();

                    foreach (Item item in eable)
                    {
                        if (item is BaseTrap) // Removed Hazard check
                        {
                            traps.Add(item);
                        }
                    }

                    eable.Free();

                    if (traps.Count > 0)
                    {
                        foreach (Item trap in traps)
                        {
                            // Corrected EffectItem usage
                            Effects.SendLocationParticles(
                                trap, 
                                0x376A, 9, 32, 5008, 0, 0, 0);

                            trap.PublicOverheadMessage(MessageType.Regular, 0x22, false, "*Revealed*");
                        }

                        Effects.PlaySound(Caster.Location, Caster.Map, 0x1F5); // Play a magical sound
                    }
                    else
                    {
                        Caster.SendMessage("No traps detected in the area.");
                    }
                }

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private TrapDetection m_Owner;

            public InternalTarget(TrapDetection owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D p)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
