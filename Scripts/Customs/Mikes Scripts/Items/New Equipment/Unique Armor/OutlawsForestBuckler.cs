using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class OutlawsForestBuckler : Buckler
{
    [Constructable]
    public OutlawsForestBuckler()
    {
        Name = "Outlaw's Forest Buckler";
        Hue = Utility.Random(250, 550);
        BaseArmorRating = Utility.RandomMinMax(20, 40);
        ArmorAttributes.LowerStatReq = 10;
        Attributes.ReflectPhysical = 10;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        PhysicalBonus = 15;
        ColdBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public OutlawsForestBuckler(Serial serial) : base(serial)
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
