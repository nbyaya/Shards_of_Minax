using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Misc;
using Server.Items;
using Server.SkillHandlers;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class DefensiveStance : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Defensive Stance", "Protectus Fortus",
                                                        //SpellCircle.Third,
                                                        21004,
                                                        9300,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 15; } }

        public DefensiveStance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You adopt a defensive stance, increasing your armor and physical resistance!");

                // Play some visual effects
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                Caster.PlaySound(0x1F2);

                // Apply buff effects
                Caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                Caster.PlaySound(0x1FC);

                // Increase armor and physical resistance
                int bonusArmor = 20; // Example value, adjust as necessary
                int bonusResist = 10; // Example value, adjust as necessary
                TimeSpan duration = TimeSpan.FromSeconds(10.0 + Caster.Skills[SkillName.Tactics].Value * 0.1);

                DefensiveStanceBuff buff = new DefensiveStanceBuff(Caster, bonusArmor, bonusResist, duration);
                buff.Start();

                // Finish casting sequence
                FinishSequence();
            }
        }

        private class DefensiveStanceBuff : Timer
        {
            private Mobile m_Caster;
            private int m_ArmorBonus;
            private int m_ResistBonus;
            private ResistanceMod m_ResistMod;

            public DefensiveStanceBuff(Mobile caster, int armorBonus, int resistBonus, TimeSpan duration) : base(duration)
            {
                m_Caster = caster;
                m_ArmorBonus = armorBonus;
                m_ResistBonus = resistBonus;
                m_ResistMod = new ResistanceMod(ResistanceType.Physical, resistBonus);

                Start();
            }

            protected override void OnTick()
            {
                // Remove buff effects
                m_Caster.SendMessage("Your defensive stance fades away.");
                m_Caster.FixedParticles(0x373A, 10, 15, 5037, EffectLayer.Waist);
                m_Caster.PlaySound(0x1FC);

                // Remove bonuses
                m_Caster.VirtualArmorMod -= m_ArmorBonus;
                m_Caster.RemoveResistanceMod(m_ResistMod);

                Stop();
            }

            public void Start()
            {
                m_Caster.VirtualArmorMod += m_ArmorBonus;
                m_Caster.AddResistanceMod(m_ResistMod);
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
