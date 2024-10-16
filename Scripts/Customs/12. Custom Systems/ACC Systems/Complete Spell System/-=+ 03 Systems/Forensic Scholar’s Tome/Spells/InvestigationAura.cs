using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.SkillHandlers;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class InvestigationAura : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Investigation Aura", "In Xen Wis",
            // SpellCircle.First,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        private static readonly TimeSpan Duration = TimeSpan.FromSeconds(30.0);

        public InvestigationAura(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x1E7); // Sound effect for casting
                Caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist); // Visual effect of an aura around caster

                BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.Bless, 1075643, Duration, Caster)); // Buff icon display

                Caster.SendMessage("You feel an aura of investigation surrounding you, enhancing your ability to detect hidden objects.");

                Caster.Skills[SkillName.DetectHidden].Base += 25; // Increase Detect Hidden skill by +25

                Timer.DelayCall(Duration, () =>
                {
                    Caster.Skills[SkillName.DetectHidden].Base -= 25; // Revert skill change after duration
                    Caster.SendMessage("The aura of investigation fades away.");
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
