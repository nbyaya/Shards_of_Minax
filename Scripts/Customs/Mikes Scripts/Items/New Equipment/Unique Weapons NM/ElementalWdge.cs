using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ElementalWdge : SkinningKnife
{
    [Constructable]
    public ElementalWdge()
    {
        Name = "Elemental Edge";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 20;
        Attributes.SpellDamage = 25;
        Slayer = SlayerName.ElementalBan;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitEnergyArea = 40;
        WeaponAttributes.MageWeapon = -10;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ElementalWdge(Serial serial) : base(serial)
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
