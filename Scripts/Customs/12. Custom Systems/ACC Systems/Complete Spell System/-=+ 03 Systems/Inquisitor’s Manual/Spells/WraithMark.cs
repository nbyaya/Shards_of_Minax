using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class WraithMark : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Wraith Mark", "Re Vel An Lor",
            21004,
            9300,
            false,
            Reagent.Nightshade,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public WraithMark(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private WraithMark m_Owner;

            public InternalTarget(WraithMark owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && target.Hidden && m_Owner.CheckSequence())
                {
                    // Reveal the target
                    target.RevealingAction();

                    // Play visual effects and sounds
                    target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                    target.PlaySound(0x1F3);

                    // Apply debuffs and mark effects
                    target.SendMessage("You have been marked by the Wraith!");
                    target.AddStatMod(new StatMod(StatType.Dex, "WraithMarkDex", -20, TimeSpan.FromSeconds(15)));
                    target.FixedEffect(0x375A, 10, 15);

                    // Increase damage taken by the target
                    Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), 15, () =>
                    {
                        if (target == null || target.Deleted)
                            return;

                        target.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*Wraith Mark effect*");
                        target.Damage(Utility.RandomMinMax(3, 5), from);
                    });

                    // Finish the sequence and consume reagents
                    m_Owner.FinishSequence();
                    m_Owner.ConsumeReagents();
                }
                else
                {
                    from.SendMessage("You cannot see that target.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
