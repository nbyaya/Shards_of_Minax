using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class Rejuvenate : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Rejuvenate", "In Vas Ylem Rel",
                                                        //SpellCircle.Third,
                                                        21005,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public Rejuvenate(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (target == null || target.Deleted || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (!Caster.InRange(target, 12))
            {
                Caster.SendLocalizedMessage(500446); // That is too far away.
                return;
            }

            if (CheckBSequence(target))
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                // Visual and Sound Effects
                Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5024);
                target.PlaySound(0x1F2);

                // Rejuvenate Effect
                int staminaRestored = (int)(Caster.Skills[CastSkill].Value * 0.5);
                int manaRestored = (int)(Caster.Skills[CastSkill].Value * 0.5);

                target.Stam += staminaRestored;
                target.Mana += manaRestored;

                target.SendMessage("You feel a surge of energy flow through you!");

                // Apply a temporary buff or visual effect to show they are invigorated
                if (target.Alive)
                {
                    target.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                    target.PlaySound(0x1F5);
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private Rejuvenate m_Owner;

            public InternalTarget(Rejuvenate owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
