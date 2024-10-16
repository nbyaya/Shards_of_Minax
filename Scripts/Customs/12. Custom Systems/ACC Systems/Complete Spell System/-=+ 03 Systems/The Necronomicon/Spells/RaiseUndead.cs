using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items; // Added this directive
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class RaiseUndead : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Raise Undead", "Kal Vas Corp",
                                                        21004,
                                                        9300,
                                                        false,
                                                        Reagent.GraveDust,
                                                        Reagent.DaemonBlood
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public RaiseUndead(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
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

                if (map == null)
                    return;

                // List of corpses in the area
                List<Item> corpses = new List<Item>();
                IPooledEnumerable eable = map.GetItemsInRange(new Point3D(p), 3);

                foreach (Item item in eable)
                {
                    if (item is Corpse)
                        corpses.Add(item);
                }

                eable.Free();

                // Reanimate up to 3 corpses
                int raised = 0;
                foreach (Corpse corpse in corpses)
                {
                    if (raised >= 3)
                        break;

                    BaseCreature undead = null;

                    if (corpse.Owner is PlayerMobile)
                    {
                        // Reanimate as skeletal minion
                        undead = new Skeleton();
                    }
                    else if (corpse.Owner is BaseCreature)
                    {
                        // Reanimate as zombie minion
                        undead = new Zombie();
                    }

                    if (undead != null)
                    {
                        Effects.SendLocationEffect(corpse.Location, map, 0x3709, 30, 10, 0x835, 0);
                        Effects.PlaySound(corpse.Location, map, 0x1FB);

                        undead.MoveToWorld(corpse.Location, map);
                        undead.ControlMaster = Caster;
                        undead.Controlled = true;
                        undead.SummonMaster = Caster;
                        undead.Summoned = true;
                        undead.Delete();

                        raised++;
                    }
                }

                Caster.Mana -= RequiredMana;
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private RaiseUndead m_Owner;

            public InternalTarget(RaiseUndead owner) : base(12, true, TargetFlags.None)
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
    }
}
