using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RiotDefendersShield : HeaterShield
{
    [Constructable]
    public RiotDefendersShield()
    {
        Name = "Riot Defender's Shield";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(30, 75);
        AbsorptionAttributes.EaterPoison = 20;
        ArmorAttributes.MageArmor = 1;
        Attributes.AttackChance = 15;
        Attributes.BonusStam = 20;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 15;
        PhysicalBonus = 25;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RiotDefendersShield(Serial serial) : base(serial)
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
