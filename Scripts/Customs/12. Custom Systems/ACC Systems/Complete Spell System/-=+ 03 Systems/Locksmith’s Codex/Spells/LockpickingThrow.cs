using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class LockpickingThrow : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Lockpicking Throw", "Tools of Disarray",
                                                        21001,
                                                        9301,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 10; } }

        public LockpickingThrow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
            else if (CheckHSequence(target))
            {
                SpellHelper.Turn(Caster, target);

                // Visual and Sound Effects
                Effects.SendMovingEffect(Caster, target, 0x1F2E, 10, 0, false, false, 0x481, 0);
                Caster.PlaySound(0x3B5);

                // Immobilize or Disorient Effect
                if (Utility.RandomBool()) // 50% chance to immobilize or disorient
                {
                    target.Freeze(TimeSpan.FromSeconds(3.0)); // Immobilizes for 3 seconds
                    target.SendMessage("You are immobilized by the lockpicking tool!");
                    Effects.SendLocationEffect(target.Location, target.Map, 0x376A, 10, 1, 0x481, 0);
                }
                else
                {
                    target.FixedParticles(0x374A, 10, 15, 5031, EffectLayer.Waist);
                    target.PlaySound(0x1F1);
                    target.Paralyzed = false;
                    target.Say("I feel disoriented...");
                    target.SendMessage("You feel disoriented!");
                    target.SendLocalizedMessage(1060415); // "You are confused!"
                }

                Caster.Mana -= RequiredMana;
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private LockpickingThrow m_Owner;

            public InternalTarget(LockpickingThrow owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
