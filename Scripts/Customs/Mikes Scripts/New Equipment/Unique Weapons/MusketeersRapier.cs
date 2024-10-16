using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MusketeersRapier : Kryss
{
    [Constructable]
    public MusketeersRapier()
    {
        Name = "Musketeer's Rapier";
        Hue = Utility.Random(400, 2600);
        MinDamage = Utility.RandomMinMax(15, 55);
        MaxDamage = Utility.RandomMinMax(55, 90);
        Attributes.BonusInt = 5;
        Attributes.NightSight = 1;
        Slayer = SlayerName.Repond;
        WeaponAttributes.HitDispel = 25;
        SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MusketeersRapier(Serial serial) : base(serial)
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
