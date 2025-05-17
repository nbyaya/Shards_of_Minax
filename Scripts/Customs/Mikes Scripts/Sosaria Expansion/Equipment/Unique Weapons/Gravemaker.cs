using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Gravemaker : HeavyCrossbow
{
    [Constructable]
    public Gravemaker()
    {
        Name = "Gravemaker";
        Hue = Utility.Random(1150, 1300);  // A shadowy hue, resembling the ominous void.
        MinDamage = Utility.RandomMinMax(25, 40);
        MaxDamage = Utility.RandomMinMax(45, 70);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 10;

        // Elemental damage - Gravemaker inflicts additional damage through dark, cursed energy
        WeaponAttributes.HitEnergyArea = 50;
        WeaponAttributes.HitFireArea = 50;

        // Slayer - Gravemaker is especially effective against Dragonkind, striking fear into their hearts
        Slayer = SlayerName.DragonSlaying;

        // Skill Bonuses - Gravemaker is designed for sharp shooters, with a focus on archery, tactics, and animal lore
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Archery, 15.0);
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 10.0);

        // Thematic effect - The Gravemaker inflicts fear on its targets, diminishing their resistance to the user
        WeaponAttributes.HitLowerDefend = 25;
        WeaponAttributes.HitLowerAttack = 25;

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Gravemaker(Serial serial) : base(serial)
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
