using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BeowulfsWarAxe : WarAxe
{
    [Constructable]
    public BeowulfsWarAxe()
    {
        Name = "Beowulf's WarAxe";
        Hue = Utility.Random(350, 2550);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.BonusHits = 20;
        Attributes.ReflectPhysical = 10;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitLeechHits = 70;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BeowulfsWarAxe(Serial serial) : base(serial)
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
