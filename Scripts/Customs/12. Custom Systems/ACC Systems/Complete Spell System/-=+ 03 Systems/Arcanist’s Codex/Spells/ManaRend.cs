using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class ManaRend : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mana Rend", "Rend Mana!",
            //SpellCircle.Fifth,
            21005,
            9403
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public ManaRend(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ManaRend m_Owner;

            public InternalTarget(ManaRend owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && target != from)
                {
                    if (m_Owner.CheckSequence())
                    {
                        if (target.Mana > 0)
                        {
                            // Calculate the mana to drain (e.g., 50% of the target's mana)
                            int manaDrain = (int)(target.Mana * 0.5);
                            manaDrain = Math.Max(manaDrain, 10); // Ensure at least 10 mana is drained

                            // Drain mana from the target
                            target.Mana -= manaDrain;
                            // Replenish caster's mana
                            m_Owner.Caster.Mana += manaDrain / 2; // Caster gets half of the drained mana

                            // Calculate the damage based on the drained mana (e.g., 1 damage per 10 mana drained)
                            int damage = (int)(manaDrain / 10.0);
                            damage = Math.Max(damage, 5); // Ensure at least 5 damage is dealt

                            // Deal damage to the target
                            target.Damage(damage, m_Owner.Caster);

                            // Play sound and visual effects
                            Effects.PlaySound(target.Location, target.Map, 0x1F2); // High-pitched magical sound
                            Effects.SendLocationEffect(target.Location, target.Map, 0x1F2, 30); // Mana drain visual effect
                            m_Owner.Caster.FixedEffect(0x373A, 1, 20); // Caster visual effect

                            // Notify the caster of success
                            m_Owner.Caster.SendMessage("You rend the target's mana and replenish your own!");
                        }
                        else
                        {
                            m_Owner.Caster.SendMessage("The target has no mana to drain.");
                        }
                    }
                }
                else
                {
                    m_Owner.Caster.SendMessage("You cannot target yourself.");
                }

                m_Owner.FinishSequence();
            }
        }
    }
}
