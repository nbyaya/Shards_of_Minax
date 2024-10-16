using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class WhirlwindAttackSpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Whirlwind Attack", "Vorpal",
            212,
            9041
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 15; } }

        public WhirlwindAttackSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedEffect(0x3728, 10, 15);
                Caster.PlaySound(0x2A1);

                List<Mobile> targets = new List<Mobile>();
                IPooledEnumerable eable = Caster.GetMobilesInRange(1);

                foreach (Mobile m in eable)
                {
                    if (m != Caster && SpellHelper.ValidIndirectTarget(Caster, m))
                    {
                        if (m == null || m.Deleted || m.Map != Caster.Map || !m.Alive || !Caster.CanSee(m) || !Caster.CanBeHarmful(m))
                            continue;

                        if (!Caster.InRange(m, 1))
                            continue;

                        if (Caster.InLOS(m))
                            targets.Add(m);
                    }
                }

                eable.Free();

                if (targets.Count > 0)
                {
                    double bushido = Caster.Skills[SkillName.ArmsLore].Value;
                    double damageBonus = 1.0 + Math.Pow((targets.Count * bushido) / 60, 2) / 100;

                    if (damageBonus > 2.0)
                        damageBonus = 2.0;

                    Caster.RevealingAction();

                    for (int i = 0; i < targets.Count; ++i)
                    {
                        Mobile m = targets[i];

                        Caster.SendLocalizedMessage(1060161); // The whirling attack strikes a target!
                        m.SendLocalizedMessage(1060162); // You are struck by the whirling attack and take damage!

                        int damage = ComputeDamage(Caster, m);
                        damage = (int)(damage * damageBonus);

                        Caster.DoHarmful(m);
                        SpellHelper.Damage(this, m, damage, 100, 0, 0, 0, 0);
                    }
                }
                else
                {
                    Caster.SendLocalizedMessage(1060161); // The whirling attack strikes a target!
                }
            }

            FinishSequence();
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
    }
}