using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarvestersFury : Pitchfork
{
    [Constructable]
    public HarvestersFury()
    {
        Name = "Harvester's Fury";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.BonusStam = 20;
        Attributes.AttackChance = 25;
        Slayer = SlayerName.TrollSlaughter;
        Slayer2 = SlayerName.LizardmanSlaughter;
        WeaponAttributes.HitLightning = 40;
        WeaponAttributes.HitLowerAttack = 25;
        SkillBonuses.SetValues(0, SkillName.Fencing, 15.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarvestersFury(Serial serial) : base(serial)
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
