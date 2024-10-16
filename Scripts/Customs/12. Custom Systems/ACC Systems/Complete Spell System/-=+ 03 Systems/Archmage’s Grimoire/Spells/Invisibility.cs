using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class Invisibility : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Invisibility", "An Lor Xen",
            21004,
            9300,
            false,
            Reagent.Nightshade
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 30; } }

        public Invisibility(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile caster = Caster;

                // Play a flash and sound effect
                caster.PlaySound(0x58D);
                Effects.SendLocationParticles(EffectItem.Create(caster.Location, caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);

                // Apply invisibility effect
                caster.Hidden = true;

                // Boost Hiding and Stealth temporarily
                caster.SendMessage("You feel incredibly stealthy!");
                caster.CheckSkill(SkillName.Hiding, 0, 100);
                caster.CheckSkill(SkillName.Stealth, 0, 100);

                caster.AddStatMod(new StatMod(StatType.Dex, "InvisibilityDexMod", 20, TimeSpan.FromSeconds(30.0)));
                caster.AddSkillMod(new TimedSkillMod(SkillName.Hiding, true, 50.0, TimeSpan.FromSeconds(30.0)));
                caster.AddSkillMod(new TimedSkillMod(SkillName.Stealth, true, 50.0, TimeSpan.FromSeconds(30.0)));

                Timer.DelayCall(TimeSpan.FromSeconds(30.0), new TimerStateCallback(RemoveInvisibility), caster);
            }

            FinishSequence();
        }

        private void RemoveInvisibility(object state)
        {
            Mobile caster = (Mobile)state;

            if (caster.Hidden)
            {
                caster.Hidden = false;
                caster.SendMessage("The invisibility effect wears off, and you become visible.");
                caster.PlaySound(0x1FD);
                Effects.SendLocationParticles(EffectItem.Create(caster.Location, caster.Map, EffectItem.DefaultDuration), 0x375A, 9, 20, 5028);
            }
        }
    }
}
