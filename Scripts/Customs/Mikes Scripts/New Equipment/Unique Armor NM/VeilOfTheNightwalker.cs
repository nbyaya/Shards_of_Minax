using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VeilOfTheNightwalker : LeatherNinjaJacket
{
    [Constructable]
    public VeilOfTheNightwalker()
    {
        Name = "Veil of the Nightwalker";
        Hue = Utility.Random(1200, 1300);
        BaseArmorRating = Utility.RandomMinMax(70, 90);
        ArmorAttributes.LowerStatReq = 50;
        ArmorAttributes.MageArmor = 1;
        Attributes.AttackChance = 15;
        Attributes.DefendChance = 15;
        Attributes.BonusStam = 20;
        SkillBonuses.SetValues(0, SkillName.Hiding, 40.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 40.0);
        SkillBonuses.SetValues(2, SkillName.Ninjitsu, 40.0);
        PhysicalBonus = 18;
        EnergyBonus = 25;
        FireBonus = 15;
        ColdBonus = 15;
        PoisonBonus = 18;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VeilOfTheNightwalker(Serial serial) : base(serial)
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
