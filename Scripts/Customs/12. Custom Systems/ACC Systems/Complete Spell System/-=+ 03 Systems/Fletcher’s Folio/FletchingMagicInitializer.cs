using System;
using Server;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class FletchingMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(EagleEye), "Eagle Eye", "Increases accuracy for a period, making arrows more likely to hit their target, especially against fast-moving enemies.", null, "Mana: 10", 21005, 9400, School.FletchersFolio);
            Register(typeof(ArrowRain), "Arrow Rain", "Fires a volley of arrows into an area, dealing damage to all enemies within the zone. The damage is spread out but can stack if multiple arrows hit the same target.", null, "Mana: 20", 21005, 9400, School.FletchersFolio);
            Register(typeof(PiercingShot), "Piercing Shot", "A powerful shot that pierces through multiple enemies in a line, dealing damage and applying a bleeding effect to all struck.", null, "Mana: 15", 21005, 9400, School.FletchersFolio);
            Register(typeof(CripplingShot), "Crippling Shot", "Shoots an arrow that slows the targets movement and attack speed, making it easier to evade or counterattack.", null, "Mana: 12", 21005, 9400, School.FletchersFolio);
            Register(typeof(ExplosiveArrow), "Explosive Arrow", "Fires an arrow tipped with a small explosive, dealing area-of-effect damage upon impact. Has a chance to ignite targets.", null, "Mana: 25", 21005, 9400, School.FletchersFolio);
            Register(typeof(SnipersMark), "Snipers Mark", "Marks a single target, increasing all damage dealt to that target by the archer and their allies for a short duration.", null, "Mana: 18", 21005, 9400, School.FletchersFolio);
            Register(typeof(Camouflage), "Camouflage", "The archer blends into their surroundings, becoming harder to detect and gaining a bonus to ranged attack damage from stealth.", null, "Mana: 15", 21005, 9400, School.FletchersFolio);
            Register(typeof(EntanglingArrow), "Entangling Arrow", "Fires an arrow that releases a net upon impact, entangling the target and preventing movement for a short duration.", null, "Mana: 20", 21005, 9400, School.FletchersFolio);
            Register(typeof(BarbedArrow), "Barbed Arrow", "An arrow with barbed tips that causes severe bleeding, inflicting damage over time and reducing the target's healing effectiveness.", null, "Mana: 16", 21005, 9400, School.FletchersFolio);
            Register(typeof(SwiftDraw), "Swift Draw", "Increases the archer's attack speed temporarily, allowing for rapid shots in quick succession.", null, "Mana: 14", 21005, 9400, School.FletchersFolio);
            Register(typeof(Featherweight), "Featherweight", "Reduces the weight and size of arrows, allowing the archer to carry more ammunition and move more swiftly.", null, "Mana: 10", 21005, 9400, School.FletchersFolio);
            Register(typeof(WindReader), "Wind Reader", "Enhances the archer's ability to read wind currents, improving accuracy over long distances and in difficult weather conditions.", null, "Mana: 12", 21005, 9400, School.FletchersFolio);
            Register(typeof(MultiShot), "Multi-Shot", "Shoots multiple arrows at once in a cone-shaped spread, hitting multiple targets within range.", null, "Mana: 20", 21005, 9400, School.FletchersFolio);
            Register(typeof(PoisonTip), "Poison Tip", "Coats the arrow tips with a potent poison, causing the next few shots to apply a poison debuff to targets, dealing damage over time.", null, "Mana: 18", 21005, 9400, School.FletchersFolio);
            Register(typeof(RangersFocus), "Rangers Focus", "Increases the archer's concentration, boosting critical hit chance and damage for a short period.", null, "Mana: 15", 21005, 9400, School.FletchersFolio);
            Register(typeof(BowyersBlessing), "Bowyers Blessing", "Temporarily enhances the durability and quality of the archer's bow, increasing attack damage and reducing the chance of weapon degradation.", null, "Mana: 22", 21005, 9400, School.FletchersFolio);
        }
    }
}
