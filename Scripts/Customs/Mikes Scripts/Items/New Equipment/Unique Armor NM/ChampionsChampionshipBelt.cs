using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChampionsChampionshipBelt : LeatherChest
{
    [Constructable]
    public ChampionsChampionshipBelt()
    {
        Name = "Champion's Championship Belt";
        Hue = Utility.Random(10, 500);
        BaseArmorRating = Utility.RandomMinMax(50, 70);
        ArmorAttributes.LowerStatReq = 40;
        Attributes.Luck = 200;
        Attributes.BonusHits = 30;
        Attributes.EnhancePotions = 25;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        PhysicalBonus = 20;
        ColdBonus = 20;
        FireBonus = 20;
        EnergyBonus = 20;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChampionsChampionshipBelt(Serial serial) : base(serial)
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
