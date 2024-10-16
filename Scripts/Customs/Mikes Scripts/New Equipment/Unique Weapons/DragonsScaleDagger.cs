using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DragonsScaleDagger : Dagger
{
    [Constructable]
    public DragonsScaleDagger()
    {
        Name = "Dragon's Scale Dagger";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(10, 50);
        MaxDamage = Utility.RandomMinMax(50, 90);
        Attributes.ReflectPhysical = 5;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitLeechHits = 20;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DragonsScaleDagger(Serial serial) : base(serial)
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
