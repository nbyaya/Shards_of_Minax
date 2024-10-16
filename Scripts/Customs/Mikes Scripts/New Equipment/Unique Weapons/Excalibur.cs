using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Excalibur : Longsword
{
    [Constructable]
    public Excalibur()
    {
        Name = "Excalibur";
        Hue = Utility.Random(500, 2750);
        MinDamage = Utility.RandomMinMax(40, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.BonusStr = 20;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.Repond;
        WeaponAttributes.MageWeapon = 1;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Excalibur(Serial serial) : base(serial)
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
