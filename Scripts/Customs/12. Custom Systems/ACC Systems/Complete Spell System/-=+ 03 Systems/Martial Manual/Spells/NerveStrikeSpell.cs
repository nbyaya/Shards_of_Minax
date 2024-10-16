using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Targeting; // Added namespace for Target

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class NerveStrikeSpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Nerve Strike", "Nerve",
            212,
            9041
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public NerveStrikeSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile target)
        {
            if (CheckSequence())
            {
                if (target == null)
                {
                    Caster.SendLocalizedMessage(500322); // You need a target to cast this spell.
                    FinishSequence();
                    return;
                }

                if (!Caster.CanBeHarmful(target, false))
                {
                    FinishSequence();
                    return;
                }

                Caster.DoHarmful(target);

                if (CheckMana())
                {
                    Caster.Mana -= RequiredMana;

                    bool immune = Server.Items.ParalyzingBlow.IsImmune(target);
                    bool doEffects = false;

                    double bushido = Caster.Skills[SkillName.Bushido].Value;
                    int damage = (int)(15.0 * (bushido - 50.0) / 70.0 + Utility.Random(10));

                    if (Core.ML)
                    {
                        AOS.Damage(target, Caster, damage, true, 100, 0, 0, 0, 0); // 0-25

                        if (!immune && ((150.0 / 7.0 + (4.0 * bushido) / 7.0) / 100.0) > Utility.RandomDouble())
                        {
                            target.Paralyze(TimeSpan.FromSeconds(2.0));
                            doEffects = true;
                        }

                        if (Caster is BaseCreature)
                            PetTrainingHelper.OnWeaponAbilityUsed((BaseCreature)Caster, SkillName.Bushido);
                    }
                    else
                    {
                        AOS.Damage(target, Caster, damage + 10, true, 100, 0, 0, 0, 0); // 10-25

                        if (!immune)
                        {
                            target.Freeze(TimeSpan.FromSeconds(2.0));
                            doEffects = true;
                        }
                    }

                    if (!immune)
                    {
                        Caster.SendLocalizedMessage(1063356); // You cripple your target with a nerve strike!
                        target.SendLocalizedMessage(1063357); // Your attacker dealt a crippling nerve strike!
                    }
                    else
                    {
                        Caster.SendLocalizedMessage(1070804); // Your target resists paralysis.
                        target.SendLocalizedMessage(1070813); // You resist paralysis.
                    }

                    if (doEffects)
                    {
                        Caster.PlaySound(0x204);
                        target.FixedEffect(0x376A, 9, 32);
                        target.FixedParticles(0x37C4, 1, 8, 0x13AF, 0, 0, EffectLayer.Waist);
                    }

                    Server.Items.ParalyzingBlow.BeginImmunity(target, Server.Items.ParalyzingBlow.FreezeDelayDuration);
                }
                else
                {
                    Caster.SendLocalizedMessage(1060178); // You don't have enough mana to cast that spell.
                }
            }

            FinishSequence();
        }

        private bool CheckMana()
        {
            return Caster.Mana >= RequiredMana;
        }

        private class InternalTarget : Target
        {
            private NerveStrikeSpell m_Owner;

            public InternalTarget(NerveStrikeSpell owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
                else
                    from.SendLocalizedMessage(500322); // You need a target to cast this spell.
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
