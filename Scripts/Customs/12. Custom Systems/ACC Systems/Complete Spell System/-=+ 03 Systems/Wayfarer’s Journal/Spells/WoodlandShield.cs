using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;
using Server.Items;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class WoodlandShield : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Woodland Shield", "Terra Arbor",
            21005,
            9301,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; } // Arbitrary choice; adjust as needed
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public WoodlandShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                Caster.PlaySound(0x1F6);

                Caster.SendMessage("You are enveloped in a protective bark skin!");

                // Apply the bark skin effect
                Caster.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, 20));
                Caster.BeginAction(typeof(WoodlandShield));

                Timer.DelayCall(TimeSpan.FromSeconds(30.0), new TimerStateCallback(EndEffect), Caster);

                // Create a visual effect around the caster
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
            }

            FinishSequence();
        }

        private void EndEffect(object state)
        {
            Mobile caster = (Mobile)state;

            if (caster != null)
            {
                caster.EndAction(typeof(WoodlandShield));
                caster.SendMessage("The bark skin fades away.");
                caster.PlaySound(0x1F8);

                // Remove the bark skin effect
                caster.RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, 20));

                // Cooldown implementation (if required)
                Timer.DelayCall(TimeSpan.FromMinutes(5.0), new TimerStateCallback(ResetCooldown), caster);
            }
        }

        private void ResetCooldown(object state)
        {
            Mobile caster = (Mobile)state;
            if (caster != null)
            {
                caster.SendMessage("You can use Woodland Shield again.");
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
