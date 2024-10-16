using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class Wraithform : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Wraithform", "Kal Xen Corp",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Seventh; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        private int originalVirtualArmor;
        private int virtualArmorModifier = 20;
        private double stamModifier = 20;

        public Wraithform(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!Caster.CanBeginAction(typeof(Wraithform)))
            {
                Caster.SendMessage("You are already in wraithform.");
                return;
            }

            if (CheckSequence())
            {
                Caster.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                Caster.PlaySound(0x1FB);

                BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.WraithForm, 1044124, 1070722, TimeSpan.FromMinutes(2), Caster));

                Caster.SendMessage("You have transformed into a wraith!");

                Caster.BodyMod = 0x3CA; // Wraith body
                Caster.HueMod = 0x4001; // Ethereal hue
                Caster.Hidden = true; // Partially ethereal

                // Save the original virtual armor
                originalVirtualArmor = Caster.VirtualArmorMod;

                // Apply the virtual armor modifier
                Caster.VirtualArmorMod += virtualArmorModifier;

                // Apply the stamina modifier
                Caster.Stam += (int)stamModifier; 

                Timer.DelayCall(TimeSpan.FromMinutes(2), new TimerStateCallback(EndWraithform_Callback), Caster);
            }

            FinishSequence();
        }

        private void EndWraithform_Callback(object state)
        {
            Mobile caster = (Mobile)state;

            if (caster == null || caster.Deleted)
                return;

            caster.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
            caster.PlaySound(0x1FB);

            caster.SendMessage("You return to your normal form.");

            caster.BodyMod = 0;
            caster.HueMod = -1;
            caster.Hidden = false;

            // Revert the virtual armor modifier
            caster.VirtualArmorMod = originalVirtualArmor;

            // Revert the stamina modifier
            caster.Stam -= (int)stamModifier;

            BuffInfo.RemoveBuff(caster, BuffIcon.WraithForm);
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
