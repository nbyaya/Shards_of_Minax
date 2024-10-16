using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostbiteEdge : VikingSword
{
    [Constructable]
    public FrostbiteEdge()
    {
        Name = "Frostbite Edge";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.FlameDousing;
        Slayer2 = SlayerName.WaterDissipation;
        WeaponAttributes.HitColdArea = 45;
        WeaponAttributes.HitDispel = 30;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostbiteEdge(Serial serial) : base(serial)
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
