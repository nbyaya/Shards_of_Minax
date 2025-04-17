using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SearingTouch : SpellWeaversWand
{
    [Constructable]
    public SearingTouch()
    {
        Name = "Searing Touch";
        Hue = Utility.Random(150, 2900);
        MinDamage = Utility.RandomMinMax(25, 70);
        MaxDamage = Utility.RandomMinMax(70, 105);
        Attributes.SpellDamage = 15;
        Attributes.RegenStam = 5;
        Slayer = SlayerName.FlameDousing;
        WeaponAttributes.HitFireArea = 40;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SearingTouch(Serial serial) : base(serial)
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
