using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class WhisperingWinds : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Whispering Winds", "Signolo!",
                                                        21016,
                                                        9215
                                                       );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
		public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 15; } }

        public WhisperingWinds(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (target == Caster)
            {
                Caster.SendLocalizedMessage(501857); // You cannot cast this on yourself.
            }
            else if (CheckSequence())
            {
                // Play visual and sound effects for the casting
                Caster.PlaySound(0x5C3); // Mysterious wind sound
                Caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Swirling wind effect

                // Send a private message to the target
                target.SendMessage(0x35, $"{Caster.Name} whispers: 'Stay alert, danger may be near.'");

                // Make the caster remain hidden if they were hidden before
                if (Caster.Hidden)
                {
                    Caster.Hidden = true;
                    Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x375A, 1, 13, 1153, 3, 9917, 0);
                    Caster.PlaySound(0x1FD); // Soft whisper sound
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private WhisperingWinds m_Owner;

            public InternalTarget(WhisperingWinds owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
