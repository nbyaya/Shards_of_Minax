using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RielsRebellionSabre : Scimitar
{
    [Constructable]
    public RielsRebellionSabre()
    {
        Name = "Riel's Rebellion Sabre";
        Hue = Utility.Random(400, 2600);
        MinDamage = Utility.RandomMinMax(25, 65);
        MaxDamage = Utility.RandomMinMax(65, 90);
        Attributes.BonusHits = 10;
        Attributes.RegenStam = 5;
        Slayer = SlayerName.Repond;
        WeaponAttributes.HitFireball = 40;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0); // or similar skill
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RielsRebellionSabre(Serial serial) : base(serial)
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
