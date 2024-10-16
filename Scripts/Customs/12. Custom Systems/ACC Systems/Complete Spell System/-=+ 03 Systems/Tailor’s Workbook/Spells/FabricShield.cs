using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Misc;
using System.Collections;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class FabricShield : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Fabric Shield", "Aria Tela",
            21003, // Spell ID (icon)
            9300,  // Action ID (animation)
            false,
            Reagent.SpidersSilk,
            Reagent.Ginseng,
            Reagent.SulfurousAsh
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 30; } }

        public FabricShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You weave an enchanted fabric shield around yourself!");

                // Visual effects
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 1153, 0, 5023, 0);
                Caster.PlaySound(0x1ED);

                // Apply shield buff
                new FabricShieldBuff(Caster, TimeSpan.FromSeconds(30.0), 0.25); // 25% damage absorption for 30 seconds

                FinishSequence();
            }
        }
    }

    public class FabricShieldBuff
    {
        private double m_DamageAbsorption;
        private Timer m_Timer;
        private Mobile m_Caster;

        public FabricShieldBuff(Mobile caster, TimeSpan duration, double damageAbsorption)
        {
            m_Caster = caster;
            m_DamageAbsorption = damageAbsorption;

            if (m_Caster.BeginAction(typeof(FabricShieldBuff)))
            {
                m_Timer = new FabricShieldTimer(caster, this, duration);
                m_Timer.Start();
            }
            else
            {
                caster.SendLocalizedMessage(500237); // You are already under a protective spell.
            }
        }

        public void End()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            m_Caster.EndAction(typeof(FabricShieldBuff));
        }

        public void OnDamage(ref int damage)
        {
            int absorbed = (int)(damage * m_DamageAbsorption);
            damage -= absorbed;
            m_Caster.FixedParticles(0x376A, 1, 14, 1153, 2, 2, EffectLayer.Waist);
            m_Caster.SendMessage("The fabric shield absorbs some of the damage!");
        }

        private class FabricShieldTimer : Timer
        {
            private Mobile m_Caster;
            private FabricShieldBuff m_Buff;

            public FabricShieldTimer(Mobile caster, FabricShieldBuff buff, TimeSpan duration) : base(duration)
            {
                m_Caster = caster;
                m_Buff = buff;
            }

            protected override void OnTick()
            {
                m_Buff.End();
                m_Caster.SendMessage("Your fabric shield dissipates.");
            }
        }
    }
}
