using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;
using System.Collections;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class LockFreeze : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Lock Freeze", "Frigus Clavis",
                                                        21001,
                                                        9301
                                                       );

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 10; } }

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public LockFreeze(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private LockFreeze m_Owner;

            public InternalTarget(LockFreeze owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is LockableContainer container)
                {
                    m_Owner.Target(container);
                }
                else
                {
                    from.SendMessage("That is not a lockable container!");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(LockableContainer container)
        {
            if (!Caster.CanSee(container))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
			else if (SpellHelper.CheckTown(Caster, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, container);  // Ensure this takes Caster and container, if not adjust accordingly.
                Effects.PlaySound(container.Location, container.Map, 0x208); // Sound of ice freezing

                container.Locked = true;
                Effects.SendLocationParticles(EffectItem.Create(container.Location, container.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 1152, 0, 0, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => Unlock(container)); // Unlocks after 10 seconds
            }

            FinishSequence();
        }

        private void Unlock(LockableContainer container)
        {
            if (container != null && container.Locked)
            {
                container.Locked = false;
                Effects.PlaySound(container.Location, container.Map, 0x56B); // Sound of ice breaking
                Effects.SendLocationParticles(EffectItem.Create(container.Location, container.Map, EffectItem.DefaultDuration), 0x37B9, 9, 32, 1153, 0, 0, 0);
                
                // Send a message to all players in the vicinity
                foreach (Mobile mobile in container.Map.GetMobilesInRange(container.Location, 10))
                {
                    mobile.SendMessage("The lock thaws, becoming operable again.");
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
