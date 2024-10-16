using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class DisguiseSelf : StealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disguise Self", "In Quas Xen",
            21004, // Spell effect (GFX)
            9300  // Sound effect
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 10; } }

        public DisguiseSelf(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private DisguiseSelf m_Owner;

            public InternalTarget(DisguiseSelf owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && target != from)
                {
                    m_Owner.ApplyDisguise(target);
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target cannot be seen or is invalid.
                }
            }
        }

        public void ApplyDisguise(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (CheckSequence())
            {
                // Visual and Sound Effects
                Effects.SendTargetParticles(Caster, 0x376A, 10, 15, 5010, EffectLayer.Waist);
                Effects.PlaySound(Caster.Location, Caster.Map, 0x213);

                // Disguise Logic
                Caster.BodyMod = target.Body;
                Caster.HueMod = target.Hue;
                Caster.NameMod = "Disguised " + target.Name;

                // Adding a timer to revert back after some time
                Timer.DelayCall(TimeSpan.FromMinutes(2.0), () => RemoveDisguise(Caster));

                Caster.SendMessage("You have disguised yourself as " + target.Name + ".");
            }

            FinishSequence();
        }

        public void RemoveDisguise(Mobile caster)
        {
            caster.BodyMod = 0;
            caster.HueMod = -1;
            caster.NameMod = null;
            caster.SendMessage("Your disguise has worn off.");
            
            // Revert to original appearance sound and visual
            Effects.SendTargetParticles(caster, 0x373A, 10, 15, 5010, EffectLayer.Waist);
            Effects.PlaySound(caster.Location, caster.Map, 0x214);
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
