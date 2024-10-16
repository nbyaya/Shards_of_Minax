using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VirtuososGreaves : PlateLegs
{
    [Constructable]
    public VirtuososGreaves()
    {
        Name = "Virtuoso's Greaves";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(60, 80);
        ArmorAttributes.LowerStatReq = 50;
        Attributes.BonusStam = 25;
        Attributes.Luck = 150;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 50.0);
        SkillBonuses.SetValues(1, SkillName.Discordance, 40.0);
        PhysicalBonus = 15;
        ColdBonus = 15;
        FireBonus = 15;
        EnergyBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VirtuososGreaves(Serial serial) : base(serial)
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
