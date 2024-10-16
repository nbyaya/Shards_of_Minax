using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VipersStrike : HeavyCrossbow
{
    [Constructable]
    public VipersStrike()
    {
        Name = "Viper's Strike";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.AttackChance = 25;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.ReptilianDeath;
        Slayer2 = SlayerName.SnakesBane;
        WeaponAttributes.HitPoisonArea = 45;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Poisoning, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VipersStrike(Serial serial) : base(serial)
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
