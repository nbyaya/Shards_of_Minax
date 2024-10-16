using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class InvisibilityCloak : StealingSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Invisibility Cloak", "An Lar Xen",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 10; } }

        public InvisibilityCloak(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Hidden = true;
                Caster.SendMessage("You are now invisible!");
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                Caster.PlaySound(0x56D);

                Timer.DelayCall(TimeSpan.FromSeconds(20.0), new TimerStateCallback(EndInvisibility), Caster);
            }

            FinishSequence();
        }

        private void EndInvisibility(object state)
        {
            Mobile caster = state as Mobile;

            if (caster != null && caster.Hidden)
            {
                caster.Hidden = false;
                caster.SendMessage("You are no longer invisible.");
                Effects.SendLocationParticles(EffectItem.Create(caster.Location, caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                caster.PlaySound(0x56E);
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }

        // This method handles the invisibility effect when the caster takes damage
        public void HandleDamage(Mobile attacker, int amount)
        {
            if (Caster.Hidden && attacker == Caster)
            {
                Caster.Hidden = false;
                Caster.SendMessage("Your invisibility fades as you attack.");
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                Caster.PlaySound(0x56E);
            }
        }
    }
}
