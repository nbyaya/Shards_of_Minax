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
    public class ArmorIgnoreSpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Armor Ignore", "Vorpal",
            212,
            9041
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 30; } }

        public ArmorIgnoreSpell(Mobile caster, Item scroll)
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
                Caster.FixedEffect(0x3728, 10, 15);
                Caster.PlaySound(0x2A1);

                if (SpellHelper.ValidIndirectTarget(Caster, target) && Caster.CanBeHarmful(target))
                {
                    Caster.RevealingAction();
                    Caster.DoHarmful(target);

                    target.SendLocalizedMessage(1060077); // The blow penetrated your armor!
                    target.FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);
                    target.PlaySound(0x56);

                    double damageScalar = 0.9;
                    int damage = ComputeDamage(Caster, target);
                    damage = (int)(damage * damageScalar);

                    if (Core.AOS)
                    {
                        AOS.Damage(target, Caster, damage, 100, 0, 0, 0, 0);
                    }
                    else
                    {
                        target.Damage(damage);
                    }
                }
                else
                {
                    Caster.SendLocalizedMessage(501857); // That is not a valid target.
                }

                FinishSequence();
            }
            else
            {
                FinishSequence();
            }
        }

        private int ComputeDamage(Mobile attacker, Mobile defender)
        {
            int damage = attacker.Skills[SkillName.ArmsLore].Fixed / 10;

            if (Core.AOS)
            {
                int physDamage = damage;
                AOS.Damage(defender, attacker, physDamage, 100, 0, 0, 0, 0);
                return physDamage;
            }
            else
            {
                defender.Damage(damage);
                return damage;
            }
        }

        private class InternalTarget : Target
        {
            private ArmorIgnoreSpell m_Owner;

            public InternalTarget(ArmorIgnoreSpell owner)
                : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                }
                else
                {
                    from.SendLocalizedMessage(501857); // That is not a valid target.
                }
            }
        }
    }
}
