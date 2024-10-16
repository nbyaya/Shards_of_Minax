using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GuardiansHelm : NorseHelm
{
    [Constructable]
    public GuardiansHelm()
    {
        Name = "Guardian's Helm";
        Hue = Utility.Random(500, 950);
        BaseArmorRating = Utility.RandomMinMax(45, 75);
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusStr = 20;
        Attributes.DefendChance = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        PhysicalBonus = 30;
        EnergyBonus = 10;
        FireBonus = 10;
        ColdBonus = 5;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GuardiansHelm(Serial serial) : base(serial)
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
