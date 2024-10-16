using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class GourmetCrafting : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Gourmet Crafting", "Produci Item Di Delizia!",
            // SpellCircle.First, // Customize the spell circle if needed
            21015,
            9311
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 12; } }

        public GourmetCrafting(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        private static Type[] m_Types = new Type[]
        {
            typeof(RandomFancyBakedGoods),
            typeof(RandomFancyCheese),
            typeof(RandomFancyDinner)
        };

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    // Randomly select an item type from the list
                    Type itemType = m_Types[Utility.Random(m_Types.Length)];

                    // Create the item instance
                    Item gourmetItem = (Item)Activator.CreateInstance(itemType);

                    // Place the item in the caster's backpack
                    Caster.AddToBackpack(gourmetItem);

                    // Play a unique sound and display a visual effect to make the ability flashy
                    Caster.PlaySound(0x3E9); // Sound effect for crafting
                    Caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Visual effect

                    // Display a success message
                    Caster.SendMessage("You have crafted a gourmet item!");
                }
                catch
                {
                    Caster.SendMessage("Something went wrong with your crafting attempt.");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
