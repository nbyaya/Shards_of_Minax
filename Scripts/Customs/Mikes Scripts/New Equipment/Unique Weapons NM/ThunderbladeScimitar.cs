using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThunderbladeScimitar : Scimitar
{
    [Constructable]
    public ThunderbladeScimitar()
    {
        Name = "Thunderblade Scimitar";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.AttackChance = 20;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.Vacuum;
        WeaponAttributes.HitLightning = 55;
        WeaponAttributes.HitManaDrain = 25;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThunderbladeScimitar(Serial serial) : base(serial)
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
