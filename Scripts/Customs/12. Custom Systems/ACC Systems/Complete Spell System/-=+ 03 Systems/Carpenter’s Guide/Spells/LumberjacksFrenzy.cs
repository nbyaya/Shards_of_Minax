using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class LumberjacksFrenzy : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Lumberjack's Frenzy", "HERES JOHNNEY!",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public LumberjacksFrenzy(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x52); // Play a sound to signify activation
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Visual effect around the caster

                int bonusStrength = (int)(Caster.Str * 0.5); // 50% Strength boost
                int bonusDexterity = (int)(Caster.Dex * 0.5); // 50% Dexterity boost

                Caster.SendMessage("You feel a surge of strength and agility!");

                // Apply temporary boosts
                BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.Strength, 1075846, 1151467, TimeSpan.FromSeconds(30), Caster));
                BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.Agility, 1075846, 1151467, TimeSpan.FromSeconds(30), Caster));

                Timer.DelayCall(TimeSpan.FromSeconds(30.0), delegate
                {
                    Caster.Str -= bonusStrength;
                    Caster.Dex -= bonusDexterity;
                    Caster.SendMessage("The effects of Lumberjack's Frenzy fade away.");
                });

                // Apply the temporary stat boosts
                Caster.Str += bonusStrength;
                Caster.Dex += bonusDexterity;
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
