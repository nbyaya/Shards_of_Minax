using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CherubsBlade : Scimitar
{
    [Constructable]
    public CherubsBlade()
    {
        Name = "Cherub's Blade";
        Hue = Utility.Random(550, 2750);
        MinDamage = Utility.RandomMinMax(25, 65);
        MaxDamage = Utility.RandomMinMax(65, 105);
        Attributes.SpellDamage = 10;
        Attributes.ReflectPhysical = 5;
        Slayer = SlayerName.Exorcism;
        WeaponAttributes.HitFireball = 60;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CherubsBlade(Serial serial) : base(serial)
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
