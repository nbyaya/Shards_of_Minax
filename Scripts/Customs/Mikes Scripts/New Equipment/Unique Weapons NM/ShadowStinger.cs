using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShadowStinger : Dagger
{
    [Constructable]
    public ShadowStinger()
    {
        Name = "Shadow Stinger";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(180, 230);
        Attributes.BonusDex = 30;
        Attributes.DefendChance = 20;
        Slayer = SlayerName.ArachnidDoom;
        WeaponAttributes.HitFatigue = 40;
        WeaponAttributes.HitLowerAttack = 35;
        SkillBonuses.SetValues(0, SkillName.Stealth, 25.0);
        SkillBonuses.SetValues(1, SkillName.DetectHidden, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShadowStinger(Serial serial) : base(serial)
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
