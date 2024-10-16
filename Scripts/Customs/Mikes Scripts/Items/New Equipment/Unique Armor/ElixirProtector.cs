using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ElixirProtector : PlateChest
{
    [Constructable]
    public ElixirProtector()
    {
        Name = "Elixir Protector";
        Hue = Utility.Random(1, 500);
        BaseArmorRating = Utility.RandomMinMax(60, 85);
        AbsorptionAttributes.EaterPoison = 30;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusHits = 50;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
        ColdBonus = 20;
        EnergyBonus = 15;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 25;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ElixirProtector(Serial serial) : base(serial)
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
