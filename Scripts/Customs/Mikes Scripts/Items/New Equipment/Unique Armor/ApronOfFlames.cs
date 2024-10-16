using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ApronOfFlames : StuddedChest
{
    [Constructable]
    public ApronOfFlames()
    {
        Name = "Apron of Flames";
        Hue = Utility.Random(300, 700);
        BaseArmorRating = Utility.RandomMinMax(30, 65);
        AbsorptionAttributes.EaterFire = 20;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.ReflectPhysical = 10;
        Attributes.CastRecovery = 1;
        SkillBonuses.SetValues(0, SkillName.Cooking, 15.0);
        FireBonus = 25;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ApronOfFlames(Serial serial) : base(serial)
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
