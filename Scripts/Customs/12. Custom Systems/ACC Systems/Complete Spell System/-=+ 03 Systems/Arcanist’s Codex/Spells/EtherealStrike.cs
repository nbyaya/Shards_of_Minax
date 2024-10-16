using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using Server.Regions;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class EtherealStrike : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ethereal Strike", "An Ethereal Force!",
            21005,
            9407
        );
		
        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public EtherealStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
            else
            {
                FinishSequence(); // Use instance method
            }
        }

        private class InternalTarget : Target
        {
            private EtherealStrike m_Owner;

            public InternalTarget(EtherealStrike owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile)
                {
                    Mobile targetMobile = (Mobile)target;

                    // Play a visual effect
                    Effects.PlaySound(targetMobile, targetMobile.Map, 0x1F5); // Ghostly sound

                    // Show the ethereal strike effect
                    Effects.SendLocationEffect(targetMobile.Location, targetMobile.Map, 0x36D4, 10, 10, 0x96, 0); // Spectral visual effect

                    // Calculate damage
                    int damage = Utility.RandomMinMax(15, 30); // Adjust the damage range as needed
                    targetMobile.Damage(damage, from);

                    from.SendMessage("You strike with a spectral force, bypassing physical defenses!");
                }
                else
                {
                    from.SendMessage("You cannot strike that target.");
                }

                m_Owner.FinishSequence(); // Use instance method
            }
        }
    }
}
