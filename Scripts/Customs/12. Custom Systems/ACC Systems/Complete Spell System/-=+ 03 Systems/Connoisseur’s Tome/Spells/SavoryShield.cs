using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;
using Server.Commands;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class SavoryShield : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Savory Shield", "Savoryus Protectus",
            21008,
            9304
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public SavoryShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x375A, 10, 15, 5017, 37, 3, EffectLayer.Waist);
                Caster.PlaySound(0x1F7);

                int duration = (int)(Caster.Skills[CastSkill].Value / 2.0); // Duration based on skill level
                double resistanceBonus = 0.1 + (Caster.Skills[CastSkill].Value / 1000.0); // 10% + skill level dependent

                // Assuming you want to add some visual or message to show the buff was applied
                BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.MagicReflection, 1075815, duration));

                ApplyResistanceIncrease(resistanceBonus);

                Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                {
                    RemoveResistanceIncrease(resistanceBonus);
                    Caster.SendMessage("The effects of the Savory Shield have worn off.");
                    BuffInfo.RemoveBuff(Caster, BuffIcon.MagicReflection);
                });
            }

            FinishSequence();
        }

        private void ApplyResistanceIncrease(double bonus)
        {
            Caster.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, (int)(5 * bonus)));
            Caster.AddResistanceMod(new ResistanceMod(ResistanceType.Fire, (int)(5 * bonus)));
            Caster.AddResistanceMod(new ResistanceMod(ResistanceType.Cold, (int)(5 * bonus)));
            Caster.AddResistanceMod(new ResistanceMod(ResistanceType.Poison, (int)(5 * bonus)));
            Caster.AddResistanceMod(new ResistanceMod(ResistanceType.Energy, (int)(5 * bonus)));
        }

        private void RemoveResistanceIncrease(double bonus)
        {
            Caster.RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, (int)(5 * bonus)));
            Caster.RemoveResistanceMod(new ResistanceMod(ResistanceType.Fire, (int)(5 * bonus)));
            Caster.RemoveResistanceMod(new ResistanceMod(ResistanceType.Cold, (int)(5 * bonus)));
            Caster.RemoveResistanceMod(new ResistanceMod(ResistanceType.Poison, (int)(5 * bonus)));
            Caster.RemoveResistanceMod(new ResistanceMod(ResistanceType.Energy, (int)(5 * bonus)));
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }

    public class BuffInfo
    {
        private BuffIcon _icon;
        private int _text;
        private int _duration;

        public BuffInfo(BuffIcon icon, int text, int duration)
        {
            _icon = icon;
            _text = text;
            _duration = duration;
        }

        public static void AddBuff(Mobile m, BuffInfo buffInfo)
        {
            m.SendMessage("You feel protected by the savory shield.");
        }

        public static void RemoveBuff(Mobile m, BuffIcon buffIcon)
        {
            m.SendMessage("Your savory shield fades away.");
        }
    }

    public enum BuffIcon
    {
        MagicReflection = 1075815
    }
}
