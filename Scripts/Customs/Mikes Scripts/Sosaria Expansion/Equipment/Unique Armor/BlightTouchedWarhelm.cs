using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BlightTouchedWarhelm : EvilOrcHelm
{
    [Constructable]
    public BlightTouchedWarhelm()
    {
        Name = "Blight-Touched Warhelm";
        Hue = Utility.Random(1000, 1200); // A dark, war-torn hue
        BaseArmorRating = Utility.RandomMinMax(30, 80); // A strong defensive item

        // Armor attributes with thematic effects
        ArmorAttributes.SelfRepair = 5; // Minor self-repair to symbolize resilience in battle
        Attributes.BonusStr = 15; // Increases strength, fitting for an orc warrior
        Attributes.BonusDex = 10; // Increases dexterity for quicker reactions in combat
        Attributes.BonusInt = 5; // Minor increase in intelligence to signify dark intelligence
        Attributes.DefendChance = 10; // Adds defensive chance to give the wearer better protection

        // Skill bonuses that tie into the blight and evil orc theme
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0); // Orcs are skilled in tactics for battle
        SkillBonuses.SetValues(1, SkillName.Wrestling, 15.0); // Wrestling to reflect brutal combat styles
        SkillBonuses.SetValues(2, SkillName.Poisoning, 10.0); // Poisoning bonus to reflect the dark, toxic nature of the helm

        // Elemental resistances to make the armor more robust
        PoisonBonus = 20; // High poison resistance due to the helm's blight origin
        PhysicalBonus = 10; // Moderate physical protection

        // A unique effect due to the helm's dark origin
        Attributes.LowerManaCost = 10; // Reduces mana costs, allowing the wearer to utilize spells with more efficiency
        Attributes.SpellDamage = 5; // Adds a slight increase in spell damage to enhance magical effectiveness

        // Attach the XML item level for persistence and uniqueness in the world
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BlightTouchedWarhelm(Serial serial) : base(serial)
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
