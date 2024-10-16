using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ElixirsGraspGloves : LeatherGloves
{
    [Constructable]
    public ElixirsGraspGloves()
    {
        Name = "Elixir's Grasp Gloves";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(20, 50);
        ArmorAttributes.LowerStatReq = 40;
        Attributes.CastSpeed = 1;
        Attributes.BonusDex = 30;
        Attributes.EnhancePotions = 50;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 50.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 30.0);
        PhysicalBonus = 8;
        EnergyBonus = 12;
        FireBonus = 8;
        ColdBonus = 12;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ElixirsGraspGloves(Serial serial) : base(serial)
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
