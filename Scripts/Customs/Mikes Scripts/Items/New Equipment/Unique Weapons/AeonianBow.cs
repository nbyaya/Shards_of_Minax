using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AeonianBow : Bow
{
    [Constructable]
    public AeonianBow()
    {
        Name = "Aeonian Bow";
        Hue = Utility.Random(200, 2400);
        MinDamage = Utility.RandomMinMax(25, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.DefendChance = 8;
        Attributes.NightSight = 1;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitFireball = 40;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AeonianBow(Serial serial) : base(serial)
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
