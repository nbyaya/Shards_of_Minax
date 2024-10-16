using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class RadiantHeal : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Radiant Heal", "Lux Vita",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public RadiantHeal(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (target is Mobile && CheckBSequence(target))
            {
                Mobile m = (Mobile)target;

                if (this.Scroll != null)
                    Scroll.Consume();

                // Heal the target
                int toHeal = Utility.RandomMinMax(20, 40);
                m.Heal(toHeal);
                m.FixedParticles(0x376A, 1, 29, 9949, 1153, 3, EffectLayer.Waist); // Healing visual effect
                m.PlaySound(0x202); // Healing sound effect

                // Damage nearby enemies
                ArrayList list = new ArrayList();

                foreach (Mobile enemy in m.GetMobilesInRange(3))
                {
                    if (enemy != Caster && enemy != m && Caster.CanBeHarmful(enemy) && !Caster.CanBeHarmful(enemy))
                        list.Add(enemy);
                }

                for (int i = 0; i < list.Count; ++i)
                {
                    Mobile enemy = (Mobile)list[i];

                    int damage = (int)(toHeal * 0.5); // Damage based on half the healed amount
                    Caster.DoHarmful(enemy);
                    AOS.Damage(enemy, Caster, damage, 100, 0, 0, 0, 0); // Apply the damage

                    enemy.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Damage visual effect
                    enemy.PlaySound(0x208); // Damage sound effect
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private RadiantHeal m_Owner;

            public InternalTarget(RadiantHeal owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
