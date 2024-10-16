using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MonksSoulGloves : BoneGloves
{
    [Constructable]
    public MonksSoulGloves()
    {
        Name = "Monk's Soul Gloves";
        Hue = Utility.Random(100, 300);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        Attributes.BonusStam = 20;
        Attributes.AttackChance = 15;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 15.0);
        SkillBonuses.SetValues(1, SkillName.Macing, 10.0);
        ColdBonus = 5;
        EnergyBonus = 5;
        FireBonus = 15;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MonksSoulGloves(Serial serial) : base(serial)
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
