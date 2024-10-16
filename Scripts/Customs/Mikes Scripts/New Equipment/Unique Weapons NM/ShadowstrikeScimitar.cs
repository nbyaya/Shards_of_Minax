using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShadowstrikeScimitar : Scimitar
{
    [Constructable]
    public ShadowstrikeScimitar()
    {
        Name = "Shadowstrike Scimitar";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 30;
        Attributes.Luck = 150;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitHarm = 40;
        WeaponAttributes.HitLowerAttack = 35;
        SkillBonuses.SetValues(0, SkillName.Hiding, 20.0);
        SkillBonuses.SetValues(1, SkillName.Swords, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShadowstrikeScimitar(Serial serial) : base(serial)
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
