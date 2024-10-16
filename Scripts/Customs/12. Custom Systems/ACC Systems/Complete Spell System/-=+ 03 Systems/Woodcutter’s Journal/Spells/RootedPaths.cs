using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class RootedPaths : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Rooted Paths", "Vas Kal An Path",
                                                        //SpellCircle.Sixth,
                                                        21007,
                                                        9307
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public RootedPaths(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(p, Caster.Map, 0x1F5); // Play tree root growing sound

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;

                        Point3D loc = new Point3D(p.X + i, p.Y + j, p.Z);
                        InternalItem root = new InternalItem(loc, Caster.Map);
                        root.MoveToWorld(loc, Caster.Map);
                    }
                }

                Caster.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist); // Visual effect for spell casting

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => ClearRoots(p, Caster.Map)); // Roots last for 10 seconds
            }

            FinishSequence();
        }

        private void ClearRoots(IPoint3D p, Map map)
        {
            foreach (Item item in map.GetItemsInRange(new Point3D(p), 2))
            {
                if (item is InternalItem)
                    item.Delete();
            }
        }

        private class InternalItem : Item
        {
            public override bool BlocksFit { get { return true; } }

            public InternalItem(Point3D loc, Map map) : base(0x1B6D) // Tree root appearance
            {
                Movable = false;
                MoveToWorld(loc, map);
            }

            public InternalItem(Serial serial) : base(serial)
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

        private class InternalTarget : Target
        {
            private RootedPaths m_Owner;

            public InternalTarget(RootedPaths owner) : base(12, true, TargetFlags.Harmful)
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
