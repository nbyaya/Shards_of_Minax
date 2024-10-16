using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TempestsRetch : Crossbow
{
    [Constructable]
    public TempestsRetch()
    {
        Name = "Tempest's Reach";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.AttackChance = 20;
        Attributes.WeaponDamage = 35;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.Fey;
        WeaponAttributes.HitLightning = 50;
        WeaponAttributes.HitFatigue = 30;
        SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TempestsRetch(Serial serial) : base(serial)
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
