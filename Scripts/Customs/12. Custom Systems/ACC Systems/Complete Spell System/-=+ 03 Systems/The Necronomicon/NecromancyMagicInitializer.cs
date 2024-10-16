using System;
using Server;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class NecromancyMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(SummonUndead), "Summon Undead", "Summon Undead to defend you", null, "Mana: 25", 21005, 9301, School.TheNecronomicon);
			Register(typeof(DarkRift), "Dark Rift", "Creates a rift that summons shadowy creatures to attack enemies over time.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(NecroticBlast), "Necrotic Blast", "Unleashes a burst of necrotic energy that deals damage and reduces the target's healing effectiveness.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(GraveTouch), "Grave Touch", "A melee attack that inflicts damage and temporarily paralyzes the target with fear.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(SoulDrain), "Soul Drain", "Drains the life force of an enemy, converting a portion of the damage dealt into health for the caster.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(CorpseExplosion), "Corpse Explosion", "Causes a corpse to explode, dealing damage to nearby enemies and spreading a plague that weakens them.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(DeathsEmbrace), "Death's Embrace", "Temporarily summons a spectral hand that grips and slows enemies in a targeted area, dealing damage over time.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(Wraithform), "Wraithform", "Transforms into a wraith, gaining increased speed and evasion while becoming partially ethereal, avoiding physical damage.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(RaiseUndead), "Raise Undead", "Reanimates fallen enemies or allies as undead minions under the caster's control.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(NecromancersWard), "Necromancer's Ward", "Creates a protective barrier that absorbs a portion of incoming damage and converts it into mana or health.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(PlagueAura), "Plague Aura", "Surrounds the caster with a plague aura that damages and weakens enemies within a certain radius.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(BoneArmorSpell), "Bone Armor", "Summons a suit of skeletal armor that enhances the caster's defense and reflects a portion of physical damage back to attackers.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(DarkVision), "Dark Vision", "Allows the caster to see in darkness and detect hidden or invisible creatures.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(EtherealPassage), "Ethereal Passage", "Enables the caster to pass through obstacles or barriers for a short period, useful for escaping or navigating difficult terrain.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(PhantomSteedSpell), "Phantom Steed", "Summons a spectral mount that provides increased speed and can pass through terrain obstacles.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
            Register(typeof(GrimHarvest), "Grim Harvest", "Collects the essence of defeated enemies, providing a temporary boost to the caster's abilities or regenerating resources.", null, "Mana: 20", 21004, 9300, School.TheNecronomicon);
        }
    }
}
