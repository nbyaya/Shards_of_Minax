using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BeastsMastersEdge : Cleaver
{
    [Constructable]
    public BeastsMastersEdge()
    {
        Name = "Beastmaster's Edge";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 25;
        Attributes.BonusDex = 15;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.Repond;
        WeaponAttributes.HitFatigue = 50;
        WeaponAttributes.HitFireball = 25;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0);
        SkillBonuses.SetValues(2, SkillName.Veterinary, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BeastsMastersEdge(Serial serial) : base(serial)
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
