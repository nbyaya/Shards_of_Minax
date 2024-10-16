using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class ElixirOfAgility : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Elixir of Agility", "Fasci Agilitas",
            // SpellCircle.Fourth,
            266,
            9040
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public ElixirOfAgility(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Visual and Sound Effects
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F4); // A bubbling potion sound, 0); // Green sparkles

                // Apply Effects
                Caster.SendMessage("You feel a surge of agility coursing through your veins!");
                Caster.AddToBackpack(new ElixirOfAgilityPotion()); // Add a potion to inventory for visual feedback

                // Increase Dexterity and Attack Speed temporarily
                Timer.DelayCall(TimeSpan.FromSeconds(5.0), () => 
                {
                    if (Caster != null && !Caster.Deleted)
                    {
                        Caster.Dex += 10; // Increase Dexterity
                        Caster.SendMessage("You feel your dexterity improve!");
                    }
                });
                
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => 
                {
                    if (Caster != null && !Caster.Deleted)
                    {
                        Caster.Dex -= 10; // Revert Dexterity
                        Caster.SendMessage("The effects of the Elixir of Agility wear off.");
                    }
                });
            }

            FinishSequence();
        }
    }

    public class ElixirOfAgilityPotion : Item
    {
        [Constructable]
        public ElixirOfAgilityPotion() : base(0xF0E) // Potion bottle item ID
        {
            Name = "Elixir of Agility";
            Hue = 0x58; // Green color
            Weight = 1.0;
        }

        public ElixirOfAgilityPotion(Serial serial) : base(serial)
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
    }
}
