using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class RuneOfTeleportation : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Rune of Teleportation", "Vas Rel Por",
            //SpellCircle.Fourth,
            21004,
            9300,
            false,
            Reagent.BlackPearl,
            Reagent.Bloodmoss
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 35; } }

        public RuneOfTeleportation(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private RuneOfTeleportation m_Owner;

            public InternalTarget(RuneOfTeleportation owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                {
                    IPoint3D p = (IPoint3D)targeted;
                    Point3D point = new Point3D(p.X, p.Y, p.Z); // Convert IPoint3D to Point3D

                    if (!from.CanSee(point))
                    {
                        from.SendLocalizedMessage(500237); // Target cannot be seen.
                    }
                    else if (SpellHelper.CheckTown(point, from) && m_Owner.CheckSequence())
                    {
                        SpellHelper.Turn(from, point);

                        Mobile targetMobile = targeted as Mobile;

                        if (targetMobile != null && targetMobile != from)
                        {
                            if (targetMobile.Map == null)
                                return;

                            Point3D fromLoc = targetMobile.Location;
                            Map map = targetMobile.Map;

                            // Visual and Sound Effects
                            Effects.SendLocationParticles(EffectItem.Create(fromLoc, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                            Effects.PlaySound(fromLoc, map, 0x1FC);

                            targetMobile.Location = point;
                            targetMobile.ProcessDelta();

                            // Visual and Sound Effects after teleport
                            Effects.SendLocationParticles(EffectItem.Create(point, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);
                            Effects.PlaySound(point, map, 0x1FC);

                            // Flashy additional effect
                            from.SendMessage("The Rune of Teleportation transports you with a burst of light!");
                        }
                        else if (targeted is Mobile) // Fixed condition to handle the mobile teleportation case
                        {
                            Point3D fromLoc = from.Location;
                            Map map = from.Map;

                            // Visual and Sound Effects
                            Effects.SendLocationParticles(EffectItem.Create(fromLoc, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                            Effects.PlaySound(fromLoc, map, 0x1FC);

                            from.Location = point;
                            from.ProcessDelta();

                            // Visual and Sound Effects after teleport
                            Effects.SendLocationParticles(EffectItem.Create(point, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);
                            Effects.PlaySound(point, map, 0x1FC);

                            // Flashy additional effect
                            from.SendMessage("The Rune of Teleportation transports you with a burst of light!");
                        }
                    }
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
