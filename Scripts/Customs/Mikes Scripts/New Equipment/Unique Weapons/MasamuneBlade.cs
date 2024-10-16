using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MasamuneBlade : VikingSword
{
    [Constructable]
    public MasamuneBlade()
    {
        Name = "Masamune Blade";
        Hue = Utility.Random(600, 2800);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(80, 130);
        Attributes.BonusHits = 10;
        Attributes.ReflectPhysical = 5;
        Slayer = SlayerName.Exorcism;
        WeaponAttributes.HitManaDrain = 15;
        WeaponAttributes.ResistColdBonus = 10;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MasamuneBlade(Serial serial) : base(serial)
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
