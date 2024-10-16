using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class TastefulCamouflage : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tasteful Camouflage", "In Connuio Vis",
            //SpellCircle.Second,
            21019,
            9315
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 12; } }

        public TastefulCamouflage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster.Hidden)
            {
                Caster.SendMessage("You are already hidden.");
                FinishSequence();
                return;
            }

            if (CheckSequence())
            {
                Caster.Hidden = true; // Make the caster hidden

                // Play visual effect at the caster's location
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 1153, 7, 9912, 0);
                // Play sound effect
                Caster.PlaySound(0x482);

                Timer.DelayCall(TimeSpan.FromSeconds(0.5), delegate // Added delay for additional effects
                {
                    // Additional smoke effect for a flashy hide
                    Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x375A, 9, 32, 5008);
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
