using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HammerlordsChestplate : PlateChest
{
    [Constructable]
    public HammerlordsChestplate()
    {
        Name = "Hammerlord's Chestplate";
        Hue = Utility.Random(350, 650);
        BaseArmorRating = Utility.RandomMinMax(50, 85);
        AbsorptionAttributes.EaterFire = 10;
        ArmorAttributes.LowerStatReq = 15;
        Attributes.BonusHits = 20;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Tactics, 10.0);
        PhysicalBonus = 20;
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HammerlordsChestplate(Serial serial) : base(serial)
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
