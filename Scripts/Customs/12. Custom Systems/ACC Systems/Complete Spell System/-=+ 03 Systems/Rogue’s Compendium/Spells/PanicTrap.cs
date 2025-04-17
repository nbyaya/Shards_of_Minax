using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class PanicTrap : StealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Panic Trap", "Gix Trappu",
            21004,
            9300
        );

        public override SpellCircle Circle => SpellCircle.Second;
        public override int RequiredMana => 10;
        public override double CastDelay => 0.1;
        public override double RequiredSkill => 20.0;

        public PanicTrap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PanicTrap m_Spell;

            public InternalTarget(PanicTrap spell) : base(12, true, TargetFlags.None)
            {
                m_Spell = spell;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D p)
                {
                    if (!from.CanSee(p))
                    {
                        from.SendLocalizedMessage(500237); // Target cannot be seen.
                    }
                    else if (m_Spell.CheckSequence())
                    {
                        SpellHelper.Turn(from, p);
                        SpellHelper.GetSurfaceTop(ref p);

                        Point3D loc = new Point3D(p);
                        Map map = from.Map;

                        if (map == null)
                            return;

                        InternalTrap trap = new InternalTrap(from, loc, map);
                        trap.MoveToWorld(loc, map);

                        Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                        Effects.PlaySound(loc, map, 0x1FB);
                    }
                }

                m_Spell.FinishSequence();
            }
        }

        private class InternalTrap : Item
        {
            private Mobile m_Caster;
            private DateTime m_Expiry;

            public InternalTrap(Mobile caster, Point3D loc, Map map) : base(0x1B72)
            {
                Movable = false;
                Visible = true;
                Name = "a panic trap";
                m_Caster = caster;
                m_Expiry = DateTime.UtcNow + TimeSpan.FromMinutes(1.0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5), CheckExpiry);
            }

            // ✅ Add required deserialization constructor
            public InternalTrap(Serial serial) : base(serial)
            {
            }

            private void CheckExpiry()
            {
                if (Deleted || DateTime.UtcNow > m_Expiry)
                {
                    Delete();
                    return;
                }

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in GetMobilesInRange(0))
                {
                    if (m != m_Caster && m is BaseCreature && m.Alive && !m.IsDeadBondedPet && m.CanBeHarmful(m_Caster, false))
                        targets.Add(m);
                }

                if (targets.Count > 0)
                {
                    foreach (Mobile target in targets)
                    {
                        if (m_Caster.CanBeHarmful(target, false))
                        {
                            m_Caster.DoHarmful(target);
                            target.SendMessage("You have triggered a panic trap!");

                            // Simulated fear effect
                            target.Frozen = true;
                            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                            {
                                if (target.Alive)
                                {
                                    target.Frozen = false;
                                    target.Combatant = null;
                                    target.Direction = (Direction)Utility.Random(8);
                                    target.Move(target.Direction);
                                }
                            });

                            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5023);
                            Effects.PlaySound(Location, Map, 0x209);

                            Delete();
                            break;
                        }
                    }
                }
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write(0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }
        }
    }
}
