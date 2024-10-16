using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WondershotCrossbow : HeavyCrossbow
{
    [Constructable]
    public WondershotCrossbow()
    {
        Name = "Wondershot Crossbow";
        Hue = Utility.Random(300, 2600);
        MinDamage = Utility.RandomMinMax(10, 50);
        MaxDamage = Utility.RandomMinMax(100, 150);
        Attributes.Luck = 200;
        Attributes.ReflectPhysical = 10;
        Slayer = SlayerName.BalronDamnation;
        WeaponAttributes.SelfRepair = 3;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 40.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WondershotCrossbow(Serial serial) : base(serial)
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
