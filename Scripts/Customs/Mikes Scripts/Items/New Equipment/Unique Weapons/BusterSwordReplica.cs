using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BusterSwordReplica : TwoHandedAxe
{
    [Constructable]
    public BusterSwordReplica()
    {
        Name = "Buster Sword Replica";
        Hue = Utility.Random(500, 2900);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(70, 120);
        Attributes.BonusStr = 20;
        Attributes.AttackChance = 5;
        WeaponAttributes.HitLightning = 20;
        WeaponAttributes.HitHarm = 10;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BusterSwordReplica(Serial serial) : base(serial)
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
