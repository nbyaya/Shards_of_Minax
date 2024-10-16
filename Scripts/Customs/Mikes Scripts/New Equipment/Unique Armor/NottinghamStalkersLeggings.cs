using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NottinghamStalkersLeggings : LeatherLegs
{
    [Constructable]
    public NottinghamStalkersLeggings()
    {
        Name = "Nottingham Stalker's Leggings";
        Hue = Utility.Random(250, 550);
        BaseArmorRating = Utility.RandomMinMax(30, 50);
        ArmorAttributes.MageArmor = 1;
        Attributes.RegenStam = 5;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        PhysicalBonus = 15;
        ColdBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NottinghamStalkersLeggings(Serial serial) : base(serial)
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
