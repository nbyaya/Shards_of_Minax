using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class Disarm : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disarm", "Uus Jux",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public Disarm(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Disarm m_Owner;

            public InternalTarget(Disarm owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (!from.CanBeHarmful(target))
                    {
                        from.SendLocalizedMessage(1001010); // You cannot harm that.
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        // Effect: Disarm target
                        Item weapon = target.FindItemOnLayer(Layer.OneHanded) ?? target.FindItemOnLayer(Layer.TwoHanded);

                        if (weapon != null)
                        {
                            target.Backpack?.DropItem(weapon);

                            // Play disarm animation and sound
                            target.FixedParticles(0x37B9, 10, 15, 5032, EffectLayer.Waist);
                            target.PlaySound(0x208);

                            // Add a timer to re-equip the weapon after a short duration
                            Timer.DelayCall(TimeSpan.FromSeconds(5.0), () => ReEquipWeapon(target, weapon));

                            // Cool visual and sound effects when casting
                            Effects.SendLocationEffect(from.Location, from.Map, 0x36BD, 20, 10, 0, 0);
                            from.PlaySound(0x204);
                        }
                        else
                        {
                            from.SendMessage("Your target has no weapon to disarm!");
                        }
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1001010); // You cannot harm that.
                }

                m_Owner.FinishSequence();
            }

            private void ReEquipWeapon(Mobile target, Item weapon)
            {
                if (target.Alive && target.Backpack != null && weapon != null)
                {
                    target.EquipItem(weapon);
                    target.SendMessage("You have re-equipped your weapon.");
                }
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
