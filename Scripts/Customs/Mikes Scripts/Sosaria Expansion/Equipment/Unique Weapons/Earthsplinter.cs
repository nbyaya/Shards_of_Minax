using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Earthsplinter : TwoHandedAxe
{
    [Constructable]
    public Earthsplinter()
    {
        Name = "Earthsplinter";
        Hue = Utility.Random(0x8B5, 0x8F0); // A deep, earthy green, symbolizing the axe's connection to the land
        MinDamage = Utility.RandomMinMax(40, 60);
        MaxDamage = Utility.RandomMinMax(75, 110);
        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 10;

        // Slayer effect – Earthsplinter is especially effective against Reptilian foes
        Slayer = SlayerName.ReptilianDeath;

        // Weapon attributes - focused on defensive and elemental control
        WeaponAttributes.HitLeechHits = 20;
        WeaponAttributes.HitLeechStam = 15;
        WeaponAttributes.BattleLust = 25;

        // Skill bonuses, reinforcing the elemental and combative nature of the axe
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(1, SkillName.Focus, 20.0);
        SkillBonuses.SetValues(2, SkillName.Lumberjacking, 10.0); // Given its connection to earth and the forest

        // Additional thematic bonus, reflecting the earth-shattering power of the axe
        SkillBonuses.SetValues(3, SkillName.Mining, 10.0);

        // Adding extra earth-based functionality
        Attributes.ReflectPhysical = 15;

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Earthsplinter(Serial serial) : base(serial)
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
