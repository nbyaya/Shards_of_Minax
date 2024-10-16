using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class GenuineSuffering : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Genuine Suffering", "Beg for Gold",
            21004, 9300,
            false
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } } // Adjust as needed

        public override double CastDelay { get { return 0.2; } } // 2 seconds cast delay
        public override double RequiredSkill { get { return 20.0; } } // Example: low skill requirement
        public override int RequiredMana { get { return 22; } }

        public GenuineSuffering(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (target == null || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (target is BaseCreature && ((BaseCreature)target).Controlled && CheckSequence())
            {
                BaseCreature npc = (BaseCreature)target;
                
                // Calculate gold amount based on the difference between max hits and current hits
                int goldAmount = (int)((Caster.HitsMax - Caster.Hits) * 0.5); // Example: 50% of the difference

                if (goldAmount > 0)
                {
                    // Visual and sound effects
                    Caster.FixedParticles(0x376A, 10, 15, 5030, EffectLayer.Waist); // Golden aura effect
                    Caster.PlaySound(0x1FA); // Coin sound effect

                    // Drop gold near the caster
                    Gold gold = new Gold(goldAmount);
                    gold.MoveToWorld(Caster.Location, Caster.Map);

                    Caster.SendMessage($"You receive {goldAmount} gold from your genuine suffering.");
                }
                else
                {
                    Caster.SendMessage("You are not wounded enough to receive any gold.");
                }
            }
            else
            {
                Caster.SendMessage("You must target a non-hostile NPC.");
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private GenuineSuffering m_Owner;

            public InternalTarget(GenuineSuffering owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
                else
                    from.SendMessage("You can only target a non-hostile NPC.");
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
