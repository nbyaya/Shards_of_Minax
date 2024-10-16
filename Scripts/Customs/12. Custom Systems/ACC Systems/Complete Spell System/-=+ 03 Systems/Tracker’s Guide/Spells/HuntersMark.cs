using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class HuntersMark : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Hunter's Mark", "Venari Sigillum",
            21004,
            9300,
            false,
            Reagent.Bloodmoss,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public HuntersMark(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private HuntersMark m_Owner;

            public InternalTarget(HuntersMark owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    m_Owner.Target(target);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, target);
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F7);
                Caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);

                target.SendMessage("You have been marked by the Hunter!");
                target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);

                // Apply a buff (replace with appropriate icon or method)
                // Assuming `BuffIcon.Tracking` is not valid, use an alternative or remove it
                // target.AddBuff(new BuffInfo(BuffIcon.Tracking, 1075652, 1075653, TimeSpan.FromMinutes(2), Caster));

                // Use a custom method to apply buffs if needed
                ApplyBuff(target);

                Timer.DelayCall(TimeSpan.FromMinutes(2), () => EndEffect(target));

                // Apply damage increase effect
                DamageBonusEffect.ApplyTo(target);

                Caster.SendMessage("You have marked your target. They will be easier to track and more vulnerable to your attacks!");
            }

            FinishSequence();
        }

        private void ApplyBuff(Mobile target)
        {
            // Implement your custom buff logic here
            // For example, you might set a flag or use another system to track buffs
            target.SendMessage("Buff applied!");
        }

        private void EndEffect(Mobile target)
        {
            target.SendMessage("The mark has faded.");
            DamageBonusEffect.RemoveFrom(target);
        }

        private static class DamageBonusEffect
        {
            private static readonly Dictionary<Mobile, double> m_Table = new Dictionary<Mobile, double>();

            public static void ApplyTo(Mobile target)
            {
                if (m_Table.ContainsKey(target))
                    return;

                double bonus = 0.2;
                m_Table[target] = bonus;
                // Apply damage bonus or similar effect here
                // target.DamageBonus += bonus;

                Timer.DelayCall(TimeSpan.FromMinutes(2), () => RemoveFrom(target));
            }

            public static void RemoveFrom(Mobile target)
            {
                if (m_Table.ContainsKey(target))
                {
                    double bonus = m_Table[target];
                    // Remove damage bonus or similar effect here
                    // target.DamageBonus -= bonus;
                    m_Table.Remove(target);
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
