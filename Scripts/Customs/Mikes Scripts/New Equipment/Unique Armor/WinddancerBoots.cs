using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WinddancerBoots : LeatherLegs
{
    [Constructable]
    public WinddancerBoots()
    {
        Name = "Winddancer Boots";
        Hue = Utility.Random(500, 1000);
        BaseArmorRating = Utility.RandomMinMax(25, 70);
        Attributes.DefendChance = 40;
        Attributes.AttackChance = -20;
        Attributes.RegenStam = 10;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WinddancerBoots(Serial serial) : base(serial)
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
