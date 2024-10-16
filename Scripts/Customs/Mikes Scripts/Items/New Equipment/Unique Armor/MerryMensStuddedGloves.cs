using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MerryMensStuddedGloves : StuddedGloves
{
    [Constructable]
    public MerryMensStuddedGloves()
    {
        Name = "Merry Men's Studded Gloves";
        Hue = Utility.Random(250, 550);
        BaseArmorRating = Utility.RandomMinMax(25, 45);
        ArmorAttributes.LowerStatReq = 15;
        Attributes.BonusStr = 10;
        Attributes.WeaponSpeed = 10;
        SkillBonuses.SetValues(0, SkillName.Stealing, 20.0);
        PhysicalBonus = 10;
        EnergyBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MerryMensStuddedGloves(Serial serial) : base(serial)
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
