using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RuneAxe : BattleAxe
{
    [Constructable]
    public RuneAxe()
    {
        Name = "Rune Axe";
        Hue = Utility.Random(300, 2900);
        MinDamage = Utility.RandomMinMax(25, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.SpellChanneling = 1;
        Attributes.LowerManaCost = 15;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RuneAxe(Serial serial) : base(serial)
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
