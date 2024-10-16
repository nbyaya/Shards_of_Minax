using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class MysticCatch : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Mystic Catch", "Mystic Fish!",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 15; } }

        public MysticCatch(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    // Visual and sound effects
                    Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                    Caster.PlaySound(0x1F5);

                    // Chance to catch a mystical fish
                    if (Utility.RandomDouble() < 0.25) // 25% chance
                    {
                        // Create the mystical fish
                        Item mysticalFish = new MysticFish();
                        Caster.AddToBackpack(mysticalFish);

                        Caster.SendMessage("You have caught a mystical fish!");

                        // Apply a temporary buff or unique bonus
                        BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.Bless, 1075643, 1075644, TimeSpan.FromMinutes(5), Caster));
                    }
                    else
                    {
                        Caster.SendMessage("You failed to catch a mystical fish.");
                    }
                }
                catch
                {
                    Caster.SendMessage("An error occurred while casting Mystic Catch.");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }

    public class MysticFish : Item
    {
        [Constructable]
        public MysticFish() : base(0x3B0C)
        {
            Name = "Mystical Fish";
            Hue = 1153;
        }

        public MysticFish(Serial serial) : base(serial)
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
