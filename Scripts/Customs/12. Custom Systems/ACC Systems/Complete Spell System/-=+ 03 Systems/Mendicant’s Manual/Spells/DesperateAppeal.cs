using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class DesperateAppeal : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Desperate Appeal", "Clamorous Appeal!",
            // Visual and sound effects
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 25.0; } }
        public override int RequiredMana { get { return 25; } }

        private static TimeSpan Cooldown = TimeSpan.FromSeconds(60); // Cooldown period of 60 seconds
        private DateTime m_NextUseAllowed;

        public DesperateAppeal(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (DateTime.UtcNow < m_NextUseAllowed)
            {
                Caster.SendMessage("You must wait before using Desperate Appeal again.");
                return;
            }

            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (target is BaseCreature && ((BaseCreature)target).Controlled)
            {
                Caster.SendMessage("This spell does not work on player-controlled creatures.");
            }
            else if (CheckSequence())
            {
                m_NextUseAllowed = DateTime.UtcNow + Cooldown;

                SpellHelper.Turn(Caster, target);

                if (target is BaseCreature) // Check if target is a BaseCreature (NPC)
                {
                    // Flashy visuals and sounds
                    target.PlaySound(0x1FB);
                    target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);

                    // Dramatic increase in chance of donation
                    double successChance = Caster.Skills[SkillName.Begging].Value / 100.0 + 0.5; // 50% base chance + begging skill factor

                    if (Utility.RandomDouble() < successChance)
                    {
                        target.Say("I feel compelled to give you something!");
                        target.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*hands over a generous donation*");

                        // Create a donation (Gold, Item, etc.)
                        Item donation = new Gold(Utility.RandomMinMax(50, 200)); // Gold between 50 and 200
                        Caster.AddToBackpack(donation);
                    }
                    else
                    {
                        target.Say("I have nothing to give you!");
                    }
                }
                else
                {
                    Caster.SendMessage("This spell only works on NPCs.");
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private DesperateAppeal m_Owner;

            public InternalTarget(DesperateAppeal owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
                else
                    from.SendMessage("You must target a living being.");
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
