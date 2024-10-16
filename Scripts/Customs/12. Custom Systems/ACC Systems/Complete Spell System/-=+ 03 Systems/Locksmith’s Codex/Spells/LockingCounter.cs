using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;
using Server.Spells.Fifth; // For example, if Paralyze is a fifth circle spell

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class LockingCounter : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Locking Counter", "Liberum Contrarium",
            21001, 9301, false,
            Reagent.BlackPearl, Reagent.Bloodmoss
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 10; } }

        public LockingCounter(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, target);

                // Check for immobilization effects
                bool isTrapped = target.Paralyzed; // Example check for Paralyze status
                
                if (isTrapped)
                {
                    target.Paralyzed = false; // Remove paralysis effect

                    // Counter visual and sound effects
                    Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x374A, 10, 30, 5052);
                    Effects.PlaySound(target.Location, target.Map, 0x208);

                    // Retaliate with a pushback effect
                    int pushBackDistance = 2;
                    Point3D from = target.Location;
                    Point3D to = new Point3D(from.X + pushBackDistance * (target.X - Caster.X), from.Y + pushBackDistance * (target.Y - Caster.Y), from.Z);

                    if (target.Map != null && target.Map.CanSpawnMobile(to))
                    {
                        target.MoveToWorld(to, target.Map);
                    }

                    Caster.SendMessage("You counter the locking attempt and push your enemy back!");
                }
                else
                {
                    Caster.SendMessage("There are no locking effects to counter.");
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private LockingCounter m_Owner;

            public InternalTarget(LockingCounter owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
                else
                    from.SendLocalizedMessage(1060508); // That is not a valid target.
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
