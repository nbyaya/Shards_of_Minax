using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class JestersMischievousBuckler : Buckler
{
    [Constructable]
    public JestersMischievousBuckler()
    {
        Name = "Jester's Mischievous Buckler";
        Hue = Utility.Random(500, 800);
        BaseArmorRating = Utility.RandomMinMax(25, 65);
        AbsorptionAttributes.EaterFire = 25;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.ReflectPhysical = 20;
        Attributes.AttackChance = 15;
        SkillBonuses.SetValues(0, SkillName.Parry, 25.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public JestersMischievousBuckler(Serial serial) : base(serial)
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
