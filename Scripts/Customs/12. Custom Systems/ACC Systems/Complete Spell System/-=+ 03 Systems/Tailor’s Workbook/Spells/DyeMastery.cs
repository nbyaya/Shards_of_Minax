using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class DyeMastery : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Dye Mastery", "Coloris Maximus",
            // SpellCircle.Fourth,
            21009,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public DyeMastery(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private DyeMastery m_Owner;

            public InternalTarget(DyeMastery owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Item item)
                {
                    m_Owner.ApplyDyeEffect(item);
                }
                else
                {
                    from.SendMessage("You can only dye items or clothing.");
                    m_Owner.FinishSequence();
                }
            }
        }

        public void ApplyDyeEffect(Item item)
        {
            if (!Caster.CanSee(item))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (CheckSequence())
            {
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F3); // Sound of magical dyeing
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x373A, 1, 15, 1153, 2, 9962, 0); // Flashy effect

                // Apply a random hue to the item
                item.Hue = Utility.RandomDyedHue();

                Caster.SendMessage("You have successfully changed the color and appearance of the item!");

                // Optionally, you could add more logic here to change the item's appearance or properties
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
