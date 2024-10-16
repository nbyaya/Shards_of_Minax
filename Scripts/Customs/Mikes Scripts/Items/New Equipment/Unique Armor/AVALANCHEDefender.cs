using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AVALANCHEDefender : LeatherChest
{
    [Constructable]
    public AVALANCHEDefender()
    {
        Name = "AVALANCHE Defender";
        Hue = Utility.Random(300, 700);
        BaseArmorRating = Utility.RandomMinMax(40, 75);
        AbsorptionAttributes.EaterFire = 20;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusHits = 30;
        Attributes.BonusStr = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        FireBonus = 20;
        PhysicalBonus = 15;
        EnergyBonus = 10;
        PoisonBonus = 10;
        ColdBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AVALANCHEDefender(Serial serial) : base(serial)
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
