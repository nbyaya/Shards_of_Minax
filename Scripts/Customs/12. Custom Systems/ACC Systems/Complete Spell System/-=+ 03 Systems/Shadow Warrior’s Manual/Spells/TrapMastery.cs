using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class TrapMastery : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Trap Mastery", "Kazeka",
            21004,
            9300,
            false,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle => SpellCircle.Third;
        public override int RequiredMana => 20;
        public override double RequiredSkill => 50.0;
        public override double CastDelay => 0.2;

        public TrapMastery(Mobile caster, Item scroll) : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);
                Map map = Caster.Map;

                if (map == null)
                    return;

                // Create a hidden trap at the target location
                BaseTrap trap = new ExplosiveTrap();
                trap.MoveToWorld(loc, map);

                // Hide the trap visually and make it detectable
                trap.Visible = false;
                trap.Hue = 1157;
                trap.Name = "Hidden Trap";

                // Play a sound and effect when the trap is set
                Effects.PlaySound(loc, map, 0x208);
                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);

                // Message to the caster
                Caster.SendMessage("You have successfully set a hidden trap!");

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private TrapMastery m_Owner;

            public InternalTarget(TrapMastery owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D p)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        // Base class for traps
        public abstract class BaseTrap : Item
        {
            public BaseTrap() : base(0x1B72)
            {
                Movable = false;
                Visible = false;
                Hue = 1157;
            }

            public BaseTrap(Serial serial) : base(serial) { }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }

            public override bool OnMoveOver(Mobile m)
            {
                if (m.AccessLevel == AccessLevel.Player && m.Alive && m.CanBeDamaged())
                {
                    TriggerTrap(m);
                    return true;
                }

                return base.OnMoveOver(m);
            }

            public abstract void TriggerTrap(Mobile m);
        }

        // Specific type of trap that explodes
        public class ExplosiveTrap : BaseTrap
        {
            public ExplosiveTrap() : base()
            {
                Name = "Explosive Trap";
            }

            // Serialization constructor
            public ExplosiveTrap(Serial serial) : base(serial) { }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }

            public override void TriggerTrap(Mobile m)
            {
                // Play explosion effect
                Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10);
                Effects.PlaySound(Location, Map, 0x307);

                // Damage the player who triggered the trap
                AOS.Damage(m, m, Utility.RandomMinMax(30, 50), 100, 0, 0, 0, 0);

                Delete(); // Remove the trap after triggering
            }
        }
    }
}
