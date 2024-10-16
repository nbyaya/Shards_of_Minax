using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BowmastersRingmailArms : RingmailArms
{
    [Constructable]
    public BowmastersRingmailArms()
    {
        Name = "Bowmaster's Ringmail Arms";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(75, 100);
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusStam = 25;
        Attributes.WeaponDamage = 20;
        SkillBonuses.SetValues(0, SkillName.Archery, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 35.0);
        SkillBonuses.SetValues(2, SkillName.AnimalTaming, 30.0);
        PhysicalBonus = 10;
        EnergyBonus = 15;
        FireBonus = 20;
        ColdBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BowmastersRingmailArms(Serial serial) : base(serial)
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
