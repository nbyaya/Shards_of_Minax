using System;
using Server;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class MeditationMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(SummonAllies), "Summon Allies", "Summon Allies to defend you", null, "Mana: 25", 21005, 9301, School.MediatorsGuide);
			Register( typeof( FocusBeam ),       "Focus Beam",       "Release a concentrated beam of magical energy that deals damage based on your Meditation skill level.", null, "Mana: 20", 21004, 9300, School.MediatorsGuide );
            Register( typeof( MindShield ),       "Mind Shield",       "Create a protective barrier around yourself or an ally that absorbs incoming damage for a short duration.", null, "Mana: 30", 21004, 9300, School.MediatorsGuide );
            Register( typeof( TranquilStrike ),   "Tranquil Strike",   "Channel a wave of calm that stuns a target temporarily and reduces their damage output.", null, "Mana: 25", 21004, 9300, School.MediatorsGuide );
            Register( typeof( ZenBlast ),         "Zen Blast",         "Unleash a powerful area-of-effect burst of energy that damages enemies and has a chance to lower their attack speed.", null, "Mana: 35", 21004, 9300, School.MediatorsGuide );
            Register( typeof( CalmingPresence ),  "Calming Presence",  "Temporarily pacify enemies in a radius, reducing their aggression and making them less likely to attack.", null, "Mana: 20", 21004, 9300, School.MediatorsGuide );
            Register( typeof( InnerFocus ),       "Inner Focus",       "Enhance your accuracy and critical hit chance for a short period, allowing you to perform better in combat.", null, "Mana: 15", 21004, 9300, School.MediatorsGuide );
            Register( typeof( MeditativeHeal ),   "Meditative Heal",   "Use your Meditation skill to gradually heal yourself or an ally over time.", null, "Mana: 25", 21004, 9300, School.MediatorsGuide );
            Register( typeof( ManaRestoration ),  "Mana Restoration",  "Restore a portion of your or an ally's mana, based on your Meditation skill level.", null, "Mana: 20", 21004, 9300, School.MediatorsGuide );
            Register( typeof( MindsEye ),         "Mind's Eye",        "Increase your or an ally's perception, making it easier to detect hidden enemies or traps for a duration.", null, "Mana: 20", 21004, 9300, School.MediatorsGuide );
            Register( typeof( EnergyShield ),     "Energy Shield",     "Create a temporary shield that absorbs a portion of incoming magical damage.", null, "Mana: 30", 21004, 9300, School.MediatorsGuide );
            Register( typeof( CalmMind ),         "Calm Mind",         "Remove debuffs or status effects from yourself or an ally, restoring normal functionality.", null, "Mana: 25", 21004, 9300, School.MediatorsGuide );
            Register( typeof( MentalClarity ),    "Mental Clarity",    "Temporarily boost your or an ally's focus, increasing skill effectiveness and reducing cooldowns.", null, "Mana: 20", 21004, 9300, School.MediatorsGuide );
            Register( typeof( InnerSanctum ),     "Inner Sanctum",     "Create a safe zone where allies inside gain increased resistance to damage and status effects for a short period.", null, "Mana: 35", 21004, 9300, School.MediatorsGuide );
            Register( typeof( EnlightenedVision ),"Enlightened Vision","Allow yourself or an ally to see hidden objects or creatures for a short time.", null, "Mana: 20", 21004, 9300, School.MediatorsGuide );
            Register( typeof( Tranquilize ),      "Tranquilize",       "Calm a group of enemies, reducing their movement speed and attack power for a brief period.", null, "Mana: 30", 21004, 9300, School.MediatorsGuide );
        }
    }
}
