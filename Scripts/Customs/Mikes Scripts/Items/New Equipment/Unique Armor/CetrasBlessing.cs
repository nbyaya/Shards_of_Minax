using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CetrasBlessing : PlateGorget
{
    [Constructable]
    public CetrasBlessing()
    {
        Name = "Cetra's Blessing";
        Hue = Utility.Random(400, 800);
        BaseArmorRating = Utility.RandomMinMax(40, 80);
        AbsorptionAttributes.ResonancePoison = 20;
        ArmorAttributes.MageArmor = 1;
        Attributes.RegenHits = 10;
        Attributes.RegenMana = 5;
        SkillBonuses.SetValues(0, SkillName.Healing, 20.0);
        ColdBonus = 15;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 20;
        EnergyBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CetrasBlessing(Serial serial) : base(serial)
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
