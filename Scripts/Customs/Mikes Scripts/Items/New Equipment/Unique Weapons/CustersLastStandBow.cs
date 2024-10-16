using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CustersLastStandBow : Bow
{
    [Constructable]
    public CustersLastStandBow()
    {
        Name = "Custer's Last Stand Bow";
        Hue = Utility.Random(350, 2550);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.AttackChance = 5;
        Attributes.Luck = 100;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitFireball = 25;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CustersLastStandBow(Serial serial) : base(serial)
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
