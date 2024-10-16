using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VATSEnhancedDagger : Dagger
{
    [Constructable]
    public VATSEnhancedDagger()
    {
        Name = "VATS Enhanced Dagger";
        Hue = Utility.Random(300, 2500);
        MinDamage = Utility.RandomMinMax(10, 45);
        MaxDamage = Utility.RandomMinMax(45, 80);
        Attributes.AttackChance = 20;
        Attributes.WeaponSpeed = 10;
        Slayer = SlayerName.ReptilianDeath;
        WeaponAttributes.HitLeechMana = 10;
        WeaponAttributes.HitMagicArrow = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VATSEnhancedDagger(Serial serial) : base(serial)
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
