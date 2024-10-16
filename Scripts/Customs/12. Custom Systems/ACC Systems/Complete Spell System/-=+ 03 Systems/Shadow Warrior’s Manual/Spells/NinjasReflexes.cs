using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class NinjasReflexes : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ninja's Reflexes", "Swift As Shadow",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public NinjasReflexes(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("Your reflexes sharpen as you move like a shadow!");

                // Apply effects: Increase movement speed and dodge chance
                Caster.AddStatMod(new StatMod(StatType.Dex, "NinjaDexBoost", 20, TimeSpan.FromSeconds(10)));
                Caster.VirtualArmorMod += 10; // Increase dodge chance by increasing VirtualArmor

                // Play visual and sound effects
                Effects.SendTargetParticles(Caster, 0x3728, 10, 15, 5013, EffectLayer.Waist);
                Caster.PlaySound(0x4E9);

                // Add a timer to revert the changes after the duration ends
                Timer.DelayCall(TimeSpan.FromSeconds(10), () => RevertEffects(Caster));
            }

            FinishSequence();
        }

        private void RevertEffects(Mobile caster)
        {
            caster.RemoveStatMod("NinjaDexBoost");
            caster.VirtualArmorMod -= 10;
            caster.SendMessage("Your reflexes return to normal.");
            Effects.SendTargetParticles(caster, 0x373A, 10, 15, 5013, EffectLayer.Waist);
        }
    }
}
