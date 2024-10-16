using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NebulaBow : Bow
{
    [Constructable]
    public NebulaBow()
    {
        Name = "Nebula Bow";
        Hue = Utility.Random(25, 35);
        MinDamage = Utility.RandomMinMax(40, 85);
        MaxDamage = Utility.RandomMinMax(85, 130);
        Attributes.SpellChanneling = 1;
        Attributes.Luck = 200;
        Slayer = SlayerName.ElementalBan;
        Slayer2 = SlayerName.Vacuum;
        WeaponAttributes.HitFireball = 50;
        WeaponAttributes.HitLightning = 30;
        SkillBonuses.SetValues(0, SkillName.Archery, 30.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 65.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NebulaBow(Serial serial) : base(serial)
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
