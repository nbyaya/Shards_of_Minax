using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShavronnesRapier : Kryss
{
    [Constructable]
    public ShavronnesRapier()
    {
        Name = "Shavronne's Rapier";
        Hue = Utility.Random(500, 2900);
        MinDamage = Utility.RandomMinMax(10, 40);
        MaxDamage = Utility.RandomMinMax(40, 60);
        Attributes.BonusMana = 20;
        Attributes.SpellDamage = 10;
        Slayer = SlayerName.ElementalHealth;
        WeaponAttributes.HitLeechMana = 30;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShavronnesRapier(Serial serial) : base(serial)
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
