using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WardensAegis : ChainChest
{
    [Constructable]
    public WardensAegis()
    {
        Name = "Warden's Aegis";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterFire = 25;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusHits = 60;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        ColdBonus = 5;
        EnergyBonus = 15;
        FireBonus = 20;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WardensAegis(Serial serial) : base(serial)
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
