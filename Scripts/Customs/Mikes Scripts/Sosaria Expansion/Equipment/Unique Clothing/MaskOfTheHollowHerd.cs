using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MaskOfTheHollowHerd : HornedTribalMask
{
    [Constructable]
    public MaskOfTheHollowHerd()
    {
        Name = "Mask of the Hollow Herd";
        Hue = 1109;  // earthy hue, matching tribal theme
        
        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 5;
        Attributes.BonusHits = 25;
        Attributes.BonusStam = 15;
        Attributes.BonusMana = 10;

        // Resistances (natural, defensive, fitting the "savage" theme)
        Resistances.Physical = 15;
        Resistances.Fire = 5;
        Resistances.Cold = 10;
        Resistances.Poison = 20;
        Resistances.Energy = 5;

        // Skill Bonuses (aligning with nature and animal lore, plus combat)
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0);  // Expert animal lore, interpreting the herd
        SkillBonuses.SetValues(1, SkillName.Herding, 15.0);     // Natural herd manipulation, increased control
        SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0);  // Enhances your healing abilities for animals
        SkillBonuses.SetValues(3, SkillName.Tracking, 10.0);    // Enhanced tracking of animal herds or foes
        SkillBonuses.SetValues(4, SkillName.Tactics, 5.0);      // Provides small combat advantage in battle with beasts

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MaskOfTheHollowHerd(Serial serial) : base(serial)
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
