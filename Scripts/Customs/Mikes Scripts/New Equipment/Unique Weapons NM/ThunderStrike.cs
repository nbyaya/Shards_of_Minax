using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThunderStrike : WarHammer
{
    [Constructable]
    public ThunderStrike()
    {
        Name = "Thunder Strike";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 70);
        MaxDamage = Utility.RandomMinMax(80, 150);
        Attributes.BonusStr = 25;
        Attributes.WeaponSpeed = 20;
        Attributes.WeaponDamage = 50;
        Slayer = SlayerName.ElementalHealth;
        WeaponAttributes.HitLightning = 60;
        WeaponAttributes.HitDispel = 30;
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThunderStrike(Serial serial) : base(serial)
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
