using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class AnimalsGrace : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Animal's Grace", "Dexio Bexio",
            21005,
            9400
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 12; } }

        public AnimalsGrace(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply the effects
                Caster.PlaySound(0x4B0); 
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); 

                int dexBonus = (int)(Caster.Dex * 0.3); 
                Caster.AddStatMod(new StatMod(StatType.Dex, "AnimalGraceDex", dexBonus, TimeSpan.FromSeconds(30)));

                Caster.Stam += (int)(Caster.StamMax * 0.1); 

                // Corrected Timer usage
                Timer timer = new AnimalGraceStaminaTimer(Caster);
                timer.Start();

                Timer.DelayCall(TimeSpan.FromSeconds(30), () => EndStaminaEffect(timer));

                // Corrected BuffInfo instantiation
                BuffInfo.AddBuff(
                    Caster, 
                    new BuffInfo(
                        BuffIcon.Bless, 
                        1152, 
                        30, 
                        TimeSpan.FromSeconds(1), 
                        Caster, 
                        true
                    )
                );

                Caster.SendMessage("You feel a surge of agility and vitality as the animal's grace flows through you.");
            }

            FinishSequence();
        }

        private void IncreaseStamina(Mobile caster)
        {
            if (caster == null || caster.Deleted)
                return;

            caster.Stam += 1; // Increase stamina by 1 every 5 seconds
        }

        private void EndStaminaEffect(Timer timer)
        {
            if (timer != null && timer.Running)
                timer.Stop();

            Caster.SendMessage("The effect of Animal's Grace has worn off.");
        }

        private class AnimalGraceStaminaTimer : Timer
        {
            private Mobile m_Caster;

            public AnimalGraceStaminaTimer(Mobile caster) : base(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5))
            {
                m_Caster = caster;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Caster == null || m_Caster.Deleted)
                {
                    Stop();
                    return;
                }

                m_Caster.Stam += 1; // Increase stamina by 1 every 5 seconds
            }
        }
    }
}
