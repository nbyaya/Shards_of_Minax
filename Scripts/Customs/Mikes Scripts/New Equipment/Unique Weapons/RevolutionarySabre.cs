using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RevolutionarySabre : Scimitar
{
    [Constructable]
    public RevolutionarySabre()
    {
        Name = "Revolutionary Sabre";
        Hue = Utility.Random(100, 2300);
        MinDamage = Utility.RandomMinMax(25, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.BonusHits = 15;
        Attributes.DefendChance = 5;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitLightning = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RevolutionarySabre(Serial serial) : base(serial)
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
