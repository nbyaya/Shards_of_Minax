using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarvestersBlessing : SkinningKnife
{
    [Constructable]
    public HarvestersBlessing()
    {
        Name = "Harvester's Blessing";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 100);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.Luck = 100;
        Attributes.DefendChance = 20;
        Slayer = SlayerName.Ophidian;
        Slayer2 = SlayerName.ArachnidDoom;
        WeaponAttributes.HitLeechHits = 30;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 25.0);
        SkillBonuses.SetValues(1, SkillName.Mining, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarvestersBlessing(Serial serial) : base(serial)
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
