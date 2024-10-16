using System;
using Server;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class ChivalryMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(CallRighteous), "Call Righteous", "Call the Righteous to defend you", null, "Mana: 25", 21005, 9301, School.PaladinsTestament);            
			Register(typeof(DivineStrike), "Divine Strike", "A powerful melee attack infused with holy energy that deals extra damage to undead and evil creatures.", null, "Mana: 20", 21001, 9300, School.PaladinsTestament);
            Register(typeof(BlessedShield), "Blessed Shield", "Temporarily increases the user's armor rating and provides a chance to block all incoming damage.", null, "Mana: 25", 21002, 9300, School.PaladinsTestament);
            Register(typeof(HolyLight), "Holy Light", "A burst of radiant light that damages all undead within a certain radius while healing allies.", null, "Mana: 30", 21003, 9300, School.PaladinsTestament);
            Register(typeof(ValorsCharge), "Valors Charge", "A forward charge attack that knocks down enemies in its path and deals bonus damage.", null, "Mana: 20", 21004, 9300, School.PaladinsTestament);
            Register(typeof(Purify), "Purify", "Cleanses the target of poisons and curses, also providing a brief immunity to future debuffs.", null, "Mana: 15", 21005, 9300, School.PaladinsTestament);
            Register(typeof(SmiteEvil), "Smite Evil", "A single-target attack that deals massive damage to evil-aligned creatures, such as demons and undead.", null, "Mana: 30", 21007, 9300, School.PaladinsTestament);
            Register(typeof(AuraOfCourage), "Aura of Courage", "Enhances the morale of nearby allies, boosting their attack speed and reducing fear effects.", null, "Mana: 20", 21008, 9300, School.PaladinsTestament);
            Register(typeof(LayOnHands), "Lay on Hands", "Heals a significant amount of health to a targeted ally or self, with a chance to revive a recently deceased player.", null, "Mana: 35", 21009, 9300, School.PaladinsTestament);
            Register(typeof(DivineRetribution), "Divine Retribution", "Reflects a portion of melee damage back to the attacker for a duration.", null, "Mana: 20", 21010, 9300, School.PaladinsTestament);
            Register(typeof(Sanctuary), "Sanctuary", "Creates a protective zone where allies receive healing over time and are immune to negative status effects.", null, "Mana: 40", 21011, 9300, School.PaladinsTestament);
            Register(typeof(HolyBlade), "Holy Blade", "Temporarily enchants the user's weapon with divine energy, increasing damage and granting a chance to smite enemies with holy fire.", null, "Mana: 25", 21012, 9300, School.PaladinsTestament);
            Register(typeof(Judgment), "Judgment", "Calls down a bolt of divine lightning on a targeted enemy, dealing heavy damage and stunning them for a short duration.", null, "Mana: 30", 21013, 9300, School.PaladinsTestament);
            Register(typeof(CrusadersResolve), "Crusaders Resolve", "Temporarily increases the user's strength and stamina, allowing for faster attacks and reduced mana cost for abilities.", null, "Mana: 20", 21014, 9300, School.PaladinsTestament);
            Register(typeof(MartyrsSacrifice), "Martyrs Sacrifice", "Sacrifices a portion of the user's health to heal all nearby allies or cleanse them of all debuffs.", null, "Mana: 30", 21015, 9300, School.PaladinsTestament);
            Register(typeof(RighteousFury), "Righteous Fury", "Empowers the user with increased critical hit chance and damage for a limited time, also granting immunity to crowd control effects.", null, "Mana: 35", 21016, 9300, School.PaladinsTestament);
        }
    }
}
