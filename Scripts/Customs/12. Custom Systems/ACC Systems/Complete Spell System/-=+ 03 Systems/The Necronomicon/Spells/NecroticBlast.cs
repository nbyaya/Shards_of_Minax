using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class NecroticBlast : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Necrotic Blast", "Corx Por",
                                                        21004, // Icon ID
                                                        9300,  // Cast animation ID
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public NecroticBlast(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private NecroticBlast m_Owner;

            public InternalTarget(NecroticBlast owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (m_Owner.CheckHSequence(target))
                    {
                        SpellHelper.Turn(m_Owner.Caster, target);

                        Effects.SendMovingParticles(m_Owner.Caster, target, 0x36BD, 7, 0, false, true, 0, 0, 0, 0, 0, 0);
                        Effects.PlaySound(target.Location, target.Map, 0x1FB);

                        // Apply damage and debuff
                        AOS.Damage(target, m_Owner.Caster, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0); // Pure necrotic damage

                        // Apply healing reduction debuff
                        target.SendMessage("You feel a dark energy reducing your healing effectiveness!");
                        target.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
                        target.PlaySound(0x1F8);

                        Timer.DelayCall(TimeSpan.FromSeconds(10), () => RemoveHealingReduction(target));
                        target.Skills[SkillName.Healing].Base -= 30; // Reduce Healing skill by 30 temporarily
                    }
                }

                m_Owner.FinishSequence();
            }

            private void RemoveHealingReduction(Mobile target)
            {
                if (target != null && !target.Deleted)
                {
                    target.SendMessage("The dark energy fades, and your healing effectiveness returns to normal.");
                    target.Skills[SkillName.Healing].Base += 30; // Restore Healing skill back to normal
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
