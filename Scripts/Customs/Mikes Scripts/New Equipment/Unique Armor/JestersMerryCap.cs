using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class JestersMerryCap : LeatherCap
{
    [Constructable]
    public JestersMerryCap()
    {
        Name = "Jester's Merry Cap";
        Hue = Utility.Random(500, 800);
        BaseArmorRating = Utility.RandomMinMax(15, 50);
        AbsorptionAttributes.EaterEnergy = 15;
        ArmorAttributes.LowerStatReq = 10;
        Attributes.Luck = 70;
        Attributes.ReflectPhysical = 15;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        SkillBonuses.SetValues(1, SkillName.Discordance, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public JestersMerryCap(Serial serial) : base(serial)
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
