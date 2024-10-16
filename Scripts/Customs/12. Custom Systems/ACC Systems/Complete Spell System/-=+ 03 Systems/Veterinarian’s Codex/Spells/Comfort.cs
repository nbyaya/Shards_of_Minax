using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class Comfort : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Comfort", "Calmium Muu",
            21005, // Sound ID for casting
            9301   // Animation ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 20; } }

        public Comfort(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (target is BaseCreature creature)
            {
                if (CheckSequence() && Caster.CanBeBeneficial(target))
                {
                    // Play calming sound and show visual effects
                    Caster.PlaySound(21005);
                    Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5023);

                    // Reduce aggression
                    creature.AIObject.Action = ActionType.Wander;
                    creature.Combatant = null;
                    creature.Warmode = false;

                    // Improve morale (increase defense or reduce damage temporarily)
                    int moraleBoost = 5 + (int)(Caster.Skills[CastSkill].Value / 20);
                    creature.VirtualArmorMod += moraleBoost;

                    // Display message to show that the creature has calmed down
                    creature.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*calms down*");

                    // Timer to restore normal state after a period
                    Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                    {
                        if (creature != null && !creature.Deleted)
                        {
                            creature.VirtualArmorMod -= moraleBoost;
                        }
                    });
                }
                else
                {
                    Caster.SendLocalizedMessage(500951); // Target cannot be calmed.
                }
            }
            else
            {
                Caster.SendLocalizedMessage(500949); // That is not a valid target.
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private Comfort m_Owner;

            public InternalTarget(Comfort owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
