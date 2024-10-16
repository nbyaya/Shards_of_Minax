using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class DiseaseResistance : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disease Resistance", "Vas An Corp",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } } // 2 seconds casting delay
        public override double RequiredSkill { get { return 30.0; } } // Required skill level
        public override int RequiredMana { get { return 20; } } // Mana cost

        public DiseaseResistance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private DiseaseResistance m_Owner;

            public InternalTarget(DiseaseResistance owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (!from.CanBeBeneficial(target))
                    {
                        from.SendMessage("You cannot benefit that target.");
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        from.DoBeneficial(target);

                        int duration = (int)(from.Skills[m_Owner.CastSkill].Value * 0.2); // Duration in seconds
                        if (duration < 10)
                            duration = 10; // Minimum duration of 10 seconds

                        target.PlaySound(0x1F2); // Sound effect for resistance boost
                        target.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Visual effect

                        ResistanceMod poisonResistMod = new ResistanceMod(ResistanceType.Poison, 20); // Increase Poison Resistance by 20
                        ResistanceMod coldResistMod = new ResistanceMod(ResistanceType.Cold, 10); // Increase Cold Resistance by 10

                        target.AddResistanceMod(poisonResistMod);
                        target.AddResistanceMod(coldResistMod);

                        Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                        {
                            target.RemoveResistanceMod(poisonResistMod);
                            target.RemoveResistanceMod(coldResistMod);
                            target.SendMessage("Your resistance to diseases and poisons has returned to normal.");
                        });

                        m_Owner.FinishSequence();
                    }
                }
                else
                {
                    from.SendMessage("You must target a creature.");
                }
            }

            protected void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
