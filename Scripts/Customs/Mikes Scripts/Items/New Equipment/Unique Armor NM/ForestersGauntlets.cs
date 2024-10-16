using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ForestersGauntlets : PlateGloves
{
    [Constructable]
    public ForestersGauntlets()
    {
        Name = "Forester's Gauntlets";
        Hue = Utility.Random(300, 400);
        BaseArmorRating = Utility.RandomMinMax(50, 70);
        ArmorAttributes.SelfRepair = 10;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusStr = 20;
        Attributes.BonusHits = 10;
        Attributes.AttackChance = 15;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 40.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        PhysicalBonus = 15;
        FireBonus = 20;
        ColdBonus = 15;
        EnergyBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ForestersGauntlets(Serial serial) : base(serial)
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
