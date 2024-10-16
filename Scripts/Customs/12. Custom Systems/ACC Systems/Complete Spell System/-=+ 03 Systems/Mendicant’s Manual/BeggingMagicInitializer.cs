using System;
using Server;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class BeggingMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(CallBeggers), "Call Beggers", "Call Beggers to defend you", null, "Mana: 25", 21005, 9301, School.MendicantsManual);
			Register( typeof( PleadForAlms ),   "Plead for Alms",  "Temporarily increase the likelihood of receiving a donation from NPCs or players.", null, "Mana: 10", 21004, 9300, School.MendicantsManual );
            Register( typeof( CompellingRequest ),   "Compelling Request",  "Boost the amount of gold or items you receive from a successful beg.", null, "Mana: 12", 21004, 9300, School.MendicantsManual );
            Register( typeof( EmpathyAbility ),   "Empathy",  "Increase the chance of receiving better-quality items or more gold when begging.", null, "Mana: 15", 21004, 9300, School.MendicantsManual );
            Register( typeof( StreetwiseInsight ),   "Streetwise Insight",  "Reveal the wealth status of NPCs, allowing you to target those who might give more.", null, "Mana: 18", 21004, 9300, School.MendicantsManual );
            Register( typeof( CharitableAura ),   "Charitable Aura",  "Create a temporary aura that increases the generosity of NPCs in a specific area.", null, "Mana: 20", 21004, 9300, School.MendicantsManual );
            Register( typeof( FlatteringSpeech ),   "Flattering Speech",  "Use a flattering speech to increase the chance of receiving a higher donation.", null, "Mana: 12", 21004, 9300, School.MendicantsManual );
            Register( typeof( DesperateAppeal ),   "Desperate Appeal",  "Make a dramatic appeal that greatly increases the chance of receiving a donation but has a cooldown period.", null, "Mana: 25", 21004, 9300, School.MendicantsManual );
            Register( typeof( SympatheticGesture ),   "Sympathetic Gesture",  "Use a sympathetic gesture that reduces the chance of receiving negative reactions from NPCs.", null, "Mana: 14", 21004, 9300, School.MendicantsManual );
            Register( typeof( PleadForMercy ),   "Plead for Mercy",  "Temporarily reduce the likelihood of being chased away or attacked when begging.", null, "Mana: 16", 21004, 9300, School.MendicantsManual );
            Register( typeof( VagrantsWisdom ),   "Vagrants Wisdom",  "Gain insight into the preferences of NPCs, increasing the effectiveness of your begging attempts.", null, "Mana: 18", 21004, 9300, School.MendicantsManual );
            Register( typeof( MendicantsCharm ),   "Mendicants Charm",  "Use charm to temporarily increase your success rate when begging from a specific NPC.", null, "Mana: 14", 21004, 9300, School.MendicantsManual );
            Register( typeof( SilverTongueSpell ),   "Silver Tongue",  "Improve your ability to negotiate for better rewards from NPCs.", null, "Mana: 16", 21004, 9300, School.MendicantsManual );
            Register( typeof( GenuineSuffering ),   "Genuine Suffering",  "Display signs of genuine suffering to increase the likelihood of receiving a donation.", null, "Mana: 22", 21004, 9300, School.MendicantsManual );
            Register( typeof( PersuasiveAppeal ),   "Persuasive Appeal",  "Persuade NPCs to give more by using a combination of logic and emotional appeal.", null, "Mana: 20", 21004, 9300, School.MendicantsManual );
            Register( typeof( SubtleManipulation ),   "Subtle Manipulation",  "Use subtle manipulation techniques to increase the effectiveness of your begging without drawing unwanted attention.", null, "Mana: 18", 21004, 9300, School.MendicantsManual );
        }
    }
}
