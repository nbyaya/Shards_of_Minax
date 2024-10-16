using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PiledriversPauldrons : PlateArms
{
    [Constructable]
    public PiledriversPauldrons()
    {
        Name = "Piledriver's Pauldrons";
        Hue = Utility.Random(400, 900);
        BaseArmorRating = Utility.RandomMinMax(55, 75);
        ArmorAttributes.DurabilityBonus = 75;
        ArmorAttributes.SelfRepair = 10;
        Attributes.AttackChance = 25;
        Attributes.BonusStr = 20;
        Attributes.WeaponSpeed = 10;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 45.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        PhysicalBonus = 15;
        FireBonus = 20;
        ColdBonus = 15;
        EnergyBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PiledriversPauldrons(Serial serial) : base(serial)
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
