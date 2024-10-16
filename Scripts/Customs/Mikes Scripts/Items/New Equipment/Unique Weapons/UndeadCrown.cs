using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class UndeadCrown : WarMace
{
    [Constructable]
    public UndeadCrown()
    {
        Name = "The Undead Crown";
        Hue = Utility.Random(650, 2900);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(50, 80);
        Attributes.BonusInt = 15;
        WeaponAttributes.HitLeechHits = 25;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public UndeadCrown(Serial serial) : base(serial)
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
