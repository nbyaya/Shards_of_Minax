using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class DeflectiveStance : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Deflective Stance", "Sanctum Flectere",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 10; } }

        public DeflectiveStance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You assume a deflective stance, enhancing your parry skill!");
                
                // Visual and Sound Effects
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008); // Flashy effect
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F7); // Sound effect

                // Increase Parry skill temporarily
                Caster.Skills[SkillName.Parry].Base += 30;
                Timer.DelayCall(TimeSpan.FromSeconds(30), () => Caster.Skills[SkillName.Parry].Base -= 30);

                // Decrease Dexterity temporarily
                int dexReduction = (int)(Caster.Dex * 0.15);
                Caster.Dex -= dexReduction;
                Timer.DelayCall(TimeSpan.FromSeconds(30), () => Caster.Dex += dexReduction);

                Caster.FixedParticles(0x373A, 1, 15, 9502, 0x1F7, 3, EffectLayer.Waist); // Additional visual effect
            }

            FinishSequence();
        }
    }
}
