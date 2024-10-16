using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class Antidote : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Antidote", "Cure Poison!",
                                                        21005,
                                                        9301
                                                       );

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public Antidote(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Antidote m_Owner;

            public InternalTarget(Antidote owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && m_Owner.Caster.CanBeBeneficial(target))
                {
                    m_Owner.Caster.DoBeneficial(target);

                    // You may need to adjust the following logic depending on the functionality of your spell
                    // If you need to replace `CheckBSequence` with `CheckSequence`, you can uncomment the following line:
                    // if (SpellHelper.CheckSequence(m_Owner.Caster, target))
                    
                    {
                        // Cure the target of poison
                        if (target.Poisoned)
                        {
                            target.CurePoison(m_Owner.Caster);
                            target.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                            target.PlaySound(0x1E0);
                            target.SendMessage("You have been cured of poison!");
                        }

                        // Apply disease cure or resistance to disease effect
                        if (target.BodyMod == 0) // Assuming disease effect changes BodyMod
                        {
                            target.BodyMod = 0; // Cure disease, if any
                            target.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                            target.PlaySound(0x1F9);
                            target.SendMessage("You feel relieved as the disease fades away!");
                        }
                        else
                        {
                            // Apply resistance to future harmful conditions for a short duration
                            BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.ReactiveArmor, 1075810, 1075812, TimeSpan.FromSeconds(10), target));
                            target.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                            target.PlaySound(0x1E0);
                            target.SendMessage("You feel temporarily protected against harmful conditions.");
                        }
                    }
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
