using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VenomsSting : PoisonBlade
{
    [Constructable]
    public VenomsSting()
    {
        Name = "Venom's Sting";
        Hue = Utility.Random(300, 2400);
        MinDamage = Utility.RandomMinMax(10, 40);
        MaxDamage = Utility.RandomMinMax(40, 70);
        Attributes.LowerManaCost = 10;
        Attributes.WeaponSpeed = 10;
        WeaponAttributes.HitPoisonArea = 30;
        WeaponAttributes.BattleLust = 15;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VenomsSting(Serial serial) : base(serial)
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
