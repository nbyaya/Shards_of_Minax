using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class StealthAssailant : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Stealth Assailant", "In Silentium Hostis",
            21001,
            9301,
            false,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 10; } }

        public StealthAssailant(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster.Hidden && Caster is PlayerMobile)
            {
                Caster.Target = new InternalTarget(this);
            }
            else
            {
                Caster.SendMessage("You must be hidden to use this ability.");
                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private StealthAssailant m_Owner;

            public InternalTarget(StealthAssailant owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && target != m_Owner.Caster && target.Alive && from.InRange(target, 1))
                {
                    m_Owner.Strike(target);
                }
                else
                {
                    from.SendMessage("That is not a valid target.");
                    m_Owner.FinishSequence();
                }
            }
        }

        public void Strike(Mobile target)
        {
            if (Caster.CanSee(target) && CheckSequence())
            {
                Caster.RevealingAction();
                SpellHelper.Turn(Caster, target);

                // Play sound effect
                Caster.PlaySound(0x1F5);

                // Visual effect: Shadowy strike animation
                target.FixedParticles(0x374A, 10, 15, 5030, EffectLayer.Head);

                // Apply surprise damage
                double damage = Caster.Skills[SkillName.Lockpicking].Value * 0.3;
                damage += Utility.RandomMinMax(15, 30); // Random bonus damage

                target.Damage((int)damage, Caster);

                // Flashy visual and sound effects for a critical hit
                if (Utility.RandomDouble() < 0.25) // 25% chance for a critical hit
                {
                    target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist);
                    target.PlaySound(0x208);
                    target.SendMessage("You feel a sharp pain as the attack critically strikes!");
                }

                // Additional visual feedback on successful stealth attack
                Caster.FixedParticles(0x376A, 10, 15, 5030, EffectLayer.Waist);
            }
            else
            {
                Caster.SendMessage("You fail to strike your target.");
            }

            FinishSequence();
        }
    }
}
