using System;
using Server;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class InscribeMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(CallScribes), "Call Scribes", "Call Scribes to defend you", null, "Mana: 25", 21005, 9301, School.ScribesCompendium);
			Register(typeof(GlyphOfDestruction), "GlyphDestruction", "Creates a glyph that deals damage to enemies in a small area when activated.", null, "Mana: 20", 21004, 9300, School.ScribesCompendium);
            Register(typeof(WardOfProtection), "Ward of Protection", "Enchants an ally with a protective glyph that reduces incoming damage for a short duration.", null, "Mana: 25", 21004, 9300, School.ScribesCompendium);
            Register(typeof(SealOfFlames), "Seal of Flames", "Inscribes a magical seal on the ground that periodically damages enemies standing within its area.", null, "Mana: 30", 21004, 9300, School.ScribesCompendium);
            Register(typeof(RuneOfConfusion), "Rune of Confusion", "Casts a rune that confuses and disorients enemies, causing them to attack randomly for a short period.", null, "Mana: 20", 21004, 9300, School.ScribesCompendium);
            Register(typeof(MarkOfTheBerserker), "Berserker", "Temporarily enhances the combat abilities of an ally, increasing their attack speed and damage.", null, "Mana: 30", 21004, 9300, School.ScribesCompendium);
            Register(typeof(ScrollOfRecall), "Scroll of Recall", "Creates a magical scroll that allows instant travel to a previously marked location.", null, "Mana: 40", 21004, 9300, School.ScribesCompendium);
            Register(typeof(GlyphOfInsight), "Glyph of Insight", "Reveals hidden objects and creatures within a certain radius.", null, "Mana: 20", 21004, 9300, School.ScribesCompendium);
            Register(typeof(RuneOfTeleportation), "Teleportation", "Teleports the caster or an ally to a designated location.", null, "Mana: 35", 21004, 9300, School.ScribesCompendium);
            Register(typeof(SigilOfHealing), "Sigil of Healing", "Creates a sigil that slowly regenerates health for the caster or an ally within its area.", null, "Mana: 30", 21004, 9300, School.ScribesCompendium);
            Register(typeof(InscriptionOfWisdom), "Wisdom", "Grants a temporary boost to intelligence and skill effectiveness.", null, "Mana: 25", 21004, 9300, School.ScribesCompendium);
            Register(typeof(ScrollOfIdentification), "Identification", "Identifies unknown items or magical properties when used.", null, "Mana: 15", 21004, 9300, School.ScribesCompendium);
            Register(typeof(GlyphOfSilence), "Glyph of Silence", "Creates an area where magic cannot be cast, silencing enemies and allies alike.", null, "Mana: 30", 21004, 9300, School.ScribesCompendium);
            Register(typeof(RuneOfIllumination), "Rune of Illumination", "Lights up a large area or dark space, making it visible and dispelling darkness.", null, "Mana: 15", 21004, 9300, School.ScribesCompendium);
            Register(typeof(SealOfInvisibility), "Seal of Invisibility", "Grants temporary invisibility to the caster or an ally.", null, "Mana: 35", 21004, 9300, School.ScribesCompendium);
            Register(typeof(ScriptOfSummoning), "Summoning", "Summons a temporary magical construct or creature to aid the caster in combat or exploration.", null, "Mana: 50", 21004, 9300, School.ScribesCompendium);
        }
    }
}
