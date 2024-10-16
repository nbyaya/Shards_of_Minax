using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DianasMoonBow : Bow
{
    [Constructable]
    public DianasMoonBow()
    {
        Name = "Diana's Moon Bow";
        Hue = Utility.Random(150, 2350);
        MinDamage = Utility.RandomMinMax(20, 65);
        MaxDamage = Utility.RandomMinMax(65, 105);
        Attributes.LowerRegCost = 20;
        Attributes.IncreasedKarmaLoss = -10;
        Slayer = SlayerName.Fey;
        Slayer2 = SlayerName.Repond;
        WeaponAttributes.HitLeechStam = 20;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
        SkillBonuses.SetValues(1, SkillName.AnimalTaming, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DianasMoonBow(Serial serial) : base(serial)
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
