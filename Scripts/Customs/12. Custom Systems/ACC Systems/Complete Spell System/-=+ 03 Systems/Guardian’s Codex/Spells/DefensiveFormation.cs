using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class DefensiveFormation : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Defensive Formation", "Defendo Formato",
            21005,
            9301,
            false,
            Reagent.BlackPearl,
            Reagent.Garlic
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public DefensiveFormation(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Waist);
                Caster.PlaySound(0x20E);

                int creatureCount = 0;

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m is BaseCreature && m.Alive && !m.IsDeadBondedPet)
                    {
                        creatureCount++;
                    }
                }

                double armorBonus = creatureCount * 2; // Each creature grants 2 armor points
                armorBonus = Math.Min(armorBonus, 30); // Cap the bonus at 30 armor points

                Caster.VirtualArmorMod += (int)armorBonus;
                Caster.SendMessage("You feel a surge of defensive power as creatures gather around you!");

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                {
                    Caster.VirtualArmorMod -= (int)armorBonus;
                    Caster.SendMessage("The defensive formation fades away.");
                });

                Caster.FixedParticles(0x376A, 10, 30, 5052, EffectLayer.Waist);
                Caster.PlaySound(0x1F8);
            }

            FinishSequence();
        }
    }
}
