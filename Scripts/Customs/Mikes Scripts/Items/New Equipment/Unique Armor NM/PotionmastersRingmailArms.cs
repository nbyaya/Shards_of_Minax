using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PotionmastersRingmailArms : RingmailArms
{
    [Constructable]
    public PotionmastersRingmailArms()
    {
        Name = "Potionmaster's RingmailArms";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(45, 65);
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusHits = 50;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 50.0);
        SkillBonuses.SetValues(1, SkillName.ItemID, 30.0);
        PhysicalBonus = 12;
        FireBonus = 8;
        ColdBonus = 8;
        EnergyBonus = 12;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PotionmastersRingmailArms(Serial serial) : base(serial)
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
