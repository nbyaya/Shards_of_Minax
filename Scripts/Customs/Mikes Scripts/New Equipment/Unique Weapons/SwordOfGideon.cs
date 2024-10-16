using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SwordOfGideon : Longsword
{
    [Constructable]
    public SwordOfGideon()
    {
        Name = "Sword of Gideon";
        Hue = Utility.Random(300, 2500);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 100);
        Attributes.AttackChance = 10;
        Attributes.NightSight = 1;
        Slayer = SlayerName.Repond;
        WeaponAttributes.HitLightning = 55;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SwordOfGideon(Serial serial) : base(serial)
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
