using System;
using Server;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class MacingMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(SummonAllies), "Summon Allies", "Summon Allies to defend you", null, "Mana: 25", 21005, 9301, School.MacebearersTome);
			Register(typeof(StunningBlow), "Stunning Blow", "Temporarily stuns an enemy, leaving them vulnerable to follow-up attacks.", null, "Mana: 20", 21004, 9300, School.MacebearersTome);
            Register(typeof(CrushingImpact), "Crushing Impact", "Delivers a powerful swing that causes extra damage and may knock the target down.", null, "Mana: 25", 21004, 9300, School.MacebearersTome);
            Register(typeof(ArmorShatter), "Armor Shatter", "Strikes with such force that it reduces the target's armor rating for a short duration.", null, "Mana: 30", 21004, 9300, School.MacebearersTome);
            Register(typeof(SpikedAssault), "Spiked Assault", "Infuses the mace with spikes, causing additional bleed damage over time.", null, "Mana: 20", 21004, 9300, School.MacebearersTome);
            Register(typeof(Pulverize), "Pulverize", "Deals a massive blow that has a chance to break the target's weapon or shield.", null, "Mana: 35", 21004, 9300, School.MacebearersTome);
            Register(typeof(ShatteringStrike), "Shattering Strike", "Aimed at breaking the enemy's defense, this ability lowers the enemy's chance to block or parry.", null, "Mana: 25", 21004, 9300, School.MacebearersTome);
            Register(typeof(MaceDance), "Mace Dance", "A rapid series of strikes that deals increased damage with each successive hit.", null, "Mana: 20", 21004, 9300, School.MacebearersTome);
            Register(typeof(SunderingBlow), "Sundering Blow", "Strikes with increased force to penetrate the target's defenses, causing bonus damage based on the target's remaining health.", null, "Mana: 30", 21004, 9300, School.MacebearersTome);
            Register(typeof(ShieldBash), "Shield Bash", "Uses the mace to bash an enemy's shield, potentially disarming them or reducing their block chance.", null, "Mana: 25", 21004, 9300, School.MacebearersTome);
            Register(typeof(GroundSlam), "Ground Slam", "Smashes the ground with the mace, causing a shockwave that can knock back enemies in a small area.", null, "Mana: 30", 21004, 9300, School.MacebearersTome);
            Register(typeof(StalwartStance), "Stalwart Stance", "Temporarily increases the player's defense and resistance to physical damage, mimicking the stability of a fortress.", null, "Mana: 15", 21004, 9300, School.MacebearersTome);
            Register(typeof(Fortify), "Fortify", "Enhances the durability of the player's armor or shield, providing temporary damage reduction.", null, "Mana: 20", 21004, 9300, School.MacebearersTome);
            Register(typeof(Pathfinder), "Pathfinder", "Clears obstacles or debris from the player's path, making traversal easier.", null, "Mana: 10", 21004, 9300, School.MacebearersTome);
            Register(typeof(MaceMastery), "Mace Mastery", "Passively increases the player's accuracy and critical strike chance with maces.", null, "Mana: 0", 21004, 9300, School.MacebearersTome);
            Register(typeof(QuickReflexes), "Quick Reflexes", "Improves dodge chance and reaction speed, allowing the player to evade attacks more effectively.", null, "Mana: 20", 21004, 9300, School.MacebearersTome);

        }
    }
}
