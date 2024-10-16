using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class CursedConfection : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Cursed Confection", "Invoke Confectio!",
                                                        //SpellCircle.Fourth,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public CursedConfection(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private CursedConfection m_Owner;

            public InternalTarget(CursedConfection owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D p)
                {
                    m_Owner.Target(p);
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
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);

                // Create the cursed cake
                CursedCake cake = new CursedCake
                {
                    ItemID = 0x9EC, // Visual ID for the cake
                    Name = "Cursed Confection",
                    Hue = 1266 // Dark purple hue to indicate cursed nature
                };
                
                cake.MoveToWorld(loc, Caster.Map);

                // Play sound and effect
                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x375A, 10, 30, 1266, 0, 5029, 0);
                Effects.PlaySound(loc, Caster.Map, 0x1E9);

                FinishSequence();
            }
        }

        private class CursedCake : Item
        {
            [Constructable]
            public CursedCake() : base(0x09E9) // Base item ID for cake
            {
                Movable = true;
                Stackable = false;
                Hue = 1266; // Dark purple to indicate curse
            }

            public CursedCake(Serial serial) : base(serial)
            {
            }

            public override void OnDoubleClick(Mobile from)
            {
                if (!from.InRange(this.GetWorldLocation(), 2))
                {
                    from.SendLocalizedMessage(500446); // That is too far away.
                    return;
                }

                if (from is PlayerMobile)
                {
                    // Apply deadly poison
                    from.ApplyPoison(from, Poison.Deadly);

                    // Random negative effect
                    int effect = Utility.Random(3);
                    switch (effect)
                    {
                        case 0:
                            from.SendMessage("You feel incredibly weak!");
                            from.AddStatMod(new StatMod(StatType.Str, "CursedConfectionStr", -10, TimeSpan.FromMinutes(2)));
                            break;
                        case 1:
                            from.SendMessage("You feel your mind slipping!");
                            from.AddStatMod(new StatMod(StatType.Int, "CursedConfectionInt", -10, TimeSpan.FromMinutes(2)));
                            break;
                        case 2:
                            from.SendMessage("You feel clumsy!");
                            from.AddStatMod(new StatMod(StatType.Dex, "CursedConfectionDex", -10, TimeSpan.FromMinutes(2)));
                            break;
                    }

                    // Play visual and sound effects on consumption
                    Effects.SendTargetParticles(from, 0x374A, 10, 15, 5013, EffectLayer.Waist);
                    Effects.PlaySound(from.Location, from.Map, 0x204);

                    this.Delete();
                }
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
}
