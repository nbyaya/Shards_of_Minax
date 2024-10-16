using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MonksBattleWraps : BoneGloves
{
    [Constructable]
    public MonksBattleWraps()
    {
        Name = "Monk's Battle Wraps";
        Hue = Utility.Random(300, 650);
        BaseArmorRating = Utility.RandomMinMax(35, 70);
        AbsorptionAttributes.EaterKinetic = 20;
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusStr = 15;
        Attributes.WeaponSpeed = 10;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 25.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MonksBattleWraps(Serial serial) : base(serial)
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
