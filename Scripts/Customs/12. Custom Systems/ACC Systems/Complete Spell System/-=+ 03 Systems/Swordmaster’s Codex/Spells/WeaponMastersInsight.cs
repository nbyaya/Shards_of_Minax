using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class WeaponMastersInsight : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Weapon Masters Insight", "Reveal Weakness",
            21016,
            9415
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 35; } }

        public WeaponMastersInsight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckHSequence(target))
            {
                if (target is BaseCreature)
                {
                    BaseCreature creature = (BaseCreature)target;
                    double duration = 10.0 + Caster.Skills[CastSkill].Value * 0.1; // Duration based on caster's skill level
                    int debuffAmount = (int)(Caster.Skills[CastSkill].Value * 0.1); // Debuff amount based on caster's skill

                    creature.FixedParticles(0x375A, 10, 15, 5018, EffectLayer.Waist);
                    creature.PlaySound(0x1E9);
                    creature.SendMessage("You feel your defenses weakening!");

                    creature.VirtualArmorMod -= debuffAmount; // Reduce target's defense

                    Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                    {
                        creature.VirtualArmorMod += debuffAmount; // Restore defense after duration
                        creature.SendMessage("Your defenses return to normal.");
                    });
                }

                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Head); // Caster visual effect
                Caster.PlaySound(0x1E9); // Caster sound effect
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private WeaponMastersInsight m_Owner;

            public InternalTarget(WeaponMastersInsight owner) : base(12, false, TargetFlags.Harmful)
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
