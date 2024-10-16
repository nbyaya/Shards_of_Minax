using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SamuraisDestinyHauberk : PlateChest
{
    [Constructable]
    public SamuraisDestinyHauberk()
    {
        Name = "Samurai's Destiny Hauberk";
        Hue = Utility.Random(300, 800);
        BaseArmorRating = Utility.RandomMinMax(70, 100);
        AbsorptionAttributes.EaterKinetic = 40;
        ArmorAttributes.DurabilityBonus = 100;
        ArmorAttributes.LowerStatReq = 50;
        Attributes.BonusStr = 30;
        Attributes.BonusStam = 30;
        SkillBonuses.SetValues(0, SkillName.Bushido, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 40.0);
        PhysicalBonus = 20;
        FireBonus = 15;
        ColdBonus = 10;
        EnergyBonus = 25;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SamuraisDestinyHauberk(Serial serial) : base(serial)
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
