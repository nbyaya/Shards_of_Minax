using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AlchemistsVisionHelmet : Helmet
{
    [Constructable]
    public AlchemistsVisionHelmet()
    {
        Name = "Alchemist's Vision Helmet";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(60, 75);
        ArmorAttributes.LowerStatReq = 50;
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusInt = 25;
        Attributes.RegenMana = 5;
        Attributes.Luck = 200;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 50.0);
        SkillBonuses.SetValues(1, SkillName.TasteID, 30.0);
        PhysicalBonus = 10;
        FireBonus = 10;
        ColdBonus = 10;
        EnergyBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AlchemistsVisionHelmet(Serial serial) : base(serial)
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
