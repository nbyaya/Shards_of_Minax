using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FootpadsArms : RingmailArms
{
    [Constructable]
    public FootpadsArms()
    {
        Name = "Footpad's Arms";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(40, 70);
        ArmorAttributes.DurabilityBonus = 80;
        Attributes.WeaponSpeed = 20;
        Attributes.WeaponDamage = 25;
        SkillBonuses.SetValues(0, SkillName.Stealing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Snooping, 40.0);
        SkillBonuses.SetValues(2, SkillName.RemoveTrap, 30.0);
        PhysicalBonus = 20;
        FireBonus = 10;
        ColdBonus = 20;
        EnergyBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FootpadsArms(Serial serial) : base(serial)
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
