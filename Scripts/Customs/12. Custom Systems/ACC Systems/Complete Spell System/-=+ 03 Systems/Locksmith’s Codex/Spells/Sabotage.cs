using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class Sabotage : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Sabotage", "In Vas Ort Ox",
            21001,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }  // Reasonable casting delay
        public override double RequiredSkill { get { return 20.0; } }  // Reasonable skill requirement
        public override int RequiredMana { get { return 10; } }  // Mana cost as specified

        public Sabotage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Sabotage m_Owner;

            public InternalTarget(Sabotage owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (!from.CanBeHarmful(target))
                    {
                        from.SendLocalizedMessage(1001016); // You cannot target that.
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        
                        // Apply visual effects
                        Effects.SendTargetParticles(target, 0x376A, 9, 32, 5008, EffectLayer.Waist);
                        target.PlaySound(0x1FB);

                        // Inflict debuff
                        target.SendMessage("Your locks and traps have been sabotaged!");
                        target.FixedParticles(0x374A, 10, 15, 5037, EffectLayer.Head);
                        
                        // Custom debuff logic: Reducing effectiveness of locks and traps
                        if (target is PlayerMobile pm)
                        {
                            pm.AddStatMod(new StatMod(StatType.Dex, "SabotageDex", -10, TimeSpan.FromSeconds(30.0))); // Example debuff effect
                        }

                        // Additional flashy effects
                        Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
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
