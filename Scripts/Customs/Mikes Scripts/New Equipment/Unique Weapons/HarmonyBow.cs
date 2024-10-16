using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarmonyBow : Bow
{
    [Constructable]
    public HarmonyBow()
    {
        Name = "Harmony Bow";
        Hue = Utility.Random(200, 2900);
        MinDamage = Utility.RandomMinMax(15, 55);
        MaxDamage = Utility.RandomMinMax(55, 85);
        Attributes.LowerRegCost = 10;
        Attributes.Luck = 100;
        Slayer = SlayerName.Fey;
        WeaponAttributes.HitLeechMana = 20;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Peacemaking, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarmonyBow(Serial serial) : base(serial)
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
