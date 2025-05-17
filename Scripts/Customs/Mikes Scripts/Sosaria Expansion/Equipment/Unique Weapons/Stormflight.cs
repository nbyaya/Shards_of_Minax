using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Stormflight : FletchersBow
{
    [Constructable]
    public Stormflight()
    {
        Name = "Stormflight";
        Hue = Utility.Random(1350, 1400); // A glowing shade of blue, with a faint electric shimmer
        MinDamage = Utility.RandomMinMax(25, 40);
        MaxDamage = Utility.RandomMinMax(45, 70);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 20;

        // Slayer effect – Stormflight is particularly effective against flying or aerial enemies
        Slayer = SlayerName.Fey;

        // Weapon attributes - Increasing precision and damage, tying into the weapon's storm theme
        WeaponAttributes.HitLightning = 30;
        WeaponAttributes.BattleLust = 15;
        
        // Skill bonuses that fit with the use of bows and fletching
        SkillBonuses.SetValues(0, SkillName.Hiding, 25.0);
        SkillBonuses.SetValues(1, SkillName.Fletching, 15.0);
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0);
        
        // A thematic bonus for Stormflight’s elemental power and precision
        Attributes.CastSpeed = 1;
        Attributes.SpellDamage = 5;

        // Xml-level item to ensure it is considered as a rare and powerful item in the world
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Stormflight(Serial serial) : base(serial)
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
