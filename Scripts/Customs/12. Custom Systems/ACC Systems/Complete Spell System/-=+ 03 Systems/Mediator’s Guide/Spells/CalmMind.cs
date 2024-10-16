using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class CalmMind : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Calm Mind", "Sano Animo",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; } // Adjust the spell circle as needed
        }

        public override double CastDelay { get { return 0.2; } } // Delay before the spell takes effect
        public override double RequiredSkill { get { return 50.0; } } // Required skill level
        public override int RequiredMana { get { return 25; } } // Mana cost

        public CalmMind(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile target)
        {
            if (Caster == null || target == null)
            {
                Caster.SendMessage("Invalid target.");
                return;
            }

            if (target.Alive && (target == Caster || Caster.CanBeBeneficial(target)))
            {
                if (CheckSequence())
                {
                    // Play flashy effects
                    Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x373A, 1, 17, 1153, 3, 9962, 0);
                    Effects.PlaySound(target.Location, target.Map, 0x1F7); // Sound effect

                    // Remove debuffs and status effects
                    RemoveDebuffs(target);

                    // Message feedback
                    Caster.SendMessage("You feel a calm energy cleanse you.");
                    target.SendMessage("You feel a calm energy cleanse you.");
                }
            }
            else
            {
                Caster.SendMessage("You cannot cleanse that target.");
            }

            FinishSequence();
        }

        private void RemoveDebuffs(Mobile target)
        {
            // Example: Remove poison and curse effects
            target.CurePoison(Caster);
            target.Paralyzed = false; // Remove paralysis
            target.Frozen = false; // Remove freezing

            // Additional custom debuffs can be removed here as needed
        }

        private class InternalTarget : Target
        {
            private CalmMind m_Owner;

            public InternalTarget(CalmMind owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                    m_Owner.Target(target);
                else
                    from.SendMessage("That is not a valid target.");
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
