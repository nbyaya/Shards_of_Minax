using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class BardsBrew : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Bard's Brew", "To me my brew!",
            //SpellCircle.First,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 30; } }

        public BardsBrew(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    // Create the consumable item
                    BardsBrewItem brew = new BardsBrewItem();
                    Caster.AddToBackpack(brew);

                    // Play visual and sound effects
                    Caster.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Waist);
                    Caster.PlaySound(0x1E9);

                    Caster.SendMessage("You summon a soothing brew that calms the nerves and sharpens the mind.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }

    public class BardsBrewItem : Item
    {
        private Timer m_Timer;

        [Constructable]
        public BardsBrewItem() : base(0x99B) // Use appropriate item ID for the brew
        {
            Name = "Bard's Brew";
            Hue = 0x481; // A soothing color for the brew
            Movable = true;

            m_Timer = new InternalTimer(this);
            m_Timer.Start();
        }

        public BardsBrewItem(Serial serial) : base(serial)
        {
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

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            // Apply the musicianship bonus
            from.SendMessage("You drink the soothing brew and feel your musical talents sharpen.");
            from.PlaySound(0x30); // Sound effect for drinking
            from.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Waist);

            from.Skills[SkillName.Musicianship].Base += 20; // Increase musicianship skill

            // Timer for removing the effect
            Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
            {
                if (from != null && !from.Deleted)
                {
                    from.Skills[SkillName.Musicianship].Base -= 20; // Remove the bonus
                    from.SendMessage("The effects of the brew wear off.");
                }
            });

            Delete(); // Remove the item after use
        }

        private class InternalTimer : Timer
        {
            private Item m_Item;

            public InternalTimer(Item item) : base(TimeSpan.FromMinutes(10.0)) // Duration before the item expires
            {
                m_Item = item;
            }

            protected override void OnTick()
            {
                m_Item.Delete();
            }
        }
    }
}
