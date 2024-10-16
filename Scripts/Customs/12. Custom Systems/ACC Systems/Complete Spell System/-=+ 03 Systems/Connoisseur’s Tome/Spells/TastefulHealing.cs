using System;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class TastefulHealing : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tasteful Healing", "Maximus Cheesu",
            //SpellCircle.First,
            21016,
            9312
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 18; } }

        public TastefulHealing(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You conjure a delicious piece of healing cheese.");
                Caster.PlaySound(0x4B9); // Sound for a magical food creation
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x375A, 9, 32, 5008); // Flashy particle effect

                HealingCheese cheese = new HealingCheese();
                cheese.MoveToWorld(Caster.Location, Caster.Map);
            }

            FinishSequence();
        }
    }

    public class HealingCheese : Item
    {
        [Constructable]
        public HealingCheese() : base(0x97E) // Cheese wheel item ID
        {
            Name = "Healing Cheese";
            Hue = 0x481; // A unique color to distinguish it
            Weight = 1.0;
        }

        public HealingCheese(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 2))
            {
                from.PlaySound(0x4B9); // Sound when eating
                from.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist); // Sparkly effect
                from.Heal(Utility.RandomMinMax(20, 40)); // Heal for a random amount between 20 and 40 HP

                from.SendMessage("You feel rejuvenated after eating the healing cheese!");
                this.Delete(); // Consume the cheese after use
            }
            else
            {
                from.SendMessage("You are too far away to eat the cheese.");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
