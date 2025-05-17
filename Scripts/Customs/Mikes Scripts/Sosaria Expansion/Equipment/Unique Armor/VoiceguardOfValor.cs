using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VoiceguardOfValor : PlateGorget
{
    [Constructable]
    public VoiceguardOfValor()
    {
        Name = "Voiceguard of Valor";
        Hue = Utility.Random(1, 1000); // Customize the color of the item
        BaseArmorRating = Utility.RandomMinMax(30, 60); // Randomize base armor rating for variety

        Attributes.DefendChance = 10;  // Boost to defense chance
        Attributes.BonusStr = 10;  // Increases strength for better combat
        Attributes.BonusHits = 20;  // Extra health to stand strong in battle
        Attributes.LowerManaCost = 5;  // Slight mana reduction for spellcasters
        ArmorAttributes.SelfRepair = 5;  // Self-repair capability for durability

        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // Bonus to tactics for strategic fighting
        SkillBonuses.SetValues(1, SkillName.Chivalry, 10.0);  // Bonus to chivalry for honorable combat
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0);  // Bonus to anatomy for better understanding of foes

        PhysicalBonus = 10;  // Slight boost to physical resistance
        EnergyBonus = 5;  // Moderate resistance to energy-based attacks
        FireBonus = 5;  // Minor resistance to fire-based attacks

        XmlAttach.AttachTo(this, new XmlLevelItem());  // Attach XML level item for interaction
    }

    public VoiceguardOfValor(Serial serial) : base(serial)
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
