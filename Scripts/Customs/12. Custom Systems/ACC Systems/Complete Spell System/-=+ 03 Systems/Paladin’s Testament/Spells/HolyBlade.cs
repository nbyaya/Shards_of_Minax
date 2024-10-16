using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class HolyBlade : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Holy Blade", "Divinus Ensis",
            21012, // Animation ID
            9300   // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public HolyBlade(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile target = Caster;
                target.SendMessage("You feel a divine power course through your weapon!");

                if (target.Weapon is BaseWeapon)
                {
                    BaseWeapon weapon = (BaseWeapon)target.Weapon;
                    HolyBladeEffect effect = new HolyBladeEffect(weapon, Caster);
                    effect.Start();

                    Effects.PlaySound(target.Location, target.Map, 0x29); // Sound effect for divine power
                    target.FixedParticles(0x376A, 9, 32, 5008, 1153, 4, EffectLayer.Waist); // Visual effect for holy enchantment
                }
                else
                {
                    Caster.SendMessage("You must be wielding a weapon to use this spell.");
                }
            }

            FinishSequence();
        }

        private class HolyBladeEffect
        {
            private Timer m_Timer;
            private BaseWeapon m_Weapon;
            private Mobile m_Caster;

            public HolyBladeEffect(BaseWeapon weapon, Mobile caster)
            {
                m_Weapon = weapon;
                m_Caster = caster;
            }

            public void Start()
            {
                if (m_Timer != null)
                    m_Timer.Stop();

                m_Timer = new InternalTimer(this, m_Weapon, m_Caster);
                m_Timer.Start();
            }

            private class InternalTimer : Timer
            {
                private BaseWeapon m_Weapon;
                private Mobile m_Caster;
                private HolyBladeEffect m_Owner;

                public InternalTimer(HolyBladeEffect owner, BaseWeapon weapon, Mobile caster)
                    : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
                {
                    m_Owner = owner;
                    m_Weapon = weapon;
                    m_Caster = caster;
                    Priority = TimerPriority.TwoFiftyMS;
                }

                protected override void OnTick()
                {
                    if (m_Caster == null || m_Weapon == null || m_Weapon.Deleted)
                    {
                        Stop();
                        return;
                    }

                    if (Utility.RandomDouble() < 0.15) // 15% chance to smite with holy fire
                    {
                        Mobile target = m_Caster.Combatant as Mobile;

                        if (target != null && target.Alive && m_Caster.InRange(target, 10))
                        {
                            int damage = (int)(m_Caster.Skills[SkillName.Magery].Value * 0.5);
                            target.Damage(damage, m_Caster);
                            target.FixedParticles(0x3709, 10, 30, 5052, 1153, 7, EffectLayer.LeftFoot); // Holy fire effect
                            target.PlaySound(0x208); // Sound effect for holy fire
                        }
                    }
                }
            }
        }
    }
}
