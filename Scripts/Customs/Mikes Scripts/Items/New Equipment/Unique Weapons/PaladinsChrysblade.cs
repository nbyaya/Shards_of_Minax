using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PaladinsChrysblade : TwoHandedAxe
{
    [Constructable]
    public PaladinsChrysblade()
    {
        Name = "Paladin's Chrysblade";
        Hue = Utility.Random(50, 2100);
        MinDamage = Utility.RandomMinMax(40, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.BonusHits = 20;
        Attributes.ReflectPhysical = 10;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitDispel = 90;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
		SkillBonuses.SetValues(1, SkillName.MagicResist, 50.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PaladinsChrysblade(Serial serial) : base(serial)
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
