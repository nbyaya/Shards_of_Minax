using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DestructoDiscDagger : Dagger
{
    [Constructable]
    public DestructoDiscDagger()
    {
        Name = "Destructo Disc Dagger";
        Hue = Utility.Random(500, 2550);
        MinDamage = Utility.RandomMinMax(10, 50);
        MaxDamage = Utility.RandomMinMax(50, 90);
        Attributes.AttackChance = 15;
        Attributes.SpellDamage = 10;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitLightning = 20;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DestructoDiscDagger(Serial serial) : base(serial)
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
