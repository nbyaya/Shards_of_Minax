using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NecromancersBoneGrips : BoneGloves
{
    [Constructable]
    public NecromancersBoneGrips()
    {
        Name = "Necromancer's BoneGrips";
        Hue = Utility.Random(10, 250);
        BaseArmorRating = Utility.RandomMinMax(35, 70);
        AbsorptionAttributes.EaterFire = 15;
        ArmorAttributes.MageArmor = 1;
        Attributes.CastRecovery = 2;
        Attributes.RegenMana = 5;
        SkillBonuses.SetValues(0, SkillName.Meditation, 20.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 15;
        PhysicalBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NecromancersBoneGrips(Serial serial) : base(serial)
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
