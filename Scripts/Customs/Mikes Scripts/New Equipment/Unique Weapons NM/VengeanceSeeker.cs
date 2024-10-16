using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VengeanceSeeker : ExecutionersAxe
{
    [Constructable]
    public VengeanceSeeker()
    {
        Name = "Vengeance Seeker";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.ReflectPhysical = 15;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.Repond;
        Slayer2 = SlayerName.DragonSlaying;
        WeaponAttributes.HitPoisonArea = 35;
        WeaponAttributes.HitLeechStam = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VengeanceSeeker(Serial serial) : base(serial)
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
