using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GenjiBow : Bow
{
    [Constructable]
    public GenjiBow()
    {
        Name = "Genji Bow";
        Hue = Utility.Random(800, 22900);
        MinDamage = Utility.RandomMinMax(25, 65);
        MaxDamage = Utility.RandomMinMax(65, 105);
        Attributes.DefendChance = 10;
        Attributes.RegenHits = 5;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitPoisonArea = 30;
        WeaponAttributes.DurabilityBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GenjiBow(Serial serial) : base(serial)
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
