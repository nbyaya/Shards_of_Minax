using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class EnemyWeakness : TacticsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Enemy Weakness", "Reveal Defectum",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 25; } }

        public EnemyWeakness(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (Caster.CanBeHarmful(m) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                Caster.DoHarmful(m);

                // Apply debuff to target
                m.SendMessage("You feel your defenses weaken!");
                m.PlaySound(0x1FB); // Play a weakening sound effect
                m.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Head); // Display particle effect on target

                // Increase damage taken by 20% for 30 seconds
                int damageIncrease = 20;
                TimeSpan duration = TimeSpan.FromSeconds(30.0);

                m.AddStatMod(new StatMod(StatType.All, "EnemyWeaknessDebuff", -damageIncrease, duration));

                // Broadcast effect to nearby allies
                foreach (Mobile potentialAlly in Caster.GetMobilesInRange(8))
                {
                    if (potentialAlly != Caster && potentialAlly.Alive && Caster.CanBeBeneficial(potentialAlly))
                    {
                        potentialAlly.SendMessage("The enemy's weakness has been revealed!");
                        potentialAlly.PlaySound(0x5C2); // Play a buff sound effect
                        potentialAlly.FixedParticles(0x373A, 1, 15, 9902, EffectLayer.Waist); // Display particle effect on allies
                    }
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private EnemyWeakness m_Owner;

            public InternalTarget(EnemyWeakness owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
