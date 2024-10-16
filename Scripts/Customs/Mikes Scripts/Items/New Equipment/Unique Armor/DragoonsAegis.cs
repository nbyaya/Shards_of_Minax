using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DragoonsAegis : PlateChest
{
    [Constructable]
    public DragoonsAegis()
    {
        Name = "Dragoon's Aegis";
        Hue = Utility.Random(450, 650);
        BaseArmorRating = Utility.RandomMinMax(50, 85);
        ArmorAttributes.LowerStatReq = 10;
        Attributes.BonusStr = 20;
        Attributes.AttackChance = 10;
        Attributes.ReflectPhysical = 5;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 25;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DragoonsAegis(Serial serial) : base(serial)
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
