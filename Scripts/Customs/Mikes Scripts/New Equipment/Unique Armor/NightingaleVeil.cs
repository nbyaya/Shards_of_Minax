using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NightingaleVeil : LeatherGorget
{
    [Constructable]
    public NightingaleVeil()
    {
        Name = "Nightingale Veil";
        Hue = Utility.Random(1, 250);
        BaseArmorRating = Utility.RandomMinMax(25, 60);
        ArmorAttributes.LowerStatReq = 15;
        Attributes.BonusDex = 15;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        SkillBonuses.SetValues(1, SkillName.Archery, 10.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 15;
        FireBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NightingaleVeil(Serial serial) : base(serial)
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
