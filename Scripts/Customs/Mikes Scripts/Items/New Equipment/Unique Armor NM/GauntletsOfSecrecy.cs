using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GauntletsOfSecrecy : LeatherGloves
{
    [Constructable]
    public GauntletsOfSecrecy()
    {
        Name = "Gauntlets of Secrecy";
        Hue = Utility.Random(1501, 1600);
        BaseArmorRating = Utility.RandomMinMax(50, 70);
        ArmorAttributes.LowerStatReq = 50;
        Attributes.BonusDex = 20;
        Attributes.RegenStam = 8;
        SkillBonuses.SetValues(0, SkillName.Hiding, 50.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 40.0);
        SkillBonuses.SetValues(2, SkillName.Ninjitsu, 40.0);
        PhysicalBonus = 15;
        ColdBonus = 15;
        FireBonus = 10;
        EnergyBonus = 25;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GauntletsOfSecrecy(Serial serial) : base(serial)
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
