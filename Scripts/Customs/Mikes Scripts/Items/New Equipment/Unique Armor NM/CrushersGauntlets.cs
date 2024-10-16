using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CrushersGauntlets : PlateGloves
{
    [Constructable]
    public CrushersGauntlets()
    {
        Name = "Crusher's Gauntlets";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(50, 85);
        ArmorAttributes.SelfRepair = 15;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusStr = 25;
        Attributes.WeaponDamage = 50;
        SkillBonuses.SetValues(0, SkillName.Macing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 35.0);
        PhysicalBonus = 18;
        FireBonus = 5;
        ColdBonus = 10;
        EnergyBonus = 10;
        PoisonBonus = 7;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CrushersGauntlets(Serial serial) : base(serial)
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
