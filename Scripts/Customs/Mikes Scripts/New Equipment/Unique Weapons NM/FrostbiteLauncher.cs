using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostbiteLauncher : Crossbow
{
    [Constructable]
    public FrostbiteLauncher()
    {
        Name = "Frostbite Launcher";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 20;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitColdArea = 45;
        WeaponAttributes.ResistColdBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(2, SkillName.Stealth, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostbiteLauncher(Serial serial) : base(serial)
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
