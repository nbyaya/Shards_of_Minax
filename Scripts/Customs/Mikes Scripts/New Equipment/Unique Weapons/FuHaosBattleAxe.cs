using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FuHaosBattleAxe : BattleAxe
{
    [Constructable]
    public FuHaosBattleAxe()
    {
        Name = "Fu Hao's Battle Axe";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(25, 65);
        MaxDamage = Utility.RandomMinMax(65, 105);
        Attributes.BonusHits = 15;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitLeechHits = 25;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FuHaosBattleAxe(Serial serial) : base(serial)
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
