using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class HealthTransfer : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Health Transfer", "Vas Mini",
            21004,
            9300,
            false,
            Reagent.Ginseng,
            Reagent.Garlic
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 25; } }

        public HealthTransfer(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private HealthTransfer m_Owner;

            public InternalTarget(HealthTransfer owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (target == from)
                    {
                        from.SendMessage("You cannot target yourself.");
                    }
                    else if (!from.CanBeBeneficial(target))
                    {
                        from.SendMessage("You cannot heal that target.");
                    }
                    else if (m_Owner.CheckSequence())
                    {
                        from.DoBeneficial(target);

                        int toTransfer = (int)(from.Hits * 0.2); // Transfer 20% of the caster's health
                        if (toTransfer < 1) toTransfer = 1; // Ensure at least 1 HP is transferred

                        if (from.Hits > toTransfer)
                        {
                            from.Hits -= toTransfer;
                            target.Hits += toTransfer;

                            from.PlaySound(0x214);
                            from.FixedParticles(0x376A, 10, 15, 5032, EffectLayer.Waist);
                            target.PlaySound(0x1F2);
                            target.FixedParticles(0x376A, 10, 15, 5032, EffectLayer.Waist);

                            from.SendMessage("You transfer a portion of your health to the target.");
                            target.SendMessage("You receive a portion of health from the caster.");
                        }
                        else
                        {
                            from.SendMessage("You do not have enough health to transfer.");
                        }
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid target.");
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
