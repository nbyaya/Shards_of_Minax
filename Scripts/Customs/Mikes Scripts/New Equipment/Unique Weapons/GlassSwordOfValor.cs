using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GlassSwordOfValor : Longsword
{
    [Constructable]
    public GlassSwordOfValor()
    {
        Name = "Glass Sword of Valor";
        Hue = Utility.Random(50, 2900);
        MinDamage = 500;
        MaxDamage = 1000;
        Attributes.BonusStr = 50;
        Attributes.Luck = 250;
        Slayer = SlayerName.OrcSlaying;
        Slayer2 = SlayerName.DragonSlaying;
        WeaponAttributes.SelfRepair = -1;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GlassSwordOfValor(Serial serial) : base(serial)
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
