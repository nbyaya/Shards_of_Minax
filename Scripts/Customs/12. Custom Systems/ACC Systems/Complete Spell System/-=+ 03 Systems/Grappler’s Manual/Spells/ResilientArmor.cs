using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class ResilientArmor : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Resilient Armor", "Fortis Arma",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 15; } }

        private static readonly TimeSpan BuffDuration = TimeSpan.FromSeconds(15.0);
        private const double ArmorBonus = 0.20; // 20% Armor bonus

        public ResilientArmor(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel a surge of resilience as your armor strengthens!");
                Caster.PlaySound(0x1F2); // Sound effect for casting
                Caster.FixedParticles(0x375A, 1, 15, 5017, 1153, 2, EffectLayer.Waist); // Visual effect

                int bonus = (int)(Caster.PhysicalResistance * ArmorBonus);

                // Create and add the resistance modifier
                ResistanceMod mod = new ResistanceMod(ResistanceType.Physical, bonus);
                Caster.AddResistanceMod(mod);

                Timer.DelayCall(BuffDuration, () =>
                {
                    Caster.RemoveResistanceMod(mod);
                    Caster.SendMessage("The resilient armor effect fades away.");
                    Caster.PlaySound(0x1F8); // Sound effect for buff ending
                    Caster.FixedParticles(0x373A, 1, 15, 5017, 1150, 2, EffectLayer.Waist); // Visual effect for buff ending
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0); // Adjusted cast delay for balance
        }
    }
}
