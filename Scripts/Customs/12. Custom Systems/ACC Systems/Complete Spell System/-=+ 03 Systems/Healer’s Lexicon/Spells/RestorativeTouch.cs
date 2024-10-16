using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class RestorativeTouch : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Restorative Touch", "An Corp Res",
            // SpellCircle.Third,
            21004,
            9300,
            false,
            Reagent.Garlic,
            Reagent.Ginseng,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public RestorativeTouch(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private RestorativeTouch m_Owner;

            public InternalTarget(RestorativeTouch owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (!from.CanBeBeneficial(target, false))
                    {
                        from.SendLocalizedMessage(500951); // You cannot heal that.
                    }
                    else if (m_Owner.CheckSequence())
                    {
                        SpellHelper.Turn(from, target);

                        from.DoBeneficial(target);

                        double healAmount = Utility.Random(20, 30) + (from.Skills[SkillName.Healing].Value / 2);
                        target.Heal((int)healAmount, from);

                        // Temporary resistance boost
                        TimeSpan duration = TimeSpan.FromSeconds(10.0 + (from.Skills[SkillName.Healing].Value * 0.1));
                        target.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, 10));
                        target.AddResistanceMod(new ResistanceMod(ResistanceType.Fire, 10));
                        target.AddResistanceMod(new ResistanceMod(ResistanceType.Cold, 10));
                        target.AddResistanceMod(new ResistanceMod(ResistanceType.Poison, 10));
                        target.AddResistanceMod(new ResistanceMod(ResistanceType.Energy, 10));

                        Timer.DelayCall(duration, () => RemoveResistanceMods(target));

                        // Visual and sound effects
                        target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                        target.PlaySound(0x1F2);
                    }
                }
                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private static void RemoveResistanceMods(Mobile target)
        {
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, 10));
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Fire, 10));
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Cold, 10));
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Poison, 10));
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Energy, 10));
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
