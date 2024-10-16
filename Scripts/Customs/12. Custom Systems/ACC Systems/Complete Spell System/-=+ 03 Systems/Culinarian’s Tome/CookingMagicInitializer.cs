using System;
using Server;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class CookingMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(FeastOfFortitude), "Feast of Fortitude", "A hearty meal that bolsters the vitality of those who consume it.", null, "Duration: 30 minutes", 21004, 9300, School.CulinariansTome);
            Register(typeof(PotionPie), "Potion Pie", "A delectable pie with a surprise inside; eat at your own risk!", null, "Effect: Random potion", 21004, 9300, School.CulinariansTome);
            Register(typeof(SpicyInfernoStew), "Spicy Inferno Stew", "A fiery stew that causes the consumer to radiate heat, scorching enemies in close proximity.", null, "Duration: Varies", 21004, 9300, School.CulinariansTome);
            Register(typeof(GastronomicGrit), "Gastronomic Grit", "A robust dish that imbues the eater with boundless energy and endurance.", null, "Duration: 1 hour", 21004, 9300, School.CulinariansTome);
            Register(typeof(BardsBrew), "Bard's Brew", "A soothing brew that calms the nerves and sharpens the mind, perfect for bards and performers.", null, "Duration: Varies", 21004, 9300, School.CulinariansTome);
            Register(typeof(CursedConfection), "Cursed Confection", "A seemingly delectable treat that harbors a hidden malediction.", null, "Effect: Random negative effect", 21004, 9300, School.CulinariansTome);
            Register(typeof(ElixirOfEnlightenment), "Elixir of Enlightenment", "A rare elixir that sharpens the mind and enhances the learning of any skill.", null, "Duration: Varies", 21004, 9300, School.CulinariansTome);
            Register(typeof(MysticMince), "Mystic Mince", "A savory mince that is rumored to enhance one’s magical abilities after consumption.", null, "Duration: Varies", 21004, 9300, School.CulinariansTome);
            Register(typeof(AmbrosialAroma), "Ambrosial Aroma", "A dish with a fragrance so divine, it calms even the most feral beasts.", null, "Duration: Varies", 21004, 9300, School.CulinariansTome);
            Register(typeof(HungersBane), "Hunger's Bane", "A satiating meal that eliminates hunger pangs for hours on end.", null, "Duration: 1 day", 21004, 9300, School.CulinariansTome);
            Register(typeof(GuardiansGrub), "Guardian's Grub", "A hearty meal that toughens the skin and strengthens the bones.", null, "Duration: Varies", 21004, 9300, School.CulinariansTome);
            Register(typeof(ChefsSurprise), "Chef's Surprise", "An experimental creation by the chef that promises unexpected results.", null, "Effect: Random", 21004, 9300, School.CulinariansTome);
            Register(typeof(InvisibilitySouffle), "Invisibility Soufflé", "A light and airy soufflé that allows one to vanish from sight.", null, "Duration: Varies", 21004, 9300, School.CulinariansTome);
            Register(typeof(EaglesFeast), "Eagle's Feast", "A dish that sharpens reflexes and enhances dexterity, making one as swift as an eagle.", null, "Duration: Varies", 21004, 9300, School.CulinariansTome);
            Register(typeof(WardensWaffle), "Warden's Waffle", "A delightful waffle that bolsters the healing powers of the eater.", null, "Duration: Varies", 21004, 9300, School.CulinariansTome);
            Register(typeof(HeartstopperChili), "Heartstopper Chili", "A dangerously hot chili that boosts attack power but comes with a painful price.", null, "Duration: Varies", 21004, 9300, School.CulinariansTome);
        }
    }
}
