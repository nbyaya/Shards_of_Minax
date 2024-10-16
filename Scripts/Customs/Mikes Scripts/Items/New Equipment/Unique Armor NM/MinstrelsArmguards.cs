using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MinstrelsArmguards : RingmailArms
{
    [Constructable]
    public MinstrelsArmguards()
    {
        Name = "Minstrel's Armguards";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(45, 65);
        ArmorAttributes.SelfRepair = 12;
        ArmorAttributes.MageArmor = 1;
        Attributes.EnhancePotions = 25;
        Attributes.RegenMana = 3;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 45.0);
        SkillBonuses.SetValues(1, SkillName.Peacemaking, 40.0);
        PhysicalBonus = 10;
        EnergyBonus = 15;
        FireBonus = 15;
        ColdBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MinstrelsArmguards(Serial serial) : base(serial)
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
