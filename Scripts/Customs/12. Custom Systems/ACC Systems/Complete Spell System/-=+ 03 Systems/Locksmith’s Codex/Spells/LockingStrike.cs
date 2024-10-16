using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class LockingStrike : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Locking Strike", "Claudo Percussus",
                                                        //SpellCircle.Second,
                                                        21001,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 10; } }

        public LockingStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (Caster.CanBeHarmful(target) && CheckSequence())
            {
                // Perform the special attack
                Caster.DoHarmful(target);

                // Lock abilities/items for a short duration
                TimeSpan duration = TimeSpan.FromSeconds(10.0 + (Caster.Skills[SkillName.Lockpicking].Value / 5.0)); // Increase duration based on caster's Lockpicking skill

                target.Frozen = true; // Prevents the target from moving
                target.SendMessage("You feel your abilities and items are temporarily locked!");
                target.PlaySound(0x1F5); // Play a locking sound effect
                target.FixedParticles(0x374A, 10, 30, 5054, EffectLayer.Waist); // Visual effect around the waist

                // Create a timer to unlock abilities after the duration
                Timer.DelayCall(duration, () => UnlockAbilities(target));

                Caster.Mana -= RequiredMana; // Deduct mana cost
            }

            FinishSequence();
        }

        private void UnlockAbilities(Mobile target)
        {
            if (target != null && !target.Deleted)
            {
                target.Frozen = false; // Re-enable movement
                target.SendMessage("Your abilities and items are no longer locked.");
                target.PlaySound(0x1F6); // Play unlocking sound effect
            }
        }

        private class InternalTarget : Target
        {
            private LockingStrike m_Owner;

            public InternalTarget(LockingStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
