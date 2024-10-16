using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class DisarmTrap : TacticsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disarm Trap", "Neutralizo",
            // SpellCircle.Third,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public DisarmTrap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private DisarmTrap m_Owner;

            public InternalTarget(DisarmTrap owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
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
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Map map = Caster.Map;
                Point3D loc = new Point3D(p);

                if (map != null && Caster.InLOS(loc))
                {
                    // Apply visual and sound effects
                    Effects.PlaySound(loc, map, 0x208);
                    Effects.SendLocationEffect(loc, map, 0x376A, 16, 10, 0, 0);

                    // Disarm traps in the targeted area
                    ArrayList list = new ArrayList();
                    foreach (Item item in map.GetItemsInRange(loc, 3))
                    {
                        if (item is BaseTrap)
                        {
                            list.Add(item);
                        }
                    }

                    foreach (Item trap in list)
                    {
                        if (trap is BaseTrap)
                        {
                            trap.Delete(); // Remove the trap
                        }
                    }

                    Caster.SendMessage("You have successfully disarmed the traps in the area.");
                }
                else
                {
                    Caster.SendMessage("You cannot disarm traps at that location.");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
