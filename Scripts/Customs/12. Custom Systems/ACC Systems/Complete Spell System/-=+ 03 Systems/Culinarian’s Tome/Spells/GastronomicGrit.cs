using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class GastronomicGrit : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Gastronomic Grit", "Summon a robust dish",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 30; } }

        public GastronomicGrit(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Summon the consumable item
                RobustDish dish = new RobustDish();
                dish.MoveToWorld(Caster.Location, Caster.Map);
                Caster.SendMessage("You summon a robust dish!");
                
                // Play flashy effects
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F7); // Sound effect
                Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x373A, 30, 10, 1153, 0); // Visual effect
            }

            FinishSequence();
        }
    }

    public class RobustDish : Item
    {
        public override string DefaultName { get { return "A Robust Dish"; } }

        [Constructable]
        public RobustDish() : base(0x09AF) // Assuming 0x9C7 is the item ID for the dish
        {
            Weight = 1.0;
            Hue = 1153; // Color of the dish
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable) return;

            // Apply the dexterity bonus
            if (from is PlayerMobile player)
            {
                player.SendMessage("You consume the robust dish, feeling a surge of agility!");

                // Play flashy effects upon consumption
                Effects.PlaySound(player.Location, player.Map, 0x206); // Consumption sound effect
                player.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);

                // Apply the dexterity bonus
                StatMod dexBonus = new StatMod(StatType.Dex, "GastronomicGritDexBonus", 10, TimeSpan.FromHours(1));
                player.AddStatMod(dexBonus);

                Delete(); // Remove the item after use
            }
        }

        public RobustDish(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
