using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items; // Make sure this namespace contains BaseTree and BaseRock

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class Pathfinder : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Pathfinder", "Remo Ostacoli",
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
        public override int RequiredMana { get { return 10; } }

        public Pathfinder(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                SpellHelper.GetSurfaceTop(ref p);

                // Play a sound effect
                Effects.PlaySound(p, Caster.Map, 0x1FE); // Sound of clearing debris

                // Add a visual effect for clearing the path
                Effects.SendLocationEffect(new Point3D(p), Caster.Map, 0x3709, 30, 10, 0x25, 0); // Earth explosion effect
                Effects.SendLocationEffect(new Point3D(p.X + 1, p.Y, p.Z), Caster.Map, 0x3709, 30, 10, 0x25, 0); // Earth explosion effect
                Effects.SendLocationEffect(new Point3D(p.X - 1, p.Y, p.Z), Caster.Map, 0x3709, 30, 10, 0x25, 0); // Earth explosion effect
                Effects.SendLocationEffect(new Point3D(p.X, p.Y + 1, p.Z), Caster.Map, 0x3709, 30, 10, 0x25, 0); // Earth explosion effect
                Effects.SendLocationEffect(new Point3D(p.X, p.Y - 1, p.Z), Caster.Map, 0x3709, 30, 10, 0x25, 0); // Earth explosion effect

                // Simulate clearing obstacles by removing certain types of items within a small radius
                foreach (Item item in Caster.Map.GetItemsInRange(new Point3D(p), 2))
                {
                    if (item is Static || item is BaseTree || item is BaseRock) // Example types that represent obstacles
                    {
                        item.Delete();
                    }
                }

                Caster.SendMessage("You have cleared the path ahead!");
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private Pathfinder m_Owner;

            public InternalTarget(Pathfinder owner) : base(12, true, TargetFlags.None)
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

// Example: Add these classes in Server/Items or a relevant directory
namespace Server.Items
{
    public class BaseTree : Item
    {
        public BaseTree() : base(0x0)
        {
        }

        public BaseTree(Serial serial) : base(serial)
        {
        }

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
    }

    public class BaseRock : Item
    {
        public BaseRock() : base(0x0)
        {
        }

        public BaseRock(Serial serial) : base(serial)
        {
        }

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
    }
}
