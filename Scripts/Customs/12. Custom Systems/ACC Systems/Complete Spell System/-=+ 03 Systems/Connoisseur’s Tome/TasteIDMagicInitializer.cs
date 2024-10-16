using System;
using Server;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class TasteIDMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(ToxicTaste), "Toxic Taste", "Apply a poison to your weapon or food that deals damage over time to enemies. Has a chance to inflict poison status.", null, "Mana: 10", 21004, 9300, School.ConnoisseursTome);
            Register(typeof(FlavorOfFury), "Flavor of Fury", "Temporarily increase your attack speed and damage, inspired by a taste that stirs great aggression.", null, "Mana: 15", 21005, 9301, School.ConnoisseursTome);
            Register(typeof(AromaticBurst), "Aromatic Burst", "Release a cloud of noxious fumes that blinds and disorients enemies, reducing their accuracy and movement speed.", null, "Mana: 20", 21006, 9302, School.ConnoisseursTome);
            Register(typeof(SpicyRetaliation), "Spicy Retaliation", "Release a burst of spicy energy when struck, dealing damage to attackers and burning them.", null, "Mana: 12", 21007, 9303, School.ConnoisseursTome);
            Register(typeof(SavoryShield), "Savory Shield", "Create a protective barrier that absorbs damage, based on the power of the flavor.", null, "Mana: 25", 21008, 9304, School.ConnoisseursTome);
            Register(typeof(FlavorfulStrike), "Flavorful Strike", "Infuse your next attack with a special flavor that causes additional elemental damage.", null, "Mana: 18", 21009, 9305, School.ConnoisseursTome);
            Register(typeof(GourmetDefense), "Gourmet Defense", "Increase your defense by creating a defensive aura, reducing incoming damage.", null, "Mana: 22", 21010, 9306, School.ConnoisseursTome);
            Register(typeof(ZestyDistraction), "Zesty Distraction", "Create a flavorful illusion that confuses and distracts enemies, making them attack the wrong target.", null, "Mana: 17", 21011, 9307, School.ConnoisseursTome);

            Register(typeof(EdibleIdentification), "Edible Identification", "Instantly identify the quality and effects of any food item or potion.", null, "Mana: 8", 21012, 9308, School.ConnoisseursTome);
            Register(typeof(TastingMeditation), "Tasting Meditation", "Restore health and mana by consuming a special dish or potion prepared with your taste skills.", null, "Mana: 15", 21013, 9309, School.ConnoisseursTome);
            Register(typeof(FlavorfulDetection), "Flavorful Detection", "Sense the presence of hidden or invisible creatures by detecting their unique scent or taste signature.", null, "Mana: 20", 21014, 9310, School.ConnoisseursTome);
            Register(typeof(GourmetCrafting), "Gourmet Crafting", "Improve the quality of food items or potions you craft, enhancing their effectiveness.", null, "Mana: 12", 21015, 9311, School.ConnoisseursTome);
            Register(typeof(TastefulHealing), "Tasteful Healing", "Create a healing tonic that speeds up recovery from wounds or diseases.", null, "Mana: 18", 21016, 9312, School.ConnoisseursTome);
            Register(typeof(ScentOfValor), "Scent of Valor", "Prepare a special aromatic concoction that boosts the morale of allies and grants them temporary resistance to fear.", null, "Mana: 20", 21017, 9313, School.ConnoisseursTome);
            Register(typeof(FlavorInfusion), "Flavor Infusion", "Enhance the properties of any item by infusing it with a special flavor, providing additional buffs.", null, "Mana: 15", 21018, 9314, School.ConnoisseursTome);
            Register(typeof(TastefulCamouflage), "Tasteful Camouflage", "Blend into your surroundings with a camouflaging effect based on nearby tastes, reducing visibility.", null, "Mana: 12", 21019, 9315, School.ConnoisseursTome);
        }
    }
}
