using System;
using Server;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class MageryMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(ArcaneBlast), "Arcane Blast", "Unleashes a powerful magical burst at a single target, dealing heavy damage.", null, "Mana: 20", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(MeteorSwarm), "Meteor Swarm", "Summons a shower of meteors that rain down on a designated area.", null, "Mana: 30", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(FrostNova), "Frost Nova", "Conjures a wave of freezing energy around the caster.", null, "Mana: 25", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(LightningStorm), "Lightning Storm", "Calls down a storm of lightning bolts from the sky.", null, "Mana: 35", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(ManaDrain), "Mana Drain", "Drains the magical energy from an enemy.", null, "Mana: 20", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(MeteorShower), "Apocalypse", "Summons a fiery meteor shower on a target area.", null, "Mana: 40", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(ShadowBolt), "Shadow Bolt", "Fires a bolt of dark energy at a single target.", null, "Mana: 25", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(TimeStop), "Time Stop", "Briefly halts the flow of time around the caster.", null, "Mana: 50", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(Teleportation), "Greater Teleport", "Instantly transports the caster to a previously marked location.", null, "Mana: 15", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(ArcaneShield), "Arcane Shield", "Creates a protective barrier around the caster.", null, "Mana: 20", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(Invisibility), "Greater Invisibility", "Renders the caster invisible to others.", null, "Mana: 30", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(SummonFamiliar), "Summon Familiar", "Conjures a magical creature to assist the caster.", null, "Mana: 40", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(MagicWard), "Magic Ward", "Establishes a protective magical ward in a designated area.", null, "Mana: 35", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(DispelMagic), "Disjunction", "Removes magical effects from a target or area.", null, "Mana: 20", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(Levitation), "Levitation", "Allows the caster to float above the ground.", null, "Mana: 25", 21004, 9300, School.ArchmagesGrimoire);
            Register(typeof(ArcaneVision), "Arcane Vision", "Enhances the caster’s vision to see magical auras and hidden objects.", null, "Mana: 15", 21004, 9300, School.ArchmagesGrimoire);
        }
    }
}
