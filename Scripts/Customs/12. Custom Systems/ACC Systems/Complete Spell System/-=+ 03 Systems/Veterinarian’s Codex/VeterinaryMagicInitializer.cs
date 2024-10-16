using System;
using Server;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class VeterinaryMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(HealingTouch), "Healing Touch", "Instantly heal a creature's wounds, restoring a significant amount of health.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(Revitalize), "Revitalize", "Temporarily boost a creature's stamina or energy, improving its performance in combat.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(Antidote), "Antidote", "Cure a creature of poison or disease, providing a defense against harmful conditions.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(Comfort), "Comfort", "Calm a distressed or agitated creature, reducing its aggression and improving its morale.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(DiseaseResistance), "Disease Resistance", "Temporarily increase a creature's resistance to diseases and poisons.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(Surgery), "Surgery", "Perform a precise operation to remove injuries or ailments, providing long-term benefits to a creature's health.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(Triage), "Triage", "Quickly assess and stabilize multiple injured creatures, ensuring they don't succumb to their wounds.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(AnimalBond), "Animal Bond", "Enhance the bond between a creature and its master, increasing their cooperation and effectiveness in combat.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);

            Register(typeof(SavageBite), "Savage Bite", "Command the creature to unleash a powerful bite attack, dealing extra damage to enemies.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(ProtectiveStance), "Protective Stance", "Direct the creature to assume a defensive posture, reducing damage taken from attacks.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(Vigilance), "Vigilance", "Increase the creature's awareness, making it harder for enemies to surprise or ambush it.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(AggressiveCharge), "Aggressive Charge", "Order the creature to charge at enemies, stunning them and dealing impact damage.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(HealingAura), "Healing Aura", "Create a healing aura around the creature, gradually restoring health to itself and nearby allies.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(RallyingRoar), "Rallying Roar", "Command the creature to let out a rallying roar, boosting the morale of allies and temporarily increasing their attack power.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(ProtectiveShield), "Protective Shield", "Envelop the creature in a protective shield that absorbs a portion of incoming damage.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
            Register(typeof(Rejuvenate), "Rejuvenate", "Instantly restore a creature's stamina and mana, allowing it to continue fighting or using abilities.", null, "Mana: 20", 21005, 9301, School.VeterinariansCodex);
        }
    }
}
