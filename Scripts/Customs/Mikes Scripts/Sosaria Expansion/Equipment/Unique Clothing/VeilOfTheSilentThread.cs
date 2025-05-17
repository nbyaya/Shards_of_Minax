using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VeilOfTheSilentThread : ClothNinjaHood
{
    [Constructable]
    public VeilOfTheSilentThread()
    {
        Name = "Veil of the Silent Thread";
        Hue = 0x455; // Dark, shadowy hue (can be adjusted for desired color)
        
        // Set attributes and bonuses
        Attributes.WeaponSpeed = 15;
        Attributes.WeaponDamage = 10;
        Attributes.DefendChance = 10;
        Attributes.Luck = 50;

        // Resistances
        Resistances.Physical = 10;
        Resistances.Cold = 10;
        Resistances.Poison = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);  // Enhances stealth abilities
        SkillBonuses.SetValues(1, SkillName.Ninjitsu, 15.0); // Boosts Ninjitsu skill, adding depth to the ninja theme
        SkillBonuses.SetValues(2, SkillName.Hiding, 15.0);   // Improves hiding, essential for ninjas to avoid detection
        SkillBonuses.SetValues(3, SkillName.Fencing, 10.0);  // Adds proficiency in fencing, common for ninjas
        SkillBonuses.SetValues(4, SkillName.Swords, 10.0);   // Aiding in close combat, necessary for a ninja

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VeilOfTheSilentThread(Serial serial) : base(serial)
    {
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
