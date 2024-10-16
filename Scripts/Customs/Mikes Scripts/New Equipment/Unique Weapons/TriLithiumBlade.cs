using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TriLithiumBlade : Longsword
{
    [Constructable]
    public TriLithiumBlade()
    {
        Name = "Tri-lithium Blade";
        Hue = Utility.Random(250, 2900);
        MinDamage = Utility.RandomMinMax(35, 65);
        MaxDamage = Utility.RandomMinMax(65, 95);
        Attributes.SpellDamage = 10;
        Attributes.DefendChance = 5;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitLightning = 25;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TriLithiumBlade(Serial serial) : base(serial)
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
