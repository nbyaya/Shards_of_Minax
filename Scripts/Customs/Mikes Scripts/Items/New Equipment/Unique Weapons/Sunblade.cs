using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Sunblade : Scimitar
{
    [Constructable]
    public Sunblade()
    {
        Name = "Sunblade";
        Hue = Utility.Random(100, 2200);
        MinDamage = Utility.RandomMinMax(20, 70);
        MaxDamage = Utility.RandomMinMax(70, 100);
        Attributes.Luck = 50;
        Attributes.ReflectPhysical = 10;
        Slayer = SlayerName.Exorcism;
        WeaponAttributes.HitLeechMana = 50;
        WeaponAttributes.ResistColdBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Sunblade(Serial serial) : base(serial)
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
