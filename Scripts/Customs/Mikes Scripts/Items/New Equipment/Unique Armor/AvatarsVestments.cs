using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AvatarsVestments : PlateChest
{
    [Constructable]
    public AvatarsVestments()
    {
        Name = "Avatar's Vestments";
        Hue = Utility.Random(100, 300);
        BaseArmorRating = Utility.RandomMinMax(50, 80);
        AbsorptionAttributes.EaterFire = 15;
        ArmorAttributes.DurabilityBonus = 25;
        Attributes.BonusInt = 20;
        Attributes.SpellDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        ColdBonus = 15;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AvatarsVestments(Serial serial) : base(serial)
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
