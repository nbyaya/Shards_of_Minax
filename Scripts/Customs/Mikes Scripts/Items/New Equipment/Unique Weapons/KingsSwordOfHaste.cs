using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KingsSwordOfHaste : Longsword
{
    [Constructable]
    public KingsSwordOfHaste()
    {
        Name = "King's Sword of Haste";
        Hue = Utility.Random(550, 2900);
        MinDamage = Utility.RandomMinMax(25, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.BonusStr = 20;
        Attributes.WeaponSpeed = 10;
        WeaponAttributes.SelfRepair = 3;
        WeaponAttributes.HitLightning = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KingsSwordOfHaste(Serial serial) : base(serial)
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
