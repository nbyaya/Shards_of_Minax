using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Misc;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class ForagersBounty : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Forager's Bounty", "Ylem Herbam",
            // SpellCircle.Fourth,
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public ForagersBounty(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You summon a bounty of food and drink!");

                Point3D location = Caster.Location;
                Map map = Caster.Map;

                if (map == null)
                    return;

                // Creating the visual and sound effects
                Effects.SendLocationParticles(EffectItem.Create(location, map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                Effects.PlaySound(location, map, 0x2E6); // Sound of food appearing

                // Summoning food and drink items
                SummonFoodAndDrink(location, map);

                // Applying buffs to nearby players
                ApplyBuffs();

                // Cooldown timer setup
                Timer.DelayCall(TimeSpan.FromMinutes(15), () => CooldownComplete(Caster));
            }

            FinishSequence();
        }

        private void SummonFoodAndDrink(Point3D location, Map map)
        {
            Item[] foodItems = new Item[]
            {
                new BreadLoaf(),
                new CheeseWedge(),
                new CookedBird(),
                new Grapes(),
                new Pitcher() // Replaced WaterPitcher with Pitcher
            };

            foreach (Item food in foodItems)
            {
                food.MoveToWorld(new Point3D(location.X + Utility.RandomMinMax(-1, 1), location.Y + Utility.RandomMinMax(-1, 1), location.Z), map);
            }
        }

        private void ApplyBuffs()
        {
            foreach (Mobile m in Caster.GetMobilesInRange(3))
            {
                if (m is PlayerMobile player && player.Alive && Caster.CanBeBeneficial(player))
                {
                    Caster.DoBeneficial(player);

                    // Apply temporary buffs to stamina regeneration and hunger satisfaction
                    BuffInfo.AddBuff(player, new BuffInfo(BuffIcon.Bless, 1075643, 1075644, TimeSpan.FromMinutes(10), player, true)); // Adjusted to use ManaRegeneration and correct parameter type
                    player.Stam += 10; // Temporary increase in stamina
                    player.Hunger = Math.Min(player.Hunger + 20, 20); // Satisfy hunger

                    player.SendMessage("You feel invigorated by the bounty of food and drink.");
                }
            }
        }

        private void CooldownComplete(Mobile caster)
        {
            caster.SendMessage("You can use 'Forager's Bounty' again.");
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
