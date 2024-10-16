using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SorcerersEnchantedLeggings : PlateLegs
{
    [Constructable]
    public SorcerersEnchantedLeggings()
    {
        Name = "Sorcerer's Enchanted Leggings";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(55, 70);
        ArmorAttributes.MageArmor = 1;
        ArmorAttributes.SelfRepair = 20;
        Attributes.BonusMana = 40;
        Attributes.RegenMana = 5;
        Attributes.CastSpeed = 2;
        SkillBonuses.SetValues(0, SkillName.Meditation, 40.0);
        SkillBonuses.SetValues(1, SkillName.Spellweaving, 40.0);
        PhysicalBonus = 15;
        FireBonus = 15;
        ColdBonus = 15;
        EnergyBonus = 25;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SorcerersEnchantedLeggings(Serial serial) : base(serial)
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
