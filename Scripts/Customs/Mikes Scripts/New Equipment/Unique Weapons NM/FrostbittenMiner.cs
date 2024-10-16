using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostbittenMiner : HammerPick
{
    [Constructable]
    public FrostbittenMiner()
    {
        Name = "Frostbitten Miner";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 25;
        Attributes.AttackChance = 20;
        Attributes.CastSpeed = -1;
        Slayer = SlayerName.TrollSlaughter;
        Slayer2 = SlayerName.WaterDissipation;
        WeaponAttributes.HitColdArea = 35;
        WeaponAttributes.HitFatigue = 30;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 15.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostbittenMiner(Serial serial) : base(serial)
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
