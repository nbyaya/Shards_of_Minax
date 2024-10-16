using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class TerrainMastery : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Terrain Mastery", "Rel In Xan",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 50; } }

        public TerrainMastery(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel the terrain empowering your agility!");

                Effects.PlaySound(Caster.Location, Caster.Map, 0x5C7); // Sound effect for spell activation
                Caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Visual effect for spell activation

                BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.Agility, 1075845, TimeSpan.FromSeconds(15), Caster)); // Add buff icon for visual indication

                double dexterityBonus = Caster.Dex * 0.5; // Calculate 50% dexterity bonus

                // Adjust dexterity directly (this is a placeholder, modify according to your implementation)
                Caster.Dex += (int)dexterityBonus; // Apply dexterity bonus

                Timer.DelayCall(TimeSpan.FromMinutes(15), () =>
                {
                    Caster.SendMessage("The effect of Terrain Mastery fades away.");
                    Caster.Dex -= (int)dexterityBonus; // Remove dexterity bonus
                    BuffInfo.RemoveBuff(Caster, BuffIcon.Agility); // Remove buff icon
                });

                StartCooldown(); // Start cooldown timer
            }

            FinishSequence();
        }

        private void StartCooldown()
        {
            CooldownTimer cooldownTimer = new CooldownTimer(Caster, TimeSpan.FromMinutes(30));
            cooldownTimer.Start();
        }

        private class CooldownTimer : Timer
        {
            private Mobile m_Caster;

            public CooldownTimer(Mobile caster, TimeSpan duration) : base(duration)
            {
                m_Caster = caster;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                m_Caster.SendMessage("You are now able to use Terrain Mastery again.");
                Stop();
            }
        }
    }
}
