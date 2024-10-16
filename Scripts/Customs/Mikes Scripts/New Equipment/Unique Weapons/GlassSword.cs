using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GlassSword : Longsword
{
    [Constructable]
    public GlassSword()
    {
        Name = "Glass Sword";
        Hue = 2344;
        MinDamage = 1;
        MaxDamage = 200;
        Attributes.Luck = 150;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Swords, 30.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GlassSword(Serial serial) : base(serial)
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
