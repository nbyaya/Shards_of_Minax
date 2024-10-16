using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RuneEtchedBlade : VikingSword
{
    [Constructable]
    public RuneEtchedBlade()
    {
        Name = "Rune Etched Blade";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 20;
        Attributes.LowerManaCost = 15;
        Attributes.SpellDamage = 10;
        Slayer = SlayerName.Ophidian;
        WeaponAttributes.MageWeapon = -10;
        WeaponAttributes.ResistPoisonBonus = 15;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RuneEtchedBlade(Serial serial) : base(serial)
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
