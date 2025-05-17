using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CragsWoe : Maul
{
    [Constructable]
    public CragsWoe()
    {
        Name = "Crag’s Woe";
        Hue = Utility.Random(1200, 1295);  // A muted, rocky brown, symbolizing the rugged mountain terrain
        MinDamage = Utility.RandomMinMax(35, 55);
        MaxDamage = Utility.RandomMinMax(60, 90);
        
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 10;
        
        // Slayer Effect - especially effective against Orcs, given Crag’s ties to mining and earth-based lore
        Slayer = SlayerName.OrcSlaying;

        // Weapon Attributes - increasing the maul’s destructive capacity and dealing with the brute force of the land
        WeaponAttributes.HitLeechHits = 30;
        WeaponAttributes.HitLeechStam = 25;
        WeaponAttributes.BattleLust = 15;
        WeaponAttributes.HitLowerDefend = 20;

        // Skill Bonuses for a physically brutal weapon with tactical advantage in combat
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // For crushing strikes and battlefield awareness
        SkillBonuses.SetValues(1, SkillName.Macing, 20.0);  // Increases effectiveness with maul-type weapons
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);  // Bonus to grappling and close combat moves

        // Additional thematic bonus for working in harsh, earth-related conditions (reflecting Devil Guard's mining and toughness)
        SkillBonuses.SetValues(3, SkillName.Mining, 15.0);  // Increases the ability to break through hard materials like stone

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CragsWoe(Serial serial) : base(serial)
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
