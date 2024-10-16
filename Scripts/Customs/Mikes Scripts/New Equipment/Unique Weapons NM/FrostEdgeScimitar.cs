using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostEdgeScimitar : Scimitar
{
    [Constructable]
    public FrostEdgeScimitar()
    {
        Name = "Frost Edge Scimitar";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.WeaponDamage = 40;
        Slayer = SlayerName.WaterDissipation;
        WeaponAttributes.HitColdArea = 45;
        WeaponAttributes.ResistColdBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Parry, 15.0);
        SkillBonuses.SetValues(1, SkillName.Swords, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostEdgeScimitar(Serial serial) : base(serial)
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
