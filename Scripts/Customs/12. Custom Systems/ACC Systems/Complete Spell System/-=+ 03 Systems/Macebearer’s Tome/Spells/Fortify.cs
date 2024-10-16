using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class Fortify : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Fortify", "Fortis Armor",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public Fortify(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (target == null || target.Deleted || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckBSequence(target))
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, target);

                // Apply visual effect and sound
                target.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                target.PlaySound(0x1F7);

                // Apply damage reduction buff with corrected argument order
                BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.DefenseMastery, 1075815, TimeSpan.FromSeconds(30), target));

                target.SendMessage("Your armor has been fortified, providing temporary damage reduction!");

                Timer.DelayCall(TimeSpan.FromSeconds(30), () => RemoveFortifyEffect(target));
            }

            FinishSequence();
        }

        private void RemoveFortifyEffect(Mobile target)
        {
            if (target == null || target.Deleted)
                return;

            BuffInfo.RemoveBuff(target, BuffIcon.DefenseMastery);
            target.SendMessage("The fortification on your armor has faded.");
        }

        private class InternalTarget : Target
        {
            private Fortify m_Owner;

            public InternalTarget(Fortify owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
