using System;
using Server;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class TailoringMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(CallAllies), "Call Allies", "Call Allies to defend you", null, "Mana: 25", 21005, 9301, School.TailorsWorkbook);
			Register(typeof(ThreadOfPain), "Thread of Pain", "Temporarily imbue a weapon with a magical thread, causing it to deal additional damage and slow enemies.", null, "Mana: 20", 21001, 9300, School.TailorsWorkbook);
            Register(typeof(StitchingStrike), "Stitching Strike", "Use a magically enchanted needle to deal damage to an enemy, with a chance to cause bleeding effects.", null, "Mana: 25", 21002, 9300, School.TailorsWorkbook);
            Register(typeof(FabricShield), "Fabric Shield", "Create a protective shield from enchanted fabric that absorbs a portion of incoming damage.", null, "Mana: 30", 21003, 9300, School.TailorsWorkbook);
            Register(typeof(EnsnaringNet), "Ensnaring Net", "Deploy a magical net that entangles and slows down enemies in a specific area.", null, "Mana: 25", 21004, 9300, School.TailorsWorkbook);
            Register(typeof(WovenWard), "Woven Ward", "Summon a protective aura around yourself or an ally that increases defense and resistance.", null, "Mana: 30", 21005, 9300, School.TailorsWorkbook);
            Register(typeof(ThreadedTrap), "Threaded Trap", "Lay down a trap that, when triggered, ensnares and damages enemies caught in its path.", null, "Mana: 35", 21006, 9300, School.TailorsWorkbook);
            Register(typeof(QuickRepair), "Quick Repair", "Instantly repair damaged clothing or armor to restore its durability.", null, "Mana: 15", 21008, 9300, School.TailorsWorkbook);
            Register(typeof(DyeMastery), "Dye Mastery", "Change the color and appearance of items and clothing with magical dyes.", null, "Mana: 20", 21009, 9300, School.TailorsWorkbook);
            Register(typeof(TailorsTouch), "Tailor's Touch", "Enhance the quality of crafted items, providing additional bonuses or effects.", null, "Mana: 25", 21010, 9300, School.TailorsWorkbook);
            Register(typeof(CamouflageCloak), "Camouflage Cloak", "Create a cloak that blends with the surroundings, making you less visible to enemies.", null, "Mana: 20", 21011, 9300, School.TailorsWorkbook);
            Register(typeof(PortableWorkbench), "Portable Workbench", "Summon a temporary workbench that allows for on-the-spot crafting and repairs.", null, "Mana: 25", 21012, 9300, School.TailorsWorkbook);
            Register(typeof(EnchantedNeedle), "Enchanted Needle", "Use a magical needle to mend wounds or ailments on allies, providing a healing effect.", null, "Mana: 30", 21013, 9300, School.TailorsWorkbook);
            Register(typeof(SilkStep), "Silk Step", "Move with increased agility and stealth for a short duration, making it easier to avoid detection.", null, "Mana: 20", 21014, 9300, School.TailorsWorkbook);
            Register(typeof(FabricFamiliarSpell), "Fabric Familiar", "Summon a small, fabric-based creature that can assist in gathering materials or scouting.", null, "Mana: 30", 21015, 9300, School.TailorsWorkbook);
            Register(typeof(WardrobeOfHolding), "Wardrobe of Holding", "Create a magical wardrobe that stores items and provides quick access to your gear.", null, "Mana: 35", 21016, 9300, School.TailorsWorkbook);
        }
    }
}
