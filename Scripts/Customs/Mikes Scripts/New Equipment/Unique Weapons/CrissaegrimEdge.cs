using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CrissaegrimEdge : Scimitar
{
    [Constructable]
    public CrissaegrimEdge()
    {
        Name = "Crissaegrim Edge";
        Hue = Utility.Random(250, 2300);
        MinDamage = Utility.RandomMinMax(35, 70);
        MaxDamage = Utility.RandomMinMax(70, 105);
        Attributes.BonusDex = 15;
        Attributes.WeaponSpeed = 10;
        Slayer = SlayerName.Fey;
        Slayer2 = SlayerName.Repond;
        WeaponAttributes.BattleLust = 25;
        WeaponAttributes.HitLightning = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CrissaegrimEdge(Serial serial) : base(serial)
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
