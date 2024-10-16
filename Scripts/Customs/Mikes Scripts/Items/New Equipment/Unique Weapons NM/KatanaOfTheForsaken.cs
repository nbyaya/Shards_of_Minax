using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KatanaOfTheForsaken : Katana
{
    [Constructable]
    public KatanaOfTheForsaken()
    {
        Name = "Katana of the Forsaken";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.BonusHits = 15;
        Attributes.WeaponSpeed = 30;
        Slayer = SlayerName.Repond;
        WeaponAttributes.HitHarm = 35;
        WeaponAttributes.HitLowerDefend = 25;
        SkillBonuses.SetValues(0, SkillName.Ninjitsu, 20.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KatanaOfTheForsaken(Serial serial) : base(serial)
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
