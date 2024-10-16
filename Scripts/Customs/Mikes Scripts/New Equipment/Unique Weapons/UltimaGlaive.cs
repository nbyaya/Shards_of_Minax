using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class UltimaGlaive : Halberd
{
    [Constructable]
    public UltimaGlaive()
    {
        Name = "Ultima Glaive";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(80, 130);
        Attributes.LowerManaCost = 10;
        Attributes.SpellDamage = 5;
		Attributes.SpellChanneling = 1;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.MageWeapon = 1;
        WeaponAttributes.HitLeechHits = 70;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public UltimaGlaive(Serial serial) : base(serial)
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
