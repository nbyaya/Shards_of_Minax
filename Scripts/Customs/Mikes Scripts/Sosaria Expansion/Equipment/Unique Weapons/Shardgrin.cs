using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Shardgrin : HammerPick
{
    [Constructable]
    public Shardgrin()
    {
        Name = "Shardgrin";
        Hue = Utility.Random(1150, 1300);  // A dark, ominous hue to represent the weapon's cursed nature
        MinDamage = Utility.RandomMinMax(30, 45);
        MaxDamage = Utility.RandomMinMax(50, 80);

        Attributes.BonusStr = 5;
        Attributes.BonusDex = 3;
        Attributes.BonusInt = 2;
        Attributes.Luck = 15;
        
        // The weapon has a special ability, reflecting its dark nature
        Attributes.WeaponSpeed = 5;
        Attributes.DefendChance = 10;

        // Slayer effect – Shardgrin is particularly effective against creatures of stone and earth
        Slayer = SlayerName.EarthShatter;

        // Unique weapon properties – it has a chance to break stone and cause a lingering fear in its victims
        WeaponAttributes.HitDispel = 20;
        WeaponAttributes.HitPhysicalArea = 30;
        WeaponAttributes.HitLeechStam = 15;

        // Skill bonuses related to the use of this tool – suitable for combat and mining endeavors
        SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(2, SkillName.Macing, 25.0);
        
        // An additional thematic bonus, enhancing the weapon’s role as both a tool of war and mining
        SkillBonuses.SetValues(3, SkillName.ArmsLore, 10.0);

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Shardgrin(Serial serial) : base(serial)
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
