using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AlmsmansAegis : BronzeShield
{
    [Constructable]
    public AlmsmansAegis()
    {
        Name = "Almsman's Aegis";
        Hue = Utility.Random(500, 700);
        BaseArmorRating = Utility.RandomMinMax(65, 85);
        ArmorAttributes.LowerStatReq = 40;
        Attributes.SpellChanneling = 1;
        Attributes.DefendChance = 20;
        SkillBonuses.SetValues(0, SkillName.Begging, 50.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 35.0);
        PhysicalBonus = 20;
        FireBonus = 20;
        ColdBonus = 10;
        EnergyBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AlmsmansAegis(Serial serial) : base(serial)
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
