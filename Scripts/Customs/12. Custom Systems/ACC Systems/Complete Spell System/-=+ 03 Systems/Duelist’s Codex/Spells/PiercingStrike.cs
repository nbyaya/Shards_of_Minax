using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class PiercingStrike : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Piercing Strike", "Ex Piercio",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.5; } } // Quick cast
        public override double RequiredSkill { get { return 50.0; } } // Moderate skill requirement
        public override int RequiredMana { get { return 20; } } // Mana cost

        public PiercingStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PiercingStrike m_Owner;

            public InternalTarget(PiercingStrike owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && m_Owner.CheckHSequence(target))
                {
                    SpellHelper.Turn(from, target);

                    // Display visual and play sound effect
                    from.PlaySound(0x1F2); // Thrust sound
                    from.FixedParticles(0x36B0, 1, 15, 9935, 1153, 7, EffectLayer.Waist); // Visual effect

                    // Damage calculation
                    double damage = Utility.RandomMinMax(30, 50); // Heavy damage
                    from.DoHarmful(target);
                    SpellHelper.Damage(TimeSpan.Zero, target, from, damage);

                    // Armor reduction effect
                    int armorReduction = 10 + (int)(from.Skills[SkillName.Swords].Value / 10);
                    TimeSpan duration = TimeSpan.FromSeconds(10.0 + from.Skills[SkillName.Swords].Value * 0.1);

                    // Flashy visual effect on target for armor reduction
                    target.FixedParticles(0x373A, 1, 15, 9911, 1153, 7, EffectLayer.Waist); // Armor break visual
                    target.PlaySound(0x1FA); // Armor breaking sound
                }

                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0); // Quick ability execution
        }
    }
}
