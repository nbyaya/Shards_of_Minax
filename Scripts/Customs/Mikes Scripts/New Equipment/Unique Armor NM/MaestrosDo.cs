using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MaestrosDo : PlateDo
{
    [Constructable]
    public MaestrosDo()
    {
        Name = "Maestro's Do";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(65, 85);
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.AttackChance = 15;
        Attributes.BonusStr = 30;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 50.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 30.0);
        PhysicalBonus = 20;
        FireBonus = 15;
        ColdBonus = 15;
        EnergyBonus = 10;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MaestrosDo(Serial serial) : base(serial)
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
