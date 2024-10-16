using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ArchersStuddedLeggingsOfAgility : StuddedLegs
{
    [Constructable]
    public ArchersStuddedLeggingsOfAgility()
    {
        Name = "Archer's Studded Leggings of Agility";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(65, 90);
        ArmorAttributes.LowerStatReq = 30;
        Attributes.BonusDex = 30;
        Attributes.ReflectPhysical = 15;
        Attributes.EnhancePotions = 25;
        SkillBonuses.SetValues(0, SkillName.Archery, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 40.0);
        PhysicalBonus = 12;
        EnergyBonus = 18;
        FireBonus = 12;
        ColdBonus = 18;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ArchersStuddedLeggingsOfAgility(Serial serial) : base(serial)
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
