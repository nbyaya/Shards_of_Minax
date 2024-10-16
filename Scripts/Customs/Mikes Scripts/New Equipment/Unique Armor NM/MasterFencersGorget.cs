using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MasterFencersGorget : PlateGorget
{
    [Constructable]
    public MasterFencersGorget()
    {
        Name = "Master Fencer's Gorget";
        Hue = Utility.Random(100, 700);
        BaseArmorRating = Utility.RandomMinMax(60, 70);
        ArmorAttributes.SelfRepair = 10;
        ArmorAttributes.MageArmor = 1;
        Attributes.AttackChance = 20;
        Attributes.DefendChance = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 30.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 50.0);
        PhysicalBonus = 12;
        FireBonus = 12;
        ColdBonus = 12;
        EnergyBonus = 12;
        PoisonBonus = 12;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MasterFencersGorget(Serial serial) : base(serial)
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
