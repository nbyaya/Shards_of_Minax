using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FangOfTheGreenWidow : PoisonBlade
{
    [Constructable]
    public FangOfTheGreenWidow()
    {
        Name = "Fang of the Green Widow";
        Hue = Utility.Random(2300, 2400);  // Dark green, representing the venomous bite of the spider
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 75);
        Attributes.WeaponSpeed = 10;  // Increased weapon speed to reflect quick strikes
        Attributes.Luck = 15;  // Luck bonus, as the weapon could bring fortune in dangerous situations
        
        // The weapon has a venomous theme, so it's particularly potent against spiders and related creatures
        Slayer = SlayerName.SpidersDeath;  // Effective against spiders
        
        // Weapon Attributes - themed for poison and lethality in combat
        WeaponAttributes.HitPoisonArea = 50;  // Adds poison effect to area
        WeaponAttributes.HitLeechStam = 20;  // Leech stamina on hit, weakening enemies
        WeaponAttributes.HitLeechHits = 15;  // Leech health, draining life force
        
        // Thematic bonus: enhancing the poison and stealth aspects of the weapon
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);  // Increases effectiveness of poisoning
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);  // Adds bonus to stealth, useful for sneaky attacks
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0);  // Increases wrestling, for close combat situations



        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FangOfTheGreenWidow(Serial serial) : base(serial)
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
