using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Gloambite : ShadowSai
{
    [Constructable]
    public Gloambite()
    {
        Name = "Gloambite";
        Hue = Utility.Random(1150, 1250);  // A deep purple to black hue, representing shadows and darkness.
        MinDamage = Utility.RandomMinMax(25, 40);
        MaxDamage = Utility.RandomMinMax(45, 70);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 15;
        
        // Slayer effect – Gloambite is particularly effective against creatures of the dark, like vampires and demons
        Slayer = SlayerName.ArachnidDoom;
        
        // Weapon attributes to enhance its stealthy, shadow-based combat style
        WeaponAttributes.HitLeechHits = 25;
        WeaponAttributes.HitLeechMana = 20;
        WeaponAttributes.HitLeechStam = 15;
        WeaponAttributes.BattleLust = 20;
        
        // Skill bonuses for stealth and subterfuge, which play into the weapon's theme as a deadly, silent strike tool
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.Ninjitsu, 15.0);
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0);
        
        // Additional thematic bonus – enhancing sneak attacks and making strikes more devastating
        SkillBonuses.SetValues(3, SkillName.Wrestling, 10.0);
        
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Gloambite(Serial serial) : base(serial)
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
