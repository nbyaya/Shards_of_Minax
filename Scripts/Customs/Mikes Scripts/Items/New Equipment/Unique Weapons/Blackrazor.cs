using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Blackrazor : Scalpel
{
    [Constructable]
    public Blackrazor()
    {
        Name = "Blackrazor";
        Hue = Utility.Random(800, 2900);
        MinDamage = Utility.RandomMinMax(25, 75);
        MaxDamage = Utility.RandomMinMax(75, 105);
        Attributes.BonusHits = 20;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.BloodDrinking;
        WeaponAttributes.HitLeechHits = 50;
        WeaponAttributes.HitManaDrain = 25;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Blackrazor(Serial serial) : base(serial)
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
