using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class ArmorShatteringStrike : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Armor Shattering Strike", "Krang Krush!",
            21004,
            9300,
            false,
            Reagent.SulfurousAsh
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public ArmorShatteringStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ArmorShatteringStrike m_Owner;

            public InternalTarget(ArmorShatteringStrike owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        // Visual and sound effects
                        Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
                        Effects.PlaySound(target.Location, target.Map, 0x307);

                        // Deal damage
                        int damage = Utility.RandomMinMax(20, 40);
                        SpellHelper.Damage(TimeSpan.FromSeconds(0.5), target, from, damage, 100, 0, 0, 0, 0);

                        // Apply temporary physical resistance reduction
                        int physResistReduction = 20;
                        TimeSpan duration = TimeSpan.FromSeconds(10);
                        ResistanceMod mod = new ResistanceMod(ResistanceType.Physical, -physResistReduction);
                        target.AddResistanceMod(mod);

                        Timer.DelayCall(duration, () => target.RemoveResistanceMod(mod));

                        from.SendMessage("You strike with a shattering blow, reducing your target's armor!");
                        target.SendMessage("Your armor has been shattered, reducing your physical resistance!");
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
