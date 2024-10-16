using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class ElixirOfEnlightenment : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Elixir of Enlightenment", "In Sancta Lumina",
            //SpellCircle.Fifth,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public ElixirOfEnlightenment(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Summon the elixir at the caster's location
                ElixirOfEnlightenmentPotion elixir = new ElixirOfEnlightenmentPotion();
                elixir.MoveToWorld(Caster.Location, Caster.Map);

                // Play a magical effect
                Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x376A, 10, 1, 1153, 4); // A glowing blue effect
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1E9); // A magical sound

                Caster.SendMessage("You conjure an Elixir of Enlightenment!");

                // Optionally consume the scroll
                if (Scroll != null)
                    Scroll.Consume();
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }

    public class ElixirOfEnlightenmentPotion : Item
    {
        [Constructable]
        public ElixirOfEnlightenmentPotion() : base(0xF0B) // Potion bottle graphic
        {
            Name = "Elixir of Enlightenment";
            Hue = 1266; // A distinctive hue for the potion
            Stackable = false;
            Weight = 1.0;
        }

        public ElixirOfEnlightenmentPotion(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendMessage("You drink the Elixir of Enlightenment, feeling your mind expand!");
            from.PlaySound(0x0F8); // Drinking sound
            from.FixedParticles(0x375A, 10, 15, 5010, EffectLayer.Waist); // Sparkle effect

            // Apply temporary intelligence bonus
            from.AddStatMod(new StatMod(StatType.Int, "ElixirIntBonus", 10, TimeSpan.FromMinutes(2.0)));

            this.Delete(); // Remove the potion after use
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
