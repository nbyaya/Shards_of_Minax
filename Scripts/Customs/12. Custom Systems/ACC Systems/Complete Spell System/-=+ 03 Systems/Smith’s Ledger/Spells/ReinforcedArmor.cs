using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class ReinforcedArmor : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Reinforced Armor", "An Cra Ol",
                                                        //SpellCircle.Fourth,
                                                        21008,
                                                        9304,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public ReinforcedArmor(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
            else if (target is PlayerMobile && target.FindItemOnLayer(Layer.InnerTorso) is BaseArmor armor)
            {
                if (CheckSequence())
                {
                    if (this.Scroll != null)
                        Scroll.Consume();
                    SpellHelper.Turn(Caster, target);

                    Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5022);
                    target.PlaySound(0x1F7);

                    double duration = 10.0 + (Caster.Skills[CastSkill].Value * 0.1);
                    new InternalTimer(target, armor, TimeSpan.FromSeconds(duration)).Start();

                    Caster.SendMessage("You have reinforced the armor!");
                    target.SendMessage("Your armor is temporarily reinforced, reducing damage received.");
                }
            }
            else
            {
                Caster.SendLocalizedMessage(1049456); // You must target a valid piece of armor.
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private ReinforcedArmor m_Owner;

            public InternalTarget(ReinforcedArmor owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Target;
            private BaseArmor m_Armor;
            private double m_OriginalArmorBonus;

            public InternalTimer(Mobile target, BaseArmor armor, TimeSpan duration) : base(duration)
            {
                m_Target = target;
                m_Armor = armor;
                m_OriginalArmorBonus = armor.PhysicalBonus;

                double armorBoost = 10.0 + (m_Target.Skills[SkillName.Blacksmith].Value * 0.1);
                armor.PhysicalBonus += (int)armorBoost;

                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                m_Armor.PhysicalBonus = (int)m_OriginalArmorBonus;
                m_Target.PlaySound(0x1F8);
                m_Target.SendMessage("The reinforcement on your armor has worn off.");
                Stop();
            }
        }
    }
}
