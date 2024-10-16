using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class WardOfProtection : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ward of Protection", "Defendo Custodia",
            21004,
            9300,
            false,
            Reagent.Garlic,
            Reagent.SpidersSilk,
            Reagent.SulfurousAsh
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 25; } }

        public WardOfProtection(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (target == null || !target.Alive || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen or is not valid.
                return;
            }

            if (CheckSequence())
            {
                SpellHelper.Turn(Caster, target);
                Effects.SendTargetParticles(target, 0x376A, 10, 15, 5022, EffectLayer.CenterFeet);
                Effects.PlaySound(target.Location, target.Map, 0x1F2);
                target.SendMessage("You are protected by a mystical ward!");

                // Corrected BuffInfo.AddBuff call
                BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Protection, 1075643, 1075644, 30));

                // Corrected Timer.DelayCall call
                Timer.DelayCall(TimeSpan.FromSeconds(30), () => RemoveWard(target));
            }

            FinishSequence();
        }

        private void RemoveWard(Mobile target)
        {
            if (target != null)
            {
                BuffInfo.RemoveBuff(target, BuffIcon.Protection);
                target.SendMessage("The ward of protection fades away.");
            }
        }

        private class InternalTarget : Target
        {
            private WardOfProtection m_Owner;

            public InternalTarget(WardOfProtection owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    m_Owner.Target(target);
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen or is not valid.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
