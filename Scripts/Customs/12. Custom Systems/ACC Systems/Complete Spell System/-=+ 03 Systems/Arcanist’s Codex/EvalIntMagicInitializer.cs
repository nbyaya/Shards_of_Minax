using System;
using Server;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class ArcanistsCodexInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(ArcaneBlast), "Arcane Blast", "Unleashes a powerful burst of arcane energy at a target, dealing high damage based on the caster’s Intelligence.", null, "Mana: 20", 21005, 9400, School.ArcanistsCodex);
            Register(typeof(MysticMissile), "Mystic Missile", "Fires a stream of magical projectiles that seek out and strike the enemy, with damage scaling based on the caster’s Intelligence.", null, "Mana: 15", 21005, 9401, School.ArcanistsCodex);
            Register(typeof(ElementalSurge), "Elemental Surge", "Channels elemental energy to create a surge of fire, ice, or lightning that damages all enemies in a small area.", null, "Mana: 25", 21005, 9402, School.ArcanistsCodex);
            Register(typeof(ManaRend), "Mana Rend", "Drains the target’s mana, causing them damage and replenishing a portion of the caster’s mana pool.", null, "Mana: 20", 21005, 9403, School.ArcanistsCodex);
            Register(typeof(ArcaneShield), "Arcane Shield", "Creates a protective barrier of arcane energy around the caster, absorbing a portion of incoming damage.", null, "Mana: 30", 21005, 9404, School.ArcanistsCodex);
            Register(typeof(SpellWeaving), "Spell Weaving", "Temporarily boosts the effectiveness of the caster’s spells, increasing their damage or utility.", null, "Mana: 25", 21005, 9405, School.ArcanistsCodex);
            Register(typeof(TeleportStrike), "Teleport Strike", "Teleports behind the enemy to deliver a surprise attack, dealing additional damage.", null, "Mana: 20", 21005, 9406, School.ArcanistsCodex);
            Register(typeof(EtherealStrike), "Ethereal Strike", "Strikes with a spectral force that bypasses physical defenses, dealing damage regardless of the target’s armor.", null, "Mana: 20", 21005, 9407, School.ArcanistsCodex);
            Register(typeof(ArcaneInsight), "Arcane Insight", "Reveals hidden traps and magical effects in the vicinity, making it easier to avoid or disarm them.", null, "Mana: 15", 21005, 9408, School.ArcanistsCodex);
            Register(typeof(ManaInfusion), "Mana Infusion", "Restores a significant amount of mana to the caster or an ally, useful in prolonged encounters.", null, "Mana: 30", 21005, 9409, School.ArcanistsCodex);
            Register(typeof(Teleportation), "Teleportation", "Teleports the caster to a predetermined location or to the last saved location.", null, "Mana: 40", 21005, 9410, School.ArcanistsCodex);
            Register(typeof(MagicalWard), "Magical Ward", "Creates a ward that reduces the effectiveness of enemy spells within a small radius.", null, "Mana: 25", 21005, 9411, School.ArcanistsCodex);
            Register(typeof(IllusionaryDecoy), "Illusionary Decoy", "Summons an illusionary duplicate of the caster to distract and confuse enemies, reducing the likelihood of being targeted.", null, "Mana: 20", 21005, 9412, School.ArcanistsCodex);
            Register(typeof(ElementalProtection), "Elemental Protection", "Provides temporary resistance to elemental damage (fire, ice, lightning) for the caster or an ally.", null, "Mana: 20", 21005, 9413, School.ArcanistsCodex);
            Register(typeof(ArcaneEye), "Arcane Eye", "Summons an arcane eye that provides a brief period of enhanced vision, revealing hidden enemies and objects.", null, "Mana: 15", 21005, 9414, School.ArcanistsCodex);
            Register(typeof(SpellEcho), "Spell Echo", "Repeats the last spell cast, allowing the caster to use it again with reduced mana cost.", null, "Mana: 25", 21005, 9415, School.ArcanistsCodex);
        }
    }
}
