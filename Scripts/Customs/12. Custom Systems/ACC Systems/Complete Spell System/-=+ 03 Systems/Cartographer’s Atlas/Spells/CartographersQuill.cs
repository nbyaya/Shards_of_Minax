using System;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class CartographersQuill : CartographySpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Cartographer’s Quill", "Explore the Unknown",
            21004,
            9300
        );

        public override SpellCircle Circle => SpellCircle.Third;

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 40.0;
        public override int RequiredMana => 30;

        public CartographersQuill(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                double randomValue = Utility.RandomDouble();

                if (randomValue <= 0.4) // 40% chance
                {
                    Caster.AddToBackpack(new TreasureMap1Scroll());
                    Caster.SendMessage("You have discovered a basic treasure map!");
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x5AC); // Sound effect
                    Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x376A, 10, 1, 1153, 4); // Flashy effect
                }
                else if (randomValue <= 0.5) // 10% chance
                {
                    Item[] rareMaps = { new TreasureMap2Scroll(), new TreasureMap3Scroll(), new TreasureMap4Scroll(), new TreasureMap5Scroll() };
                    Item rareMap = rareMaps[Utility.Random(rareMaps.Length)];
                    Caster.AddToBackpack(rareMap);
                    Caster.SendMessage("You have discovered a rare treasure map!");
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x5B3); // Different sound effect
                    Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x376A, 10, 1, 1160, 4); // Different flashy effect
                }
                else // 50% chance
                {
                    Caster.SendMessage("You found nothing this time.");
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x5B0); // Fail sound effect
                }
            }

            FinishSequence();
        }

    }
}
