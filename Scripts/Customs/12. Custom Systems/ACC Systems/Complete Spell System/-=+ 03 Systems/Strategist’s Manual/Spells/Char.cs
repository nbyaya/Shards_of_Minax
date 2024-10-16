using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class Char : TacticsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Charge", "Charge!",
            21004, // Graphic for the spell (customize as needed)
            9300   // Sound effect for the spell (customize as needed)
        );

        public override SpellCircle Circle => SpellCircle.First; // Define the spell circle if applicable
        public override double CastDelay => 0.5; // Quick cast time
        public override double RequiredSkill => 30.0; // Minimum skill required
        public override int RequiredMana => 30; // Mana cost

        public Char(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Char m_Owner;

            public InternalTarget(Char owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && m_Owner.CheckSequence())
                {
                    if (from.CanBeHarmful(target))
                    {
                        from.DoHarmful(target);

                        // Charge effect
                        from.MoveToWorld(target.Location, target.Map); // Move to target's location
                        Effects.PlaySound(from.Location, from.Map, 9300); // Play charge sound
                        Effects.SendLocationEffect(target.Location, target.Map, 0x10E, 30, 10, 0x481, 0); // Play visual effect

                        // Damage and knockback logic
                        int damage = Utility.RandomMinMax(15, 25); // Example damage range
                        AOS.Damage(target, from, damage, 100, 0, 0, 0, 0); // Deal damage to the target

                        // Knockback effect
                        Point3D pushLocation = GetKnockbackLocation(from, target);
                        target.MoveToWorld(pushLocation, target.Map);

                        // Apply stun or slow effect (optional, to enhance the ability's impact)
                        target.Freeze(TimeSpan.FromSeconds(1.5)); // Freeze target for a short duration

                        m_Owner.FinishSequence();
                    }
                    else
                    {
                        from.SendMessage("You can't see your target.");
                    }
                }
                else
                {
                    from.SendMessage("You must target a valid enemy.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }

            private Point3D GetKnockbackLocation(Mobile caster, Mobile target)
            {
                // Calculate knockback location based on direction from caster to target
                Direction d = target.GetDirectionTo(caster);

                // Calculate offset based on direction
                int offsetX = 0;
                int offsetY = 0;

                switch (d)
                {
                    case Direction.North:
                        offsetY = -1;
                        break;

                    case Direction.East:
                        offsetX = 1;
                        break;

                    case Direction.South:
                        offsetY = 1;
                        break;

                    case Direction.West:
                        offsetX = -1;
                        break;

                }

                return new Point3D(target.X + offsetX, target.Y + offsetY, target.Z);
            }
        }
    }
}
