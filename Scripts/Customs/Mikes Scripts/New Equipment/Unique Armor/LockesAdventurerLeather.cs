using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LockesAdventurerLeather : LeatherChest
{
    [Constructable]
    public LockesAdventurerLeather()
    {
        Name = "Locke's Adventurer Leather";
        Hue = Utility.Random(200, 700);
        BaseArmorRating = Utility.RandomMinMax(30, 65);
        AbsorptionAttributes.EaterFire = 15;
        Attributes.BonusDex = 20;
        Attributes.DefendChance = 10;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LockesAdventurerLeather(Serial serial) : base(serial)
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
