using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class KeybladeAttack : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Keyblade Attack", "Fraa Vel'Inas",
            21001,
            9301,
            false,
            Reagent.Bloodmoss,
            Reagent.BlackPearl,
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 10; } }

        public KeybladeAttack(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private KeybladeAttack m_Owner;

            public InternalTarget(KeybladeAttack owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (m_Owner.CheckHSequence(target))
                    {
                        SpellHelper.Turn(m_Owner.Caster, target);
                        m_Owner.Caster.MovingEffect(target, 0x1FBD, 10, 0, false, false, 1153, 0); // Keyblade visual effect
                        target.FixedParticles(0x374A, 10, 15, 5037, EffectLayer.Waist); // Flashy particle effect
                        target.PlaySound(0x1FA); // Sound effect

                        double damage = Utility.RandomMinMax(15, 25); // Randomized damage

                        // Apply damage to the target
                        AOS.Damage(target, m_Owner.Caster, (int)damage, 100, 0, 0, 0, 0);

                        from.SendMessage("You strike your enemy with a magically enhanced keyblade, dealing extra damage!");
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
