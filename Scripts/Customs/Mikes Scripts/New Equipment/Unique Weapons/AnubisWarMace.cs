using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AnubisWarMace : WarMace
{
    [Constructable]
    public AnubisWarMace()
    {
        Name = "Anubis WarMace";
        Hue = Utility.Random(500, 2700);
        MinDamage = Utility.RandomMinMax(25, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.BonusStr = 10;
        Attributes.RegenHits = 3;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.BalronDamnation;
        WeaponAttributes.HitHarm = 20;
        WeaponAttributes.BloodDrinker = 15;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AnubisWarMace(Serial serial) : base(serial)
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
