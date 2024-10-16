using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhiteMagesDivineVestment : LeatherChest
{
    [Constructable]
    public WhiteMagesDivineVestment()
    {
        Name = "WhiteMage's Divine Vestment";
        Hue = Utility.Random(900, 1000);
        BaseArmorRating = Utility.RandomMinMax(25, 60);
        AbsorptionAttributes.EaterPoison = 15;
        ArmorAttributes.MageArmor = 1;
        Attributes.RegenHits = 10;
        SkillBonuses.SetValues(0, SkillName.Healing, 25.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 20;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhiteMagesDivineVestment(Serial serial) : base(serial)
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
