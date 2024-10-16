using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Doomsickle : Scimitar
{
    [Constructable]
    public Doomsickle()
    {
        Name = "Doomsickle";
        Hue = Utility.Random(600, 2900);
        MinDamage = Utility.RandomMinMax(25, 75);
        MaxDamage = Utility.RandomMinMax(75, 115);
        Attributes.SpellDamage = 10;
        Attributes.BonusHits = 10;
        Slayer = SlayerName.BalronDamnation;
        WeaponAttributes.BloodDrinker = 30;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Doomsickle(Serial serial) : base(serial)
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
